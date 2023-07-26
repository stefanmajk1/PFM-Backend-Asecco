using PFMBackend.Mapping.Entities;
using TinyCsvParser.Mapping;

namespace PFMBackend.Mapping
{//ovo je klasa za mapiranje podataka iz CSV fajla 'CategoryCsv' u objekte
    public class CsvCategoryMapping : CsvMapping<CategoryCsv>
    /*TinyCsvParser biblioteka i definise kako ce se vrednosti iz CSV fajla
  mapirati na svojstva TransactionCsvEntity klase*/
    {
        public CsvCategoryMapping() : base()
        {   //prva kolona u CSV fajlu odgovara svojstvu 'Code' u 'CategoryCsv' klase itd.
            MapProperty(0, c => c.Code);
            MapProperty(1, c => c.ParentCode);
            MapProperty(2, c => c.Name);
        }
    }
}
