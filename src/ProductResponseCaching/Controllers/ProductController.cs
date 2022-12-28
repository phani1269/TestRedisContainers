using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ProductResponseCaching.Cache;
using ProductResponseCaching.Models;
using ProductResponseCaching.Repositories;

namespace ProductResponseCaching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILoanRepo _loanRepo;

        public ProductController(ILoanRepo loanRepo)
        {
            _loanRepo = loanRepo;
        }

        [HttpGet]
        [Cached(600)]
        public ActionResult GetAllLoans()
        {
            var loanSchemas = _loanRepo.GetAllLoans();
            return Ok(loanSchemas);
        }
        [HttpGet("{id}")]
        [Cached(600)]
        public ActionResult GetLoanById(int id)
        {
            var loan = _loanRepo.GetLoansById(id);
            return Ok(loan);
        }
        [HttpPost]
        public ActionResult CreateLoan(LoanSchemas loan)
        {
            var result = _loanRepo.CreateLoan(loan);
            return Ok(result);
        }
    }
}
