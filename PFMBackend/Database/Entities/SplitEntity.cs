namespace PFMBackend.Database.Entities
{
    //entitet koj ce se cuvati u bazi
    public class SplitEntity
    {
        public string TransactionId { get; set; }
        public string Catcode { get; set; }
        public double Amount { get; set; }
        public TransactionEntity Transaction { get; set; }
        public CategoryEntity Category { get; set; }
    }
}
