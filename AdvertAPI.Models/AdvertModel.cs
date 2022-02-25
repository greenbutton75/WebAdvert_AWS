using System.Text.Json.Serialization;

namespace AdvertAPI.Models
{
    public class AdvertModel
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("price")]
        public double Price { get; set; }
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        [JsonPropertyName("filePath")]
        public string? FilePath { get; set; }
        [JsonPropertyName("id")]
        public string? Id { get; set; }
    }
}