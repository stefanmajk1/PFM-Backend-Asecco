using PFMBackend.Models.Category;
using PFMBackend.Models.Transaction.Enums;

namespace PFMBackend.Services
{
    public interface ICategoriesService
    {
        //unos kategorija troškova u bazu podataka
        Task<int> InsertCategories(List<Category> categories);

        //vraća analitičke podatke o troškovima po određenoj kategoriji
        SpendingsByCategory GetSpendingsByCategory(string catcode, DateTime? startDate, DateTime? endDate, DirectionsEnum? direction);

        //vraća listu kategorija troškova
        CategoryList<Category> GetCategories(string? parentId);
        //CategoryList<Category> GetAllCategories();

    }
}
