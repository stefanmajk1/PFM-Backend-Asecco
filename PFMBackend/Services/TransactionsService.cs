using AutoMapper;
using PFMBackend.Commands;
using PFMBackend.Database.Entities;
using PFMBackend.Database.Repositories;
using PFMBackend.Models.Categorization;
using PFMBackend.Models.Transaction;
using PFMBackend.Models.Transaction.Enums;
using System.Text.Json;
using System.IO;

namespace PFMBackend.Services
{
    //servis koji sluzi za upravljanje transakcijama
    public class TransactionsService : ITransactionsService//nasledjujemo interfejs, i primenjujemo njegove metode
    {
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IMapper _mapper;

        public TransactionsService(ITransactionsRepository transactionsRepository, IMapper mapper)
        {
            _transactionsRepository = transactionsRepository;//predstavlja repozitorijum, za rad sa transakcijama
            _mapper = mapper;//za mapiranje modela i entiteta
        }

        /*ova metoda vraca listu transakcija koje zadovoljavaju odredjene kriterijume*/
        public async Task<TransactionPagedList<TransactionWithSplits>> GetTransactions(List<TransactionKindsEnum> transactionKinds = null, DateTime? startDate = null, DateTime? endDate = null, int page = 1,
        int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.Asc)
        {
            //poziva metodu 'Get' kako bi dobio listu transakcija koje zadovoljavaju kriterijume
            var pagedList = await _transactionsRepository.Get(transactionKinds, startDate, endDate, page, pageSize, sortBy, sortOrder);

            //mapiramo rezultat pomocu AutoMappera
            return _mapper.Map<TransactionPagedList<TransactionWithSplits>>(pagedList);
        }

        //metoda suzi za unos transakcija u bazu podataka
        public async Task<int> InsertTransactions(List<Transaction> transactions)
        {
            //mapiramo sa modela na entitet
            List<TransactionEntity> transToInsert = _mapper.Map<List<Transaction>, List<TransactionEntity>>(transactions);

            //unosimo transakcije u bazu
            return await _transactionsRepository.Insert(transToInsert);
        }

        //metoda odgovorna za automatsko kategorizovanje transakcija na osnovu unapred definisanih pravila
        public async Task<int> AutoCategorizeTransactions()
        {
            string path = "C:\\Users\\HP\\source\\repos\\PFMBackend\\PFMBackend\\JSON\\autocategorization.json";
            string jsonString = System.IO.File.ReadAllText(path);

            //'RulesList'je model koji predstavlja listu pravila automatskog kategorizovanja koje su unapred definisane
            RulesList rulesList = JsonSerializer.Deserialize<RulesList>(jsonString);//deserijalizuje JSON u objekat

            await _transactionsRepository.AutoCategorize(rulesList);

            return 0;
        }

        //kategorizacija pojedinačne transakcije identifikovane sa id, koristeći informacije koje se nalaze u transactionCategorizeCommand
        public async Task<PFMBackend.Problem.Problem> CategorizeTransaction(string id, TransactionCategorizeCommand transactionCategorizeCommand)
        {
            return await _transactionsRepository.Categorize(id, transactionCategorizeCommand); //vraca vrednosti po id
            //await ključnu reč, to znači da će sačekati da se završi izvršavanje asinhronog zadatka pre nego što se nastavi sa izvršavanjem ostatka metode
        }


        //deljenje transakcije na više kategorija ili podkategorija
        public async Task<PFMBackend.Problem.Problem> SplitTransaction(string id, SplitTransactionCommand splitTransactionCommand)
        {
            return await _transactionsRepository.Split(id, splitTransactionCommand);//vraca vrednosti po id
            //await ključnu reč, to znači da će sačekati da se završi izvršavanje asinhronog zadatka pre nego što se nastavi sa izvršavanjem ostatka metode
        }
    }
}
