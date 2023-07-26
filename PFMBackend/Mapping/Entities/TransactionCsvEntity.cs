namespace PFMBackend.Mapping.Entities
{
    //entitet koji ce sadrzati podatke iz CSV fajla za transakciju
    public class TransactionCsvEntity 
    {
        public string Id { get; set; }

        public string BeneficiaryName { get; set; }

        public string Date { get; set; }

        public string Direction { get; set; }

        public string Amount { get; set; }

        public string Description { get; set; }

        public string Currency { get; set; }

        public string Mcc { get; set; }

        public string Kind { get; set; }
        public string CatCode { get; set; }
    }
}
