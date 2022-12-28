using Microsoft.EntityFrameworkCore;
using ProductAPI.Cache;
using ProductAPI.Extensions;
using ProductAPI.Models;
using ProductAPI.Repositories;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ILoanRepo, LoanRepo>();

// redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = $"{builder.Configuration.GetValue<string>("RedisCache:Host")}:{builder.Configuration.GetValue<int>("RedisCache:Port")}";
});
builder.Services.AddScoped<IResponseCacheService, ResponseCacheService>();



builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnectionString"),
                    providerOptions => providerOptions.EnableRetryOnFailure()));

var deltaBackOffms = Convert.ToInt32(TimeSpan.FromSeconds(5).TotalMilliseconds);
var maxdeltaBackOffms = Convert.ToInt32(TimeSpan.FromSeconds(20).TotalMilliseconds);

var options = new ConfigurationOptions
{

    EndPoints = { $"{builder.Configuration.GetValue<string>("RedisCache:Host")}:{builder.Configuration.GetValue<int>("RedisCache:Port")}" },
    ConnectRetry = 5,
    ReconnectRetryPolicy = new ExponentialRetry(deltaBackOffms,
                                    maxdeltaBackOffms),
    ConnectTimeout = 1000,
    AbortOnConnectFail = false,
    SyncTimeout = 10000

};



var redisMultiplexer = ConnectionMultiplexer.Connect(options);
builder.Services.AddSingleton<IConnectionMultiplexer>(redisMultiplexer);



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product.API v1"));
}

app.UseAuthorization();


app.MapControllers();


app.MigrateDatabase<AppDbContext>((context, services) =>
{
    var logger = services.GetService<ILogger<ContextSeed>>();
    ContextSeed.SeedAsync(context, logger).Wait();
}).Run();


app.Run();

