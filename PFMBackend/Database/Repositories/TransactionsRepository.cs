using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PFMBackend.Commands;
using PFMBackend.Database.Entities;
using PFMBackend.Models.Categorization;
using PFMBackend.Models.Transaction;
using PFMBackend.Models.Transaction.Enums;
using PFMBackend.Problem;
using System.Data;

namespace PFMBackend.Database.Repositories
{
    //implementacija interfejsa i njenih metoda
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly TransactionsDbContext _dbContext;
        private readonly IMapper _mapper;

        public TransactionsRepository(TransactionsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext; //pristup bazi
            _mapper = mapper;//mapiranje entiteta i modela
        }

        public async Task<TransactionPagedList<TransactionEntity>> Get(List<TransactionKindsEnum> transactionKinds = null, 
            DateTime? startDate = null, DateTime? endDate = null, int page = 1,
        int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.Asc)
        {
            startDate ??= new DateTime(2010, 1, 1);
            endDate ??= DateTime.Today.AddYears(5);

            startDate = startDate.Value.ToUniversalTime();
            endDate = endDate.Value.ToUniversalTime();

            DateTime startDateVal = startDate.Value;
            DateTime endDateVal = endDate.Value;

            var query = _dbContext.Transactions.AsQueryable().SetSplits(_dbContext.Splits.ToList());


            if (transactionKinds != null && transactionKinds.Count > 0)
            {
                query = query.Where(t => transactionKinds.Contains(t.Kind) && 
                t.Date >= startDateVal && t.Date <= endDateVal);
            }

            query = query.Where(t => t.Date >= startDateVal && t.Date <= endDateVal);


            var totalCount = await query.CountAsync();
            var totalPages = Math.Ceiling((double)totalCount / pageSize);
            page = page > totalPages ? (int)totalPages : page;

            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortOrder == SortOrder.Desc)
                {
                    query = query.OrderByDescending(sortBy, t => t.Id);
                }
                else
                {
                    query = query.OrderBy(sortBy, t => t.Id);
                }
            }

            query = query.Skip((page - 1 < 0 ? 0 : page - 1) * pageSize).Take(pageSize);
            var pagedSortedTransactionsWithSplitsList = await query.ToListAsync();

            return new TransactionPagedList<TransactionEntity>
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                Page = page,
                TotalPages = (int)totalPages,
                SortOrder = sortOrder,
                SortBy = sortBy,
                Items = pagedSortedTransactionsWithSplitsList
            };
        }

        //ubacujemo u bazu
        public async Task<int> Insert(List<TransactionEntity> transToInsert)
        {
            await _dbContext.Transactions.AddRangeAsync(transToInsert);

            await _dbContext.SaveChangesAsync();

            return 0;
        }

        public async Task<int> AutoCategorize(RulesList rulesList)
        {

            foreach (var rule in rulesList.Rules)
            {
                List<TransactionEntity> transactionsToUpdate = new List<TransactionEntity>();
                transactionsToUpdate = _dbContext.Transactions
                    .FromSqlInterpolated($"SELECT * FROM transactions WHERE {rule.Predicate}")
                    .ToList();

                if (transactionsToUpdate.Count > 0)
                {
                    transactionsToUpdate.ForEach(t => t.Catcode = rule.Catcode);
                    _dbContext.UpdateRange(transactionsToUpdate);
                }
            }

            await _dbContext.SaveChangesAsync();

            return 0;
        }

        private bool EvaluatePredicate(string predicate, TransactionEntity transaction)
        {
            var dt = new DataTable();
            dt.Columns.Add("Catcode", typeof(int)); // Add necessary columns from your TransactionEntity class
            dt.Columns.Add("BeneficiaryName", typeof(string));

            // Add rows to the DataTable with data from the transaction
            dt.Rows.Add(transaction.Catcode, transaction.BeneficiaryName); // Adjust column names according to your TransactionEntity class

            // Create a DataView to apply the filter
            var dv = new DataView(dt);
            dv.RowFilter = predicate;

            // If any rows match the filter, return true; otherwise, return false
            return dv.Count > 0;
        }


        public async Task<PFMBackend.Problem.Problem> Categorize(string id, TransactionCategorizeCommand transactionCategorizeCommand)
        {
            if (!await _dbContext.Categories.AnyAsync(c => c.Code == transactionCategorizeCommand.Catcode))
            {
                return new BusinessProblem
                {
                    ProblemLiteral = "provided-category-does-not-exist",
                    ProblemMessage = "Provided category does not exist",
                    ProblemDetails = $"Category with code {transactionCategorizeCommand.Catcode} does not exist in database"
                };
            }
            if (!await _dbContext.Transactions.AnyAsync(t => t.Id == id))
            {
                return new BusinessProblem
                {
                    ProblemLiteral = "provided-transaction-does-not-exist",
                    ProblemMessage = "Provided transaction does not exist",
                    ProblemDetails = $"Transaction with id {id} does not exist in database"
                };
            }

            var categorizedTransaction = await _dbContext.Transactions.FirstAsync(t => t.Id == id);
            categorizedTransaction.Catcode = transactionCategorizeCommand.Catcode;

            _dbContext.Update(categorizedTransaction);
            await _dbContext.SaveChangesAsync();

            return null;
        }

        

        public async Task<PFMBackend.Problem.Problem> Split(string id, SplitTransactionCommand splitTransactionCommand)
        {
            var notExisting = splitTransactionCommand.Splits.Where(s => !_dbContext.Categories.Any(c => c.Code == s.Catcode));
            if (notExisting.Count() > 0)
            {
                return new BusinessProblem
                {
                    ProblemLiteral = "provided-category-does-not-exist",
                    ProblemMessage = "Provided category does not exist",
                    ProblemDetails = $"Category with code {notExisting.First().Catcode} does not exist in database"
                };
            }
            if (!await _dbContext.Transactions.AnyAsync(t => t.Id == id))
            {
                return new BusinessProblem
                {
                    ProblemLiteral = "provided-transaction-does-not-exist",
                    ProblemMessage = "Provided transaction does not exist",
                    ProblemDetails = $"Transaction with id {id} does not exist in database"
                };
            }

            var splitsQuery = splitTransactionCommand.Splits.AsQueryable();
            if (splitsQuery.Sum(s => s.Amount) > _dbContext.Transactions.Where(t => t.Id == id).Sum(t => t.Amount))
            {
                return new BusinessProblem
                {
                    ProblemLiteral = "split-amount-over-transaction-amount",
                    ProblemMessage = "Split amount is larger then transaction amount",
                    ProblemDetails = ""
                };
            }
            if (splitsQuery.Sum(s => s.Amount) < _dbContext.Transactions.Where(t => t.Id == id).Sum(t => t.Amount))
            {
                return new BusinessProblem
                {
                    ProblemLiteral = "split-amount-over-transaction-amount",
                    ProblemMessage = "Split amount is smaller then transaction amount",
                    ProblemDetails = ""
                };
            }

            var splitsToRemove = _dbContext.Splits.AsQueryable().Where(s => s.TransactionId == id);
            if (await splitsToRemove.CountAsync() > 0)
            {
                _dbContext.Splits.RemoveRange(splitsToRemove);
            }

            List<SplitEntity> splits = new List<SplitEntity>(_mapper.Map<List<SplitEntity>>(splitsQuery.ToList()));
            splits.ForEach(s => s.TransactionId = id);

            await _dbContext.Splits.AddRangeAsync(splits);
            await _dbContext.SaveChangesAsync();

            return null;
        }
    }
}
