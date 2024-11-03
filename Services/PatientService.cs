using hospital_api.Dates;
using hospital_api.Enums;
using hospital_api.Modules;
using hospital_api.Repositories.repositoryInterfaces;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hospital_api.Services;

public class PatientService : IPatientService
{
    //Надо будет вынести
    private readonly AccountsContext _context;
    private readonly IPatientRepository _patientRepository;
    private readonly IDictionaryRepository _dictionaryRepository;

    public PatientService(AccountsContext context, IPatientRepository patientRepository,
        IDictionaryRepository dictionaryRepository)
    {
        _context = context;
        _patientRepository = patientRepository;
        _dictionaryRepository = dictionaryRepository;
    }

    public async Task RegistrationPatient(PatientCreateModel model)
    {
        PatientModel patient = new()
        {
            name = model.name,
            birthday = model.birthday,
            gender = model.gender
        };
    
        await _patientRepository.AddPatient(patient);
    }
    
    public async Task<PatientModel> GetPatient(string id)
    {
        return await _patientRepository.FindPatient(id);
    }

    public async Task<string> AddInpection(InspectionCreateModel model, Guid patintId, Guid doctorId, string doctorName)
    {
        
        Inspection newInspection = new Inspection
        {
            date = model.date,
            anamnesis = model.anamnesis,
            complaints = model.complaints,
            treatment = model.treatment,
            conclusion = model.conclusion,
            nextVisitDate = model.nextVisitDate,
            deathDate = model.deathDate,
            previousInspectionId = model.previousInspectionId,
            patient = patintId,
            doctor = doctorId
        };

        await _patientRepository.AddInspection(newInspection);

        newInspection.baseInspectionId = (newInspection.previousInspectionId == null ? newInspection.id : 
            (await _patientRepository.FindInspection(newInspection.previousInspectionId)).baseInspectionId);
        
        foreach (var diagnosis in model.diagnosis)
        {
            Diagnosis newDiagnosis = new Diagnosis
            {
                code = await _dictionaryRepository.getIcd10Code((diagnosis.icdDiagnosisId)),
                name = await _dictionaryRepository.getIcd10Name((diagnosis.icdDiagnosisId)),
                description = diagnosis.description,
                type = diagnosis.type,
                icdDiagnosisId = diagnosis.icdDiagnosisId,
                inspectionId = newInspection.id
            };

            await _patientRepository.AddDiagnosis(newDiagnosis);
        }
        
        foreach (var comment in model.consultations)
        {
            Consultation newConsultation = new Consultation
            {
                inspectionId = newInspection.id,
                specialityId = comment.specialityId
            };
            
            await _patientRepository.AddConsultation(newConsultation);
        
            Comment newComment = new Comment
            {
                content = comment.comment.content,
                authorId = doctorId,
                author = doctorName,
                consultationId = newConsultation.id
            };
            await _patientRepository.AddComments(newComment);
        }

        return (newInspection.id).ToString();
    }

