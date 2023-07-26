using Newtonsoft.Json;

namespace PFMBackend.Problem
{
    public class BusinessProblem : Problem
    {
        //Tipovi problema

        [JsonProperty("problem-literal")]
        public string ProblemLiteral { get; set; }
        [JsonProperty("problem-message")]
        public string ProblemMessage { get; set; }
        [JsonProperty("problem-details")]
        public string ProblemDetails { get; set; }
    }
}
