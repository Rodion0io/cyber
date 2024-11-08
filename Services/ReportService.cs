using System.Globalization;
using hospital_api.Dates;
using hospital_api.Enums;
using hospital_api.Modules;
using hospital_api.Repositories.repositoryInterfaces;
using hospital_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace hospital_api.Services;

public class ReportService : IReportService
{
    private readonly AccountsContext _context;
    private readonly IInputOptions _inputOptions;
    private readonly IDictionaryRepository _dictionaryRepository;

    public ReportService(AccountsContext context, IInputOptions inputOptions,
        IDictionaryRepository dictionaryRepository)
    {
        _context = context;
        _inputOptions = inputOptions;
        _dictionaryRepository = dictionaryRepository;
    }

    public async Task<IcdRootsReportModel> GetReportModel(DateTime start, DateTime end,
        List<Guid> icdRoot)
    {
        int totalCountInspection = 0;
        
        // var copy = _context.Inspections.ToList();
        
        var copy = _context.Inspections.Select(i => new InspectionPreviewModel
        {
            id = i.id,
            createTime = i.createTime,
            previousId = i.previousInspectionId,
            date = i.date,
            conclusion = i.conclusion,
            doctorId = i.doctor,
            doctor = _context.Doctors.Where(x => x.id == i.doctor)
                .Select(x => x.name).FirstOrDefault(),
            patientId = i.patient,
            patient = _context.Patients.Where(x => x.id == i.patient)
                .Select(x => x.name).FirstOrDefault(),
            diagnosis = _context.Diagnosis.Where(x => x.inspectionId == i.id)
                .Select(x => new DiagnosisModel
                {
                    id = x.id,
                    createTime = x.createTime,
                    code = x.code,
                    name = x.name,
                    description = x.description,
                    type = x.type
                }).FirstOrDefault(),
                
            hasChain = false,
            hasNested = false

        }).ToList();

        copy = copy.Where(i => start <= i.date && i.date <= end).ToList();
        
        copy = await _inputOptions.GetFilteringByParentCode(copy, icdRoot);

        IcdRootsReportFiltersModel reportFilter = new IcdRootsReportFiltersModel
        {
            start = start,
            end = end,
            icdRoots = new List<string>()
        };

        foreach (var value in icdRoot)
        {
            var nameIcd = await _dictionaryRepository.getIcd10Code(value);
            reportFilter.icdRoots.Add(nameIcd);
        }
        

        // Создаем список словарей, где каждый словарь представляет собой пару "пациент - код диагноза"
        var patientDiagnosesList = new List<Dictionary<string, object>>();

        foreach (var patient in copy)
        {
            var twoSymbolsRecCode = await _dictionaryRepository.getTwoSymbolsRecCode(patient.diagnosis.code);
            var recCode = icdRoot.FirstOrDefault(i => _dictionaryRepository.getRecCodeParent(i).Result == twoSymbolsRecCode);
            var icdCode = await _dictionaryRepository.getIcd10Code(recCode);

            // Ищем, есть ли уже такой пациент в списке
            var patientDict = patientDiagnosesList.FirstOrDefault(p => p["Patient"].ToString() == patient.patient);

            if (patientDict == null)
            {
                // Если пациента еще нет в списке, создаем новый словарь
                patientDict = new Dictionary<string, object>
                {
                    { "Patient", patient.patientId },
                    { "Diagnoses", new Dictionary<string, int>() }
                };
                patientDiagnosesList.Add(patientDict);
            }

            // Получаем словарь диагнозов для текущего пациента
            var diagnosesDict = (Dictionary<string, int>)patientDict["Diagnoses"];

            // Увеличиваем счетчик для текущего диагноза
            if (diagnosesDict.ContainsKey(icdCode))
            {
                diagnosesDict[icdCode]++;
            }
            else
            {
                diagnosesDict[icdCode] = 1;
            }
        }
        
        
        List<IcdRootsReportRecordModel> list = new List<IcdRootsReportRecordModel>();


        foreach (var value in patientDiagnosesList)
        {
            var patientId = (Guid)value["Patient"];
            var diagnosesDict = (Dictionary<string, int>)value["Diagnoses"];
            
            // это венсити в репозиторий
            string patientName = _context.Patients.FirstOrDefaultAsync(i => i.id == patientId).Result.name;
            Gender patientGender = _context.Patients.FirstOrDefaultAsync(i => i.id == patientId).Result.gender;
            DateTime birthday = _context.Patients.FirstOrDefaultAsync(i => i.id == patientId).Result.birthday;

            IcdRootsReportRecordModel model = new IcdRootsReportRecordModel
            {
                patientName = patientName,
                patientBirthdate = birthday,
                gender = patientGender,
                visitsByRoot = diagnosesDict
            };
            
            list.Add(model);
        }

        
        IcdRootsReportModel result = new IcdRootsReportModel
        {
            filters = reportFilter,
            records = list.ToArray(),
            // summaryByRoot = 
        };
        return result;
    }
}