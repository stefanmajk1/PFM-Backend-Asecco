using PFMBackend.Database.Entities;
using PFMBackend.Models.Category;
using PFMBackend.Models.Transaction.Enums;

namespace PFMBackend.Database.Repositories
{
    public interface ICategoriesRepository
    {
        Task<int> Insert(List<CategoryEntity> categoriesToInsert);
        SpendingsByCategory GetSpendings(string catcode, DateTime? startDate, DateTime? endDate, DirectionsEnum? direction);
        CategoryList<CategoryEntity> Get(string? parentId);

        //CategoryList<CategoryEntity> GetAll();
    }
}
