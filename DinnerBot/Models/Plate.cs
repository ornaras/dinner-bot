using System.Text.Json.Nodes;

namespace DinnerBot.Models
{
    internal readonly struct Plate(JsonNode src)
    {
        public int Id { get; } = src["id"].GetValue<int>();
        public int CategoryId { get; } = int.Parse(src["categoryID"].GetValue<string>());
        public string Name { get; } = src["name"].GetValue<string>();
        public string PictureUrl { get; } = src["thumbnailPicture"].GetValue<string>();
        public string Mass { get; } = src["values"][0]["mass"].GetValue<string>();
        public uint Price { get; } = uint.Parse(src["values"][0]["price"].GetValue<string>());
    }
}
