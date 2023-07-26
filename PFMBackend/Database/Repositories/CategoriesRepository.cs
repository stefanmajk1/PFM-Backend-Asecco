using PFMBackend.Database.Entities;
using PFMBackend.Models.Transaction.Enums;
using PFMBackend.Database;
using PFMBackend.Models.Category;

namespace PFMBackend.Database.Repositories
{
    //klasa za implementaciju metoda iz interfejsa
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly TransactionsDbContext _dbContext;

        public CategoriesRepository(TransactionsDbContext dbContext)
        {
            _dbContext = dbContext;//pristup bazi
        }

        //public CategoryList<CategoryEntity> GetAll()
        //{
        //    var categoriesQuery = _dbContext.Categories.AsQueryable();
        //    return new CategoryList<CategoryEntity>
        //    {
        //        Items = categoriesQuery.ToList()
        //    };
        //}

        //uzimanje Category po Id
        public CategoryList<CategoryEntity> Get(string? parentId)
        {
            //uzimanje kategorije
            var categoriesQuery = _dbContext.Categories.AsQueryable();

            //trazenje Category po Id
            if (!string.IsNullOrEmpty(parentId))
            {
                categoriesQuery = categoriesQuery.Where(c => c.ParentCode == parentId);
            }
            

            //vracanje
            return new CategoryList<CategoryEntity>
            {
                Items = categoriesQuery.ToList()
            };
        }
        //vraća analitičke podatke o troškovima po određenoj kategoriji
        public SpendingsByCategory GetSpendings(string catcode, DateTime? startDate, DateTime? endDate, DirectionsEnum? direction)
        {
            startDate ??= new DateTime(2010, 1, 1);
            endDate ??= DateTime.Today.AddYears(5);

            startDate = startDate.Value.ToUniversalTime();
            endDate = endDate.Value.ToUniversalTime();

            DateTime startDateVal = startDate.HasValue ? startDate.Value : DateTime.MinValue;
            DateTime endDateVal = endDate.HasValue ? endDate.Value : DateTime.MaxValue;



            //upit nad bazom podataka kako bi se izvukle transakcije u određenom vremenskom periodu
            var transactionsQuery = _dbContext.Transactions.AsQueryable().Where(
                t => t.Date > startDate.Value && t.Date < endDate.Value && t.Catcode != null);

            //upit nad bazom podataka kako bi se izvukle sve kategorije
            // Primenjujemo opcioni filter za kategorije
            if (!string.IsNullOrEmpty(catcode))
            {
                transactionsQuery = transactionsQuery.Where(t => t.Catcode == catcode);
            }

            // Primenjujemo opcioni filter za smer transakcija (Debit ili Credit)
            if (direction != null)
            {
                transactionsQuery = transactionsQuery.Where(t => t.Direction == direction);
            }

            // Grupisanje transakcija po kategorijama
            var groupedTransactions = transactionsQuery
                .GroupBy(t => t.Catcode)
                .Select(group => new SpendingInCategory
                {
                    Catcode = group.Key,
                    Amount = Math.Round(group.Sum(t => t.Amount), 2),
                    Count = group.Count()
                })
                .ToList();

            // Vraćamo analitičke podatke o troškovima po kategorijama
            return new SpendingsByCategory
            {
                Groups = groupedTransactions
            };
        }
        // vrši unos kategorija troškova u bazu podataka
        public async Task<int> Insert(List<CategoryEntity> categoriesToInsert)
        {
            var categories = categoriesToInsert.GroupBy(c => c.Code).Select(g => g.Last()).ToList();

            _dbContext.Categories.UpdateRange(categories.Where(c => _dbContext.Categories.Contains(c)));
            await _dbContext.Categories.AddRangeAsync(categories.Where(c => !_dbContext.Categories.Contains(c)));
            await _dbContext.SaveChangesAsync();

            return 0;
        }
    }
}
