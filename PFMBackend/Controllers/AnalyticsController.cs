using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PFMBackend.Models.Transaction.Enums;
using PFMBackend.Services;

namespace PFMBackend.Controllers
{
    [ApiController]
    [Route("spending-analytics")]//endpoint
    public class AnalyticsController : ControllerBase
    {
        private readonly ILogger<AnalyticsController> _logger;
        private readonly ICategoriesService _categoriesService;

        //Dependency Injection omogućava da se logger i servis automatski injektuju u
        //kontroler prilikom kreiranja
        public AnalyticsController(ILogger<AnalyticsController> logger, 
            ICategoriesService categoriesService)
        {
            _logger = logger;
            _categoriesService = categoriesService;
        }

        [HttpGet]
        public IActionResult ViewSpendingByCategory([FromQuery] string catcode, 
            [FromQuery(Name = "start-date")] DateTime? startDate, 
            [FromQuery(Name = "end-date")] DateTime? endDate, [FromQuery] DirectionsEnum? direction)
        {
            //dobija analitički izveštaj o potrošnji, po kategoriji
            var spendings = _categoriesService.GetSpendingsByCategory(catcode, startDate, endDate, direction);
            
            //Vraća HTTP 200 OK
            return Ok(JsonConvert.SerializeObject(spendings, Formatting.Indented));
        }
    }
}
