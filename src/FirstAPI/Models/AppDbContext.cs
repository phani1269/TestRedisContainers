using Microsoft.EntityFrameworkCore;

namespace ProductAPI.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }
        public DbSet<LoanSchemas> Loans { get; set; }
    }
}
