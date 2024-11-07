using hospital_api.Modules;
using hospital_api.Repositories.repositoryInterfaces;
using hospital_api.Services.Interfaces;

namespace hospital_api.Services;

public class InputOptions : IInputOptions
{
    private readonly IDictionaryRepository _dictionaryRepository;

    public InputOptions(IDictionaryRepository dictionaryRepository)
    {
        _dictionaryRepository = dictionaryRepository;
    }

    public List<InspectionPreviewModel> GetFilteringGroupInspection(List<InspectionPreviewModel> inspections, 
        bool isGrouped)
    {
        // var result;
        
        if (isGrouped)
        {
            return inspections.Where(i => i.previousId == null).ToList();
        }
        else
        {
            return inspections;
        }
    }

    public async Task<List<InspectionPreviewModel>> GetFilteringByParentCode(List<InspectionPreviewModel> inspections,
        List<Guid> rootElementsCode)
    {
        List<InspectionPreviewModel> result = new List<InspectionPreviewModel>();
        List<string> recCodes = new List<string>();

        foreach (var code in rootElementsCode)
        {
            var currentRecCode = await _dictionaryRepository.getRecCodeParent(code);
            recCodes.Add(currentRecCode);
        }

        foreach (var value in inspections)
        {
            var inspectionCode = await _dictionaryRepository.getTwoSymbolsRecCode(value.diagnosis.code);
            if (recCodes.Contains(inspectionCode))
            {
                result.Add(value);
            }
        }
        
        return result;
    }
    
}