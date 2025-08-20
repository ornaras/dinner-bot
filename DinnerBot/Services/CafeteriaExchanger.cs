using DinnerBot.Models;
using System.Collections.ObjectModel;
using System.Text.Json.Nodes;

namespace DinnerBot.Services;

public class CafeteriaExchanger(ILogger<CafeteriaExchanger> logger)
{
    const string HOST = "https://borsh-panel.s2.sellkit.ru";
    internal async Task<ReadOnlyCollection<Category>> OpenCatalog()
    {
        using var http = new HttpClient();
        using var req = new HttpRequestMessage(HttpMethod.Post, $"{HOST}/NewApi/Content/GetCatalog.php");
        req.Headers.Add("Authorization", "guest_token");
        req.Content = new MultipartFormDataContent
        {
            { new StringContent("true"), "web" },
            { new StringContent("0"), "OrganisationID" }
        };
        using var resp = await http.SendAsync(req);
        var content = await resp.Content.ReadAsStringAsync();
        var json = JsonNode.Parse(content);
        if (json is null) return Array.AsReadOnly(Array.Empty<Category>());

        var jArray = json["plates"]?.AsArray() ?? [];
        var plates = new Plate[jArray.Count];
        for(var i = 0; i < plates.Length; i++)
            plates[i] = new Plate(jArray[i]);

        jArray = json["categories"]?.AsArray() ?? [];
        var categories = new Category[jArray.Count];
        for (var i = 0; i < categories.Length; i++)
            categories[i] = new Category(jArray[i], plates);

        categories = [.. categories.Where(c => c.Plates.Count > 0)];
        return Array.AsReadOnly(categories);
    }
}
