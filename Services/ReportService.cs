using hospital_api.Dates;
using hospital_api.Enums;
using hospital_api.Modules;
using hospital_api.Repositories.repositoryInterfaces;
using hospital_api.Services.Interfaces;

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
        
        // List<IcdRootsReportRecordModel> records = new List<IcdRootsReportRecordModel>();
        
        var records = copy
            .GroupBy(i => i.patientId)
            .Select(group =>
            {
                var patientInspections = group.ToList();
                var patient = patientInspections.First();

                var visitsByRoot = patientInspections
                    .GroupBy(i => i.diagnosis?.code)
                    .Where(g => g.Key != null)
                    .ToDictionary(g => g.Key, g => g.Count());

                return new IcdRootsReportRecordModel
                {
                    patientName = patient.patient,
                    patientBirthdate = DateTime.Now, // Предполагается, что у вас есть такое поле
                    gender = Gender.Female, // Предполагается, что у вас есть такое поле
                    visitsByRoot = visitsByRoot
                };
            })
            .ToList();

        IcdRootsReportModel result = new IcdRootsReportModel
        {
            filters = reportFilter,
            records = records.ToArray(),
            // summaryByRoot = 
        };
        return result;
    }
}