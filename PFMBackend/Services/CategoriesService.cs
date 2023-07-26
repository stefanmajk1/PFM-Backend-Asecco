using AutoMapper;
using PFMBackend.Database.Entities;
using PFMBackend.Database.Repositories;
using PFMBackend.Models.Category;
using PFMBackend.Models.Transaction.Enums;

namespace PFMBackend.Services
{
    //implementacija metoda iz interfejsa
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IMapper _mapper;

        public CategoriesService(ICategoriesRepository categoriesRepository, IMapper mapper)
        {
            _categoriesRepository = categoriesRepository;//predstavlja repozitorijum, za rad sa transakcijama
            _mapper = mapper;//za mapiranje modela i entiteta
        }

        //public CategoryList<Category> GetAllCategories()
        //{
        //    return _mapper.Map<CategoryList<Category>>(_categoriesRepository.GetAll());
        //}

        //vraća listu kategorija troškova
        public CategoryList<Category> GetCategories(string? parentId)
        {
            return _mapper.Map<CategoryList<Category>>(_categoriesRepository.Get(parentId));//mapiranje entiteta kategorija
        }

        //vraća analitičke podatke o troškovima po određenoj kategoriji
        public SpendingsByCategory GetSpendingsByCategory(string catcode, DateTime? startDate, DateTime? endDate, DirectionsEnum? direction)
        {
            //dobijamo analitičke podatke o troškovima za određenu kategoriju, koji se zatim vraćaju kao rezultat metode
            return _categoriesRepository.GetSpendings(catcode, startDate, endDate, direction);
        }

        //unos kategorija troškova u bazu podataka
        public async Task<int> InsertCategories(List<Category> categories)
        {
            // mapiranje kategorije iz modela u entitet
            var categoriesToInsert = _mapper.Map<List<Category>, List<CategoryEntity>>(categories); 

            await _categoriesRepository.Insert(categoriesToInsert);//ubacivanje u repozitorijum

            return 0;
        }
    }
}
