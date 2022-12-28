using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ProductAPI.Cache;
using ProductAPI.Models;
using ProductAPI.Repositories;
using StackExchange.Redis;
using System.Text.Json;

namespace FirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly ILoanRepo _loanRepo;

        public ProductAPIController(ILoanRepo loanRepo)
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
            var loan  = _loanRepo.GetLoansById(id);
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
