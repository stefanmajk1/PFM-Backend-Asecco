using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PFMBackend.Models.Category
{
    public class Category
    {
        [Required]
        [ReadOnly(true)]
        [JsonProperty("code")]
        public string Code { get; set; }
        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("parent-code")]
        public string ParentCode { get; set; }
    }
}
