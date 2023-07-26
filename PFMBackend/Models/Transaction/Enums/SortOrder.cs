using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace PFMBackend.Models.Transaction.Enums
{
    /*JsonConverter se primenjuje na enumericijama kako bi se odredilo kako ce se
     konvertovati enumeracijske vrednosti prilikom serijalizaije u JSON froman ili
    deserijalizacije iz JSON formata*/
    [JsonConverter(typeof(StringEnumConverter))]//using Newtonsoft.Json

    //Enumeracija koja predstavlja redosled sortiranja
    public enum SortOrder
    {
        [EnumMember(Value = "asc")]
        Asc,
        [EnumMember(Value = "desc")]
        Desc
    }
}
