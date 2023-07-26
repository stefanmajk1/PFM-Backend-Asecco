using Newtonsoft.Json;

namespace PFMBackend.Problem
{
    public class ValidationProblem : Problem
    {
        [JsonProperty("errors")]
        public List<Errors> Errors { get; set; }//lista errora, koju cemo puniti i vracati
    }
}
