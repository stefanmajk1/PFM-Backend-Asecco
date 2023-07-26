using PFMBackend.Commands;
using PFMBackend.Database.Entities;
using PFMBackend.Models.Categorization;
using PFMBackend.Models.Transaction;
using PFMBackend.Models.Transaction.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace PFMBackend.Database.Repositories
{
    //interfejs koj definise metode, koje ce se primenjivati
    public interface ITransactionsRepository
    {
        //umetanje liste transakcije u bazu podataka
        Task<int> Insert(List<TransactionEntity> transToInsert);

        //metoda se koristi za dohvatanje liste transakcija iz baze podataka
        Task<TransactionPagedList<TransactionEntity>> Get(List<TransactionKindsEnum> transactionKinds = null,
            DateTime? startDate = null, DateTime? endDate = null, int page = 1,
        int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.Asc);
        Task<PFMBackend.Problem.Problem> Categorize(string id, TransactionCategorizeCommand transactionCategorizeCommand);
        Task<PFMBackend.Problem.Problem> Split(string id, SplitTransactionCommand splitTransactionCommand);
        Task<int> AutoCategorize(RulesList rulesList);
    }
}
