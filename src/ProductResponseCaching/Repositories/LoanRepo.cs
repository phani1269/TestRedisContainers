using ProductResponseCaching.Models;

namespace ProductResponseCaching.Repositories
{
    public class LoanRepo : ILoanRepo
    {
        private readonly AppDbContext _dbContext;

        public LoanRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CreateLoan(LoanSchemas loan)
        {
            if (loan!=null)
            {
                _dbContext.Loans.Add(loan);
                _dbContext.SaveChanges();
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
