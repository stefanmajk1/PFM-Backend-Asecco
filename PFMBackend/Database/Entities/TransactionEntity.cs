using PFMBackend.Models.Transaction.Enums;

namespace PFMBackend.Database.Entities
{
    //model entiteta za transakciju koj ce se koristiti za bazu podataka
    public class TransactionEntity
    {
        public string Id { get; set; }

        public string BeneficiaryName { get; set; }

        public DateTime Date { get; set; }

        public DirectionsEnum Direction { get; set; }

        public double Amount { get; set; }

        public string Description { get; set; }

        public string Currency { get; set; }

        public MccCodeEnum? Mcc { get; set; }

        public TransactionKindsEnum Kind { get; set; }

        public CategoryEntity Category { get; set; }
        public string Catcode { get; set; }
        public List<SplitEntity> Splits { get; set; }
    }
}
