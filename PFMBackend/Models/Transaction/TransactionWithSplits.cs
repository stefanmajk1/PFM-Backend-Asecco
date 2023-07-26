using Newtonsoft.Json;
using PFMBackend.Models.Category;
using PFMBackend.Models.Transaction.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Newtonsoft.Json.Converters;
using PFMBackend.JSON;

namespace PFMBackend.Models.Transaction
{
    public class TransactionWithSplits
    {
        [Required]
        [ReadOnly(true)]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("beneficiary-name")]
        public string BeneficiaryName { get; set; }

        [JsonProperty("date")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]//format godine
        public DateTime Date { get; set; }

        [JsonProperty("direction")]
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]//konvertovanje iz json u string
        public DirectionsEnum Direction { get; set; }

        [JsonProperty("amount")]
        [Required]
        [JsonConverter(typeof(DoubleToStringJsonConverter))]//konvertovanje iz json u double
        public double Amount { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [MinLength(3)]
        [MaxLength(3)]
        [Required]
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("mcc")]
        public MccCodeEnum? Mcc { get; set; }

        [Required]
        [JsonProperty("kind")]
        public TransactionKindsEnum Kind { get; set; }

        [JsonProperty("catcode")]
        [ReadOnly(true)]
        public string CatCode { get; set; }

        [JsonProperty("splits")]
        public List<SingleCategorySplit> Splits { get; set; }
    }
}
