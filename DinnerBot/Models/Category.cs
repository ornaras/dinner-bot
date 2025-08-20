using System.Collections.ObjectModel;
using System.Text.Json.Nodes;

namespace DinnerBot.Models
{
    public readonly struct Category(JsonNode src, IEnumerable<Plate> plates)
    {
        public int Id { get; } = int.Parse(src["id"].GetValue<string>());
        public string Name { get; } = src["name"].GetValue<string>();
        public ReadOnlyCollection<Plate> Plates { get; } = 
            Array.AsReadOnly([.. plates.Where(p => p.CategoryId == int.Parse(src["id"].GetValue<string>())).OrderBy(p => p.Id)]);
    }
}
