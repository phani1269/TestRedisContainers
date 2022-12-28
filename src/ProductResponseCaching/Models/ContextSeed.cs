namespace ProductResponseCaching.Models
{
    public class ContextSeed
    {
        public static async Task SeedAsync(AppDbContext dbContext, ILogger<ContextSeed> logger)
        {
            if (!dbContext.Loans.Any())
            {
                dbContext.Loans.AddRange(GetPreconfiguredOrders());
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(AppDbContext).Name);
            }
        }

        private static IEnumerable<LoanSchemas> GetPreconfiguredOrders()
        {
            return new List<LoanSchemas>
            {
                 new LoanSchemas() {  SchemaName = "MUTHOOT ONE PERCENT LOAN", Description = "Online Gold Loan (OGL) facility available,Loan Amount: ₹1,500 to ₹50,000,Tenure: 12 months,Free insurance for pledged gold ornaments,Scheme with the lowest rate of interest (12% p.a.) if 100% interest is paid monthly" },
                    new LoanSchemas() { SchemaName = "MUTHOOT ULTIMATE LOAN (MUL)", Description = "Online Gold Loan (OGL) facility available,Loan starting from ₹1,500 and no maximum limit,Tenure: 12 months,Free insurance for pledged gold ornaments,Rate of interest is 22% p.a. with 2% rebate if 100% interest is paid monthly" },
                    new LoanSchemas() {  SchemaName = "MUTHOOT MUDRA LOAN", Description = "Offered at South India branches only,Loan Amount: ₹1,500 to ₹1 lakhs,Highly discounted rate of 11.9% p.a." }
            };
        }
    }
}
