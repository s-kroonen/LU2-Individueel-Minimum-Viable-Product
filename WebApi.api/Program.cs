using WebApi.api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var sqlConnectionString = builder.Configuration["SqlConnectionString"];

var loggerFactory = LoggerFactory.Create(logging => logging.AddConsole());
var logger = loggerFactory.CreateLogger("Startup");
logger.LogInformation($"Using SQL Connection String: {sqlConnectionString}");

builder.Services.AddSingleton<ILogger<UserRepository>>(sp =>
{
    return loggerFactory.CreateLogger<UserRepository>();
});

builder.Services.AddTransient<WeatherForecastRepository, WeatherForecastRepository>(o => new WeatherForecastRepository(sqlConnectionString));
builder.Services.AddTransient<UserRepository>(sp =>
    new UserRepository(sqlConnectionString, sp.GetRequiredService<ILogger<UserRepository>>()));


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
