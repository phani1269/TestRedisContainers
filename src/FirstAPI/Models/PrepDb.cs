using Microsoft.EntityFrameworkCore;

namespace ProductAPI.Models
{
    public static class PrepDb
    {
         public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using( var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if(isProd)
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }
            
            if(!context.Loans.Any())
            {
                Console.WriteLine("--> Seeding Data...");

                context.Loans.AddRange(
                    new LoanSchemas() { Id = 1, SchemaName = "MUTHOOT ONE PERCENT LOAN", Description = "Online Gold Loan (OGL) facility available,Loan Amount: ₹1,500 to ₹50,000,Tenure: 12 months,Free insurance for pledged gold ornaments,Scheme with the lowest rate of interest (12% p.a.) if 100% interest is paid monthly" },
                    new LoanSchemas() { Id = 2, SchemaName = "MUTHOOT ULTIMATE LOAN (MUL)", Description = "Online Gold Loan (OGL) facility available,Loan starting from ₹1,500 and no maximum limit,Tenure: 12 months,Free insurance for pledged gold ornaments,Rate of interest is 22% p.a. with 2% rebate if 100% interest is paid monthly" },
                    new LoanSchemas() { Id = 3, SchemaName = "MUTHOOT MUDRA LOAN", Description = "Offered at South India branches only,Loan Amount: ₹1,500 to ₹1 lakhs,Highly discounted rate of 11.9% p.a." }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}
