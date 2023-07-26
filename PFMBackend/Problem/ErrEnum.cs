using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace PFMBackend.Problem
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ErrEnum
    {
        //opisi gresaka, i njihove vrednosti

        [Description("Value supplied does not meet minimum length")]
        [EnumMember(Value = "min-length")]
        MinLength,
        [Description("Value supplied exceeds maximum allowed length")]
        [EnumMember(Value = "max-length")]
        MaxLength,
        [Description("Mandatory field or parameter was not supplied")]
        [EnumMember(Value = "required")]
        Required,
        [Description("Value supplied was out of allowed range")]
        [EnumMember(Value = "out-of-range")]
        OutOfRange,
        [Description("Value supplied does not have expected format")]
        [EnumMember(Value = "invalid-format")]
        InvalidFormat,
        [Description("Value supplied does not belong to enumeration")]
        [EnumMember(Value = "unknown-enum")]
        UnknownEnum,
        [Description("Value supplied does not belong to classification")]
        [EnumMember(Value = "not-on-list")]
        NotOnList,
        [Description("Value supplied does not conform to check digit validation")]
        [EnumMember(Value = "check-digit-invalid")]
        CheckDigitInvalid,
        [Description("Parameter must be used with other parameters that were not supplied")]
        [EnumMember(Value = "combination-required")]
        CombinationRequired,
        [Description("Parameter is read-only")]
        [EnumMember(Value = "read-only")]
        ReadOnly
    }
}
