using Newtonsoft.Json;
using PFMBackend.Models.Transaction.Enums;

namespace PFMBackend.Models.Transaction
{

    //genericka klasa koja predstavlja paginiranu listu bankovnih transakcija
    public class TransactionPagedList<T>
    {
        //definisemo promenljive, koje ce vracati vrednosti u Properties
        private int totalCount;
        private int pageSize;
        private int page;
        private int totalPages;
        [JsonProperty("total-count")]
        public int TotalCount { get { return totalCount; } set { totalCount = value < 0 ? 0 : value; } }//ako je manje od 0 postavi 0
        [JsonProperty("page-size")]
        public int PageSize
        {
            get { return pageSize; }
            set
            {
                if (value < 1)//ako je manje od 1 postavi 1
                {
                    pageSize = 1;
                }
                else if (value > 100)//ako je vece od 100 postavi 100
                {
                    pageSize = 100;
                }
                else
                {
                    pageSize = value;
                }
            }
        }
        [JsonProperty("page")]
        public int Page { get { return page; } set { page = value < 1 ? 1 : value; } }//ako je manje od 1 postavi 1
        [JsonProperty("total-pages")]
        public int TotalPages { get { return totalPages; } set { totalPages = value < 0 ? 0 : value; } }//ako je manje od 0 postavi 0
        [JsonProperty("sort-order")]
        public SortOrder SortOrder { get; set; }
        [JsonProperty("sort-by")]
        public string SortBy { get; set; }
        [JsonProperty("items")]
        public List<T> Items { get; set; }
    }
}
