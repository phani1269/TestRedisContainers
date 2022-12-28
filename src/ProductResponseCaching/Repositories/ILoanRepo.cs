using ProductResponseCaching.Models;

namespace ProductResponseCaching.Repositories
{
    public interface ILoanRepo
    {
        IEnumerable<LoanSchemas> GetAllLoans();
        LoanSchemas GetLoansById(int id);
        bool CreateLoan(LoanSchemas loan);
    }
}
