using System;
using System.Globalization;
using PFMBackend.Mapping.Entities;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace PFMBackend.Mapping
{
    //ovo je klasa za mapiranje podataka iz CSV fajla 'TransactionCsvEntity' u objekte
    public class CsvTransactionMapping : CsvMapping<TransactionCsvEntity>
    /*TinyCsvParser biblioteka i definise kako ce se vrednosti iz CSV fajla
      mapirati na svojstva TransactionCsvEntity klase*/
    {
        public CsvTransactionMapping() : base()
        {   //prva kolona u CSV fajlu odgovara svojstvu 'id' u 'TransactionCsvEntity' klase itd.
            MapProperty(0, x => x.Id);
            MapProperty(1, x => x.BeneficiaryName);
            MapProperty(2, x => x.Date);
            MapProperty(3, x => x.Direction);
            MapProperty(4, x => x.Amount);
            MapProperty(5, x => x.Description);
            MapProperty(6, x => x.Currency);
            MapProperty(7, x => x.Mcc);
            MapProperty(8, x => x.Kind);
        }
    }
}
