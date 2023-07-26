using AutoMapper;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TinyCsvParser.Mapping;
using TinyCsvParser;
using PFMBackend.Services;
using PFMBackend.Problem;
using PFMBackend.Mapping;
using PFMBackend.Mapping.Entities;
using PFMBackend.File;
using PFMBackend.Models.Category;
using PFMBackend.Validation;

namespace PFMBackend.Controllers
{
    [ApiController]
    [Route("categories")]//endpoint
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;//beleženje događaja u kontroleru
        private readonly IMapper _mapper;//mapiranje modela
        private readonly ICategoriesService _categoriesService;//rad sa kategorijama


        //Dependency Injection omogućava da se logger i servis automatski injektuju u
        //kontroler prilikom kreiranja
        public CategoriesController(ILogger<CategoriesController> logger,
            IMapper mapper, ICategoriesService categoriesService)
        {
            _logger = logger;
            _mapper = mapper;
            _categoriesService = categoriesService;
        }

        [HttpPost("import")]//endpoint
        public async Task<IActionResult> ImportCategories([FromForm(Name = "csv-file")] IFormFile csvFile)
        {
            if (csvFile == null)//ako je fajl prazan, vratice error
            {
                //HTTP 400 Bad Request i JSON odgovor koji sadrži detalje greške
                return BadRequest(JsonConvert.SerializeObject(new ValidationProblem
                {
                    Errors = new List<Errors>{
                        new Errors{
                            Tag = "csv-file",
                            Error = ErrEnum.Required,
                            Message = Validation.Validate.GetEnumDescription(ErrEnum.Required)
                        }
                    }
                }, Formatting.Indented));
            }
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
            CsvCategoryMapping csvCategoryMapping = new CsvCategoryMapping();
            CsvParser<CategoryCsv> csvParser = 
                new CsvParser<CategoryCsv>(csvParserOptions, csvCategoryMapping);

            string filePath = await MethodFiles.GetFilePath(csvFile);

            //citanje iz fajla
            var categoryList = csvParser.ReadFromFile(filePath, Encoding.ASCII).ToList();

            //validira se lista iz fajla
            var validationProblem = 
                Validation.Validate.ValidateList<CsvMappingResult<CategoryCsv>>(categoryList);

            //HTTP 400 Bad Request
            if (validationProblem.Errors.Count > 0)
            {
                return BadRequest(JsonConvert.SerializeObject(validationProblem, Formatting.Indented));
            }
            //mapiranje iz 'categoryList' i listu 'Category'
            var categories = _mapper.Map<List<CsvMappingResult<CategoryCsv>>,
                                        List<Category>>(categoryList);

            //kategorije unosimo u bazu
            await _categoriesService.InsertCategories(categories);

            //briše se privremeni CSV fajl
            System.IO.File.Delete(filePath);


            //vraca poruku HTTP 200 OK status, sa porukom 'Categories imported'
            return Ok("Categories imported");
        }

        [HttpGet]
        public IActionResult GetCategories([FromQuery(Name = "parent-id")] string? parentId)
        {

            //dobio spisak kategorija na osnovu prosleđenog 'parentId'
            var listToReturn = _categoriesService.GetCategories(parentId);


            // HTTP 200 OK status sa listom kategorija
            return Ok(JsonConvert.SerializeObject(listToReturn, Formatting.Indented));
        }

        //[HttpGet("all")] // Dodajte novu rutu /categories/all
        //public IActionResult GetAllCategories()
        //{
        //    var listToReturn = _categoriesService.GetAllCategories();
        //    return Ok(JsonConvert.SerializeObject(listToReturn, Formatting.Indented));
        //}
    }
}
