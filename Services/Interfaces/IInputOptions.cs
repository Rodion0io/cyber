using hospital_api.Modules;

namespace hospital_api.Services.Interfaces;

public interface IInputOptions
{
    public List<InspectionPreviewModel> GetFilteringGroupInspection(List<InspectionPreviewModel> inspections,
        bool isGrouped);

    // public List<InspectionPreviewModel> GetFilteringByParentCode(List<InspectionPreviewModel> inspections,
    //     List<Guid> rootElementsCode);
}