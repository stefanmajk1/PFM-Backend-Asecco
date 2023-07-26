using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PFMBackend.Problem
{
    
    public class Errors
    {//tipovi gresaka

        [Required]
        [JsonProperty("tag")]
        public string Tag { get; set; }
        [Required]
        [JsonProperty("error")]
        public ErrEnum Error { get; set; }
        [Required]
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