    // //Так можно задвать тип функции? Или нужно использовать Task?
    public bool checkPrevInspection(InspectionCreateModel model)
    {
        if (model.previousInspectionId != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> checkTimeNewInspection(InspectionCreateModel model)
    {

        if (model.previousInspectionId != null)
        {
            var prevTime = await _patientRepository.FindInspection(model.previousInspectionId);
    
            if (prevTime != null)
            {
                int compareTime = DateTime.Compare(model.date, prevTime.date);
                if (compareTime > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        
    }
    
    public int CheckConclusion(InspectionCreateModel model)
    {
        if ((model.conclusion == Conclusion.Disease && model.nextVisitDate == null && model.deathDate == null) ||
            (model.conclusion == Conclusion.Disease && model.nextVisitDate == null && model.deathDate  != null))
        {
            // return BadRequest("Если диагноз 'выздоровление', дата следующего визита и дата смерти не могут быть установлены.")
            return 1;
        }
        else if ((model.conclusion == Conclusion.Death && model.nextVisitDate != null && model.deathDate  == null) ||
                 (model.conclusion == Conclusion.Death && model.nextVisitDate != null && model.deathDate  != null))
        {
            // return BadRequest("Если диагноз болен, то нужно назначить дату следующего визита и даты смерти быть не может");
            return 2;
        }
        else if (model.conclusion == Conclusion.Recovery && (model.nextVisitDate != null || model.deathDate != null))
        {
            // return BadRequest("Если диагноз болен, то нужно назначить дату следующего визита и даты смерти быть не может");
            return 3;
        }
        else
        {
            return 4;
        }
    }
    
    public bool CheckTypeDiagnosis(InspectionCreateModel model)
    {
        int count = 0;
        foreach (var diagnos in model.diagnosis)
        {
            if (diagnos.type == DiagnosisType.Main)
            {
                count++;
            }
    
            if (count > 1)
            {
                break;
            }
        }
    
        return count == 1;
    }
    
    public bool CheckAllSpecialities(InspectionCreateModel model)
    {
        int flag = 1;
        string prevSpecialityId = null;
        foreach (var value in model.consultations)
        {
            if ((value.specialityId).ToString() == prevSpecialityId)
            {
                flag = 0;
                break;
            }
            else
            {
                prevSpecialityId = value.specialityId.ToString();
            }
        }
    
        return flag == 1;
    }

    public async Task<bool> CheckDethPatient(InspectionCreateModel model)
    {

        if (model.previousInspectionId != null)
        {
            var status = await _patientRepository.FindInspection(model.previousInspectionId);
    
            if (status.conclusion == Conclusion.Death)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public async Task<InspectionShortModel[]> GetInspectionWithoutChild(Guid patientId, string? partName)
    {
        List<InspectionShortModel> inspections = new List<InspectionShortModel>();
        Inspection[] allInspections = await _patientRepository.GetInspetionWithoutChild(patientId);

        if (allInspections.Length == 0)
        {
            throw new Exception("Нет осмотров или нет такого пациента!");
        }
        else
        {
            foreach (var value in allInspections)
            {
                List<DiagnosisModel> diagnosis = await _patientRepository.GetDiagnosisInspectionWithUotChild(value.id, partName);

                foreach (var x in diagnosis)
                {
                    InspectionShortModel shortInspection = new InspectionShortModel
                    {
                        id = value.id,
                        createTime = value.createTime,
                        date = value.date,
                        diagnosis = x
                    };
                    inspections.Add(shortInspection);
                }
            }
        }
        
        return inspections.ToArray();
    }


    public async Task<List<PatientModel>> GetFilteringPatient(string? name, List<Conclusion> conclusions,
    SortPatient? sorting,
    bool? scheduledVisits, bool? onlyMine, Guid doctorId)
    {
        var сopy = _context.Inspections.ToList();

        if (name != null)
        {
            var patientIds = await _patientRepository.GetPatientName(name);
            сopy = сopy.Where(i => patientIds.Contains(i.patient)).ToList();
        }
        
        if (onlyMine != null && onlyMine != false)
        {
            сopy = сopy.Where(i => i.doctor == doctorId).ToList();
        }
        
        if (conclusions.Count != 0)
        {
            сopy = сopy.Where(i => conclusions.Contains(i.conclusion)).ToList();
        }
        
        if (scheduledVisits != null && scheduledVisits != false)
        {
            var maxDates = сopy
                .GroupBy(i => i.patient)
                //Эту строчку выдал чат
                .Select(g => new { Patient = g.Key, MaxDate = g.Max(i => i.date) })
                .ToList();
            сopy = сopy.Where(i => i.nextVisitDate != null &&
                                   maxDates.Any(md => md.Patient == i.patient && i.nextVisitDate > md.MaxDate)).ToList();
        }

        var patientsWithNames = new List<(Inspection Inspection, string PatientName)>();
        
        switch (sorting)
        {
            case SortPatient.NameAsc:
                foreach (var inspection in сopy)
                {
                    var patient = await _patientRepository.FindPatient(inspection.patient.ToString());
                    patientsWithNames.Add((inspection, patient.name));
                }

                сopy = sorting == SortPatient.NameAsc
                    ? patientsWithNames.OrderBy(x => x.PatientName).Select(x => x.Inspection).ToList()
                    : patientsWithNames.OrderBy(x => x.PatientName).Select(x => x.Inspection).ToList();
                break;
            case SortPatient.NameDesc:
                
                foreach (var inspection in сopy)
                {
                    var patient = await _patientRepository.FindPatient(inspection.patient.ToString());
                    patientsWithNames.Add((inspection, patient.name));
                }

                сopy = sorting == SortPatient.NameAsc
                    ? patientsWithNames.OrderBy(x => x.PatientName).Select(x => x.Inspection).ToList()
                    : patientsWithNames.OrderByDescending(x => x.PatientName).Select(x => x.Inspection).ToList();
                break;
            case SortPatient.CreateAsc:
                сopy = сopy.OrderBy(i => i.createTime).ToList();
                break;
            case SortPatient.CreateDesc:
                сopy = сopy.OrderByDescending(i => i.createTime).ToList();
                break;
            case SortPatient.InspectionAsc:
                сopy = сopy.OrderBy(i => i.date).ToList();
                break;
            case SortPatient.InspectionDesc:
                сopy = сopy.OrderByDescending(i => i.date).ToList();
                break;
        }

        var patientList = new List<PatientModel>();
        foreach (var inspection in сopy)
        {
            patientList.Add(await _patientRepository.FindPatient(inspection.patient.ToString()));
        }

        var result = patientList.GroupBy(x => x)
            .Select(y => y.First()).ToList();

        return result;
    }
}