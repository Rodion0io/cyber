using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using hospital_api.Dates;
using hospital_api.Modules;
using hospital_api.Repositories.repositoryInterfaces;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace hospital_api.Services;

public class DictionaryServic : IDictionaryServic
{
    private readonly IDictionaryRepository _dictionaryRepository;

    public DictionaryServic(IDictionaryRepository dictionaryRepository)
    {
        _dictionaryRepository = dictionaryRepository;
    }

    public async Task<List<SpecialityModel>> GetFullSpeciaityTable()
    {
        return await _dictionaryRepository.getFullListSpeciality();
    }

    public async Task AddIcd10ModelsFromJson(string jsonFilePath)
    {
        string jsonString = await File.ReadAllTextAsync(jsonFilePath);
    
        using JsonDocument document = JsonDocument.Parse(jsonString);
        JsonElement root = document.RootElement;

        foreach (JsonElement element in root.EnumerateArray())
        {
            string recCode = null;
            string parentId = null;
            DateTime? mkbData = null;
            if (element.TryGetProperty("REC_CODE", out JsonElement mkbCodeElement))
            {
                recCode = mkbCodeElement.GetString();
            }

            if (element.TryGetProperty("ID_PARENT", out JsonElement mkbParentId))
            {
                parentId = mkbParentId.GetString();
            }

            if (element.TryGetProperty("DATE", out JsonElement dateElement))
            {
                mkbData = DateTime.ParseExact(dateElement.GetString(), "dd.MM.yyyy",
                    System.Globalization.CultureInfo.InvariantCulture);
                mkbData = DateTime.SpecifyKind(mkbData.Value, DateTimeKind.Utc);
            }

            Icd10Model model = new Icd10Model
            {
                id = element.GetProperty("ID").GetInt32(),
                code = element.GetProperty("MKB_CODE").GetString(),
                name = element.GetProperty("MKB_NAME").GetString(),
                recCode = recCode,
                parentId = parentId,
                actual = element.GetProperty("ACTUAL").GetInt32(),
                date = mkbData
            };
            await _dictionaryRepository.AddIcd10Models(model);
        }
    }

    public async Task<int> returnLenghtTable()
    {
        return await _dictionaryRepository.GetSizeTable();
    }

    //Так стоит делать?
    public async Task<List<Icd10RecordModel>> FullListIcd()
    {
        return await _dictionaryRepository.getFullListIcd10();
    }

    public async Task<List<Icd10RecordModel>> FullListIcdRoots()
    {
        return await _dictionaryRepository.getFullListIcd10Roots();
    }
}