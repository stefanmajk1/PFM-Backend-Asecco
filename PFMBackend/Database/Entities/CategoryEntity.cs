namespace PFMBackend.Database.Entities
{
    //entitet koj ce se cuvati u bazi
    public class CategoryEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentCode { get; set; }
        public List<TransactionEntity> Transaction { get; set; }
        public List<SplitEntity> Splits { get; set; }
    }
}
