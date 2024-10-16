using System.ComponentModel.DataAnnotations;
using hospital_api.Dates;
using hospital_api.Modules;
using hospital_api.Repositories;

namespace hospital_api.CustomValidation;

public class IsSpeciality : ValidationAttribute
{
    
    public override bool IsValid(object? value)
    {
        string[] arr =
        {
            "e8f93a49-b93f-47f0-a912-08dbffad6d0e", "302d5c0c-5623-4810-a913-08dbffad6d0e",
            "2c4b19f5-511d-4f27-a914-08dbffad6d0e", "4676b2f4-de54-4fce-a915-08dbffad6d0e",
            "b0f1d7c7-18e5-488b-a916-08dbffad6d0e", "6cb7fe40-bafe-49bc-a917-08dbffad6d0e",
            "75735935-74d3-4fa2-a918-08dbffad6d0e", "ed1b936e-9c67-4da6-a919-08dbffad6d0e",
            "87a9c38c-0d2d-4a52-a91a-08dbffad6d0e", "5aa83ee6-9bb0-4afe-a91b-08dbffad6d0e",
            "6c20f45d-a7d1-4605-a91c-08dbffad6d0e", "dfcc00ff-6595-41ad-a91d-08dbffad6d0e",
            "bf1f4b00-cf9c-48e4-a91e-08dbffad6d0e", "9ea305d2-b1f8-405e-a91f-08dbffad6d0e",
            "d82c6890-d26d-450b-a920-08dbffad6d0e", "2e73cece-5fda-4211-a921-08dbffad6d0e",
            "bec96e6f-8673-47c9-a922-08dbffad6d0e", "15e97e43-315c-44b5-a923-08dbffad6d0e"
        };
        
        if (arr.Contains(value.ToString()))
        {
            return true;
        }

        return false;

    }
}