using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PFMBackend.Models.Category
{
    public class SingleCategorySplit
    {
        [Required]
        [JsonProperty("catcode")]
        public string Catcode { get; set; }
        [Required]
        [JsonProperty("amount")]
        public double Amount { get; set; }
    }
}
