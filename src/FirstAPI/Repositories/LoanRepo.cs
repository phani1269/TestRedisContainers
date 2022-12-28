using ProductAPI.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace ProductAPI.Repositories
{
    public class LoanRepo : ILoanRepo
    {
        private readonly AppDbContext _dbContext;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public LoanRepo(AppDbContext dbContext, IConnectionMultiplexer connectionMultiplexer)
        {
            _dbContext = dbContext;
            _connectionMultiplexer = connectionMultiplexer;
        }

        public bool CreateLoan(LoanSchemas loan)
        {
            if (loan!=null)
            {
               var result =  _dbContext.Loans.Add(loan);
                _dbContext.SaveChanges();

                var db = _connectionMultiplexer.GetDatabase(0);
                var cachedData = db.HashGet("/api/ProductAPI", "data");

                var cacheList = JsonSerializer.Deserialize<List<LoanSchemas>>(cachedData);
                cacheList.Add(result.Entity);
                var serial = JsonSerializer.Serialize(cacheList);
                db.HashSet("/api/ProductAPI", new HashEntry[] { new HashEntry("data", serial) });
                return true;
            }
            return false;
        }

        public IEnumerable<LoanSchemas> GetAllLoans()
        {
            var loansList = _dbContext.Loans.ToList();
            return loansList;
        }

        public LoanSchemas GetLoansById(int id)
        {
            var loan = _dbContext.Loans.Where(x => x.Id == id).SingleOrDefault();
            return loan;
        }
    }
}
