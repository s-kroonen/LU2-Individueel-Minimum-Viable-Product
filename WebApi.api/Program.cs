using WebApi.api.Repositories;
using Avans.Identity.Dapper;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

var sqlConnectionString = builder.Configuration["SqlConnectionString"];

var loggerFactory = LoggerFactory.Create(logging => logging.AddConsole());
var logger = loggerFactory.CreateLogger("Startup");
logger.LogInformation($"Using SQL Connection String: {sqlConnectionString}");

builder.Services.AddAuthorization();
builder.Services
    .AddIdentityApiEndpoints<IdentityUser>()
    .AddDapperStores(options =>
    {
        options.ConnectionString = sqlConnectionString;
    });

builder.Services.AddTransient<WeatherForecastRepository, WeatherForecastRepository>(o => new WeatherForecastRepository(sqlConnectionString));


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
