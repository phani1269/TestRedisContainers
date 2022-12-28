
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddCacheRegistration(builder.Configuration);
builder.Services.AddStackExchangeRedisCache(options =>
{
options.Configuration = $"{builder.Configuration.GetValue<string>("RedisCache:Host")}:{builder.Configuration.GetValue<int>("RedisCache:Port")}";
});

builder.Services.AddDataProtection();

builder.Services.AddSession(options => {
    options.Cookie.Name = "ChargesAPI_session";
   // options.IdleTimeout = TimeSpan.FromMinutes(5);
});


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
//use Session
app.UseSession();
app.UseAuthorization();

app.MapControllers();

app.Run();
