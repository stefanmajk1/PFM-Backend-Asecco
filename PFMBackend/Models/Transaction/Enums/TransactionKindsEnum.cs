using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;

namespace PFMBackend.Models.Transaction.Enums
{
    /*JsonConverter se primenjuje na enumericijama kako bi se odredilo kako ce se
     konvertovati enumeracijske vrednosti prilikom serijalizaije u JSON froman ili
    deserijalizacije iz JSON formata*/

    [JsonConverter(typeof(StringEnumConverter))]//using Newtonsoft.Json
    public enum TransactionKindsEnum // enumeracija koja predstavlja vrstu transakcije
    {
        [Description("Deposit")]
        Dep,

        [Description("Withdrawal")]
        wdw,

        [Description("Payment")]
        Pmt,

        [Description("Fee")]
        Fee,

        [Description("Interest credit")]
        Inc,

        [Description("Reversal")]
        Rev,

        [Description("Adjustment")]
        Adj,

        [Description("Loan disbursement")]
        Lnd,

        [Description("Loan repayment")]
        Lnr,

        [Description("Foreign currency exchange")]
        Fcx,

        [Description("Account opening")]
        Aop,

        [Description("Account closing")]
        Acl,

        [Description("Split payment")]
        Spl,

        [Description("Salary")]
        Sal
    }
}
