using Microsoft.EntityFrameworkCore;
using ProductResponseCaching.Cache;
using ProductResponseCaching.Extensions;
using ProductResponseCaching.Models;
using ProductResponseCaching.Repositories;

var builder = WebApplication.CreateBuilder(args);

// In Memory storage dependencies
builder.Services.AddMemoryCache();


builder.Services.AddScoped<ILoanRepo, LoanRepo>();




builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnectionString"),
                    providerOptions => providerOptions.EnableRetryOnFailure()));
builder.Services.AddScoped<IResponseCacheService, ResponseCacheService>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MigrateDatabase<AppDbContext>((context, services) =>
{
    var logger = services.GetService<ILogger<ContextSeed>>();
    ContextSeed.SeedAsync(context, logger).Wait();
}).Run();

app.Run();
