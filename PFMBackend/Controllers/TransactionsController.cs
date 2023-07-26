using AutoMapper;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PFMBackend.Commands;
using PFMBackend.Mapping.Entities;
using PFMBackend.Mapping;
using PFMBackend.Models.Transaction.Enums;
using PFMBackend.Services;
using System.Text;
using TinyCsvParser.Mapping;
using TinyCsvParser;
using PFMBackend.Problem;
using PFMBackend.File;
using PFMBackend.Models.Transaction;

namespace PFMBackend.Controllers
{
    [ApiController]//ovo je api kontroler
    public class TransactionsController : ControllerBase
    {
        private readonly ILogger<TransactionsController> _logger;//belezi dogadjaj u kontroleru
        private readonly IMapper _mapper;//mapiranje modela
        private readonly ITransactionsService _transactionsService;//rad sa transakcijama


        //DI
        public TransactionsController(ILogger<TransactionsController> logger, IMapper mapper,
            ITransactionsService transactionsService)
        {
            _logger = logger;
            _mapper = mapper;
            _transactionsService = transactionsService;
        }

        [HttpPost("transactions/import")]//endpoint
        public async Task<IActionResult> ImportTransactions([FromForm(Name = "csv-file")] IFormFile csvFile)
        {
            if (csvFile == null)//ako je fajl prazan vratice error
            {
                //HTTP 400 Bad Request
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
            CsvTransactionMapping csvTransactionMapping = new CsvTransactionMapping();
            CsvParser<TransactionCsvEntity> csvParser = new CsvParser<TransactionCsvEntity>(csvParserOptions, csvTransactionMapping);

            //citanje fajla
            string filePath = await MethodFiles.GetFilePath(csvFile);

            //pravljenje liste transakcija
            var transactionList = csvParser.ReadFromFile(filePath, Encoding.ASCII).ToList();

            //validacija lista transakcija
            var validationProblem = Validation.Validate.ValidateList<CsvMappingResult<TransactionCsvEntity>>(transactionList);

            //HTTP 400 Bad Request
            if (validationProblem.Errors.Count > 0)
            {
                return BadRequest(JsonConvert.SerializeObject(validationProblem, Formatting.Indented));
            }

            //mapiranje iz liste 'transactionList' u listu 'Transaction'
            var transactions = _mapper.Map<List<CsvMappingResult<TransactionCsvEntity>>, List<Transaction>>(transactionList);

            //briše se privremeni CSV fajl
            System.IO.File.Delete(filePath);

            //unosenje transakcija u bazu podataka
            await _transactionsService.InsertTransactions(transactions);

            //Vraća HTTP 200 OK
            return Ok("Transaction splitted");
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions([FromQuery(Name = "transaction-kinds")] List<TransactionKindsEnum> transactionKinds, 
            [FromQuery(Name = "start-date")] DateTime? startDate, [FromQuery(Name = "end-date")] DateTime? endDate, [FromQuery] int? page,
        [FromQuery(Name = "page-size")] int? pageSize, [FromQuery(Name = "sort-by")] string sortBy, 
        [FromQuery(Name = "sort-order")] SortOrder sortOrder)
        {
            page ??= 1;
            pageSize ??= 10;

            //uzima listu transakcija
            var listToReturn = await _transactionsService.GetTransactions(transactionKinds, startDate, endDate, page.Value, pageSize.Value,
                sortBy, sortOrder);

            //Vraća HTTP 200 OK
            return Ok(JsonConvert.SerializeObject(listToReturn, Formatting.Indented, new JsonSerializerSettings
            { DateFormatString = "MM/dd/yyyy" }));//format godine
        }

        [HttpPost("transaction/{id}/categorize")]
        public async Task<IActionResult> CategorizeTransaction([FromRoute] string id, [FromBody] TransactionCategorizeCommand transactionCategorizeCommand)
        {
            List<Errors> errors = new List<Errors>();//prazna lsita errora
            if (string.IsNullOrEmpty(id))//ako je id prazan, punimo listu error
            {
                errors.Add(new Errors { Tag = "id", Error = ErrEnum.Required, Message = Validation.Validate.GetEnumDescription(ErrEnum.Required) });
            }
            if (transactionCategorizeCommand == null)
            {
                errors.Add(new Errors { Tag = "transaction-categorize-command", Error = ErrEnum.Required, Message = Validation.Validate.GetEnumDescription(ErrEnum.Required) });
            }
            //HTTP 400
            if (errors.Count > 0)
            {
                return BadRequest(JsonConvert.SerializeObject(errors, Formatting.Indented));
            }

            var problem = await _transactionsService.CategorizeTransaction(id, transactionCategorizeCommand);

            if (problem != null)
            {
                if (problem is BusinessProblem)//440 error
                {
                    Response.StatusCode = 440;
                    return new ObjectResult(JsonConvert.SerializeObject(problem, Formatting.Indented)) { StatusCode = 440 };
                }
            }

            return Ok("Transaction splitted");
        }

        [HttpPost("transaction/{id}/split")]
        public async Task<IActionResult> SplitTransaction([FromRoute] string id, [FromBody] SplitTransactionCommand splitTransactionCommand)
        {
            var problem = await _transactionsService.SplitTransaction(id, splitTransactionCommand);

            if (problem != null)
            {
                //Http 400
                if (problem is BusinessProblem)
                {
                    return new ObjectResult(JsonConvert.SerializeObject(problem, Formatting.Indented)) { StatusCode = 440 };
                }
            }

            return Ok("Transaction splitted");
        }


        [HttpPost("transaction/auto-categorize")]
        public async Task<IActionResult> AutoCategorizeTransactions()
        {
            await _transactionsService.AutoCategorizeTransactions();

            return Ok("Transaction auto categorized");
        }
    }
}