using PFMBackend.Models.Transaction.Enums;
using PFMBackend.Models.Transaction;
using PFMBackend.Commands;

namespace PFMBackend.Services
{
    //interfejs koj sadrzi metode koje ce se primenuti
    public interface ITransactionsService 
    {
        /*ova metoda se koristi za unos liste transakcija u bazu podataka, prima listu transakcija kao parametar
         i vraca asihroni task koji ce vratiti broj unetih transakcija*/
        Task<int> InsertTransactions(List<Transaction> transactions);

        /*Ova metoda se koristi za dohvat liste transakcija koje zadovoljavaju odredjene kriterijume.
         Metoda vraca asihroni task koji ce vratiti paginiranu listu transakcijju sa splitovima*/
        Task<TransactionPagedList<TransactionWithSplits>> GetTransactions(List<TransactionKindsEnum>
            transactionKinds = null, DateTime? startDate = null, DateTime? endDate = null, int page = 1,
        int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.Asc);

        //kategorizacija pojedinačne transakcije identifikovane sa id, koristeći informacije koje se nalaze u transactionCategorizeCommand
        Task<PFMBackend.Problem.Problem> CategorizeTransaction(string id, TransactionCategorizeCommand transactionCategorizeCommand);

        //deljenje transakcije na više kategorija ili podkategorija
        Task<PFMBackend.Problem.Problem> SplitTransaction(string id, SplitTransactionCommand splitTransactionCommand);


        //metoda odgovorna za automatsko kategorizovanje transakcija na osnovu unapred definisanih pravila
        Task<int> AutoCategorizeTransactions();
    }
}
