using Newtonsoft.Json;

namespace PFMBackend.Models.Category
{
    public class SpendingsByCategory
    {
        [JsonProperty("groups")]
        public List<SpendingInCategory> Groups { get; set; }
    }
}
