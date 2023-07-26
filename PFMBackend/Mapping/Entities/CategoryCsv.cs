namespace PFMBackend.Mapping.Entities
{
    //entitet koji ce sadrzati podatke iz CSV fajla za transakciju
    public class CategoryCsv
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentCode { get; set; }
    }
}
