using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdvertAPI.Models
{
    public enum AdvertStatus
    {
        Pending = 1,
        Active = 2
    }

    public class ConfirmAdvertModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("filePath")]
        public string FilePath { get; set; }
        [JsonPropertyName("status")]
        public AdvertStatus Status { get; set; }
    }
}
