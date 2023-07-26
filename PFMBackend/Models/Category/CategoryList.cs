using Newtonsoft.Json;

namespace PFMBackend.Models.Category
{
    public class CategoryList<T>
    {
        [JsonProperty("items")]
        public List<T> Items { get; set; }
    }
}
