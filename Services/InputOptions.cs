using hospital_api.Modules;
using hospital_api.Services.Interfaces;

namespace hospital_api.Services;

public class InputOptions : IInputOptions
{
    // private readonly IInputOptions _inputOptions;
    //
    // public InputOptions(IInputOptions inputOptions)
    // {
    //     _inputOptions = inputOptions;
    // }

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
            return inspections.Where(i => i.previousId != null).ToList();
        }
    }

    // public List<InspectionPreviewModel> GetFilteringByParentCode(List<InspectionPreviewModel> inspections,
    //     List<Guid> rootElementsCode)
    // {
    //     
    // }
}