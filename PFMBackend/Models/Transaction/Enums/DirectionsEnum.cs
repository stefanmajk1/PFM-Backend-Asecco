using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace PFMBackend.Models.Transaction.Enums
{
    /*JsonConverter se primenjuje na enumericijama kako bi se odredilo kako ce se
     konvertovati enumeracijske vrednosti prilikom serijalizaije u JSON froman ili
    deserijalizacije iz JSON formata*/
    [JsonConverter(typeof(StringEnumConverter))]//using Newtonsoft.Json;

    public enum DirectionsEnum//Enumeracija koja predstavlja pravac transakcije
    {
        [Description("Debit")]
        [EnumMember(Value = "d")]
        D,
        [Description("Credit")]
        [EnumMember(Value = "c")]
        C
    }
}
