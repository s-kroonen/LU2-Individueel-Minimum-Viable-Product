using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using WebApi.api.Repositories;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAuthenticationService, AspNetIdentityAuthenticationService>();

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var sqlConnectionString = builder.Configuration.GetValue<string>("SqlConnectionString");

builder.Services
    .AddIdentityApiEndpoints<IdentityUser>()
    .AddDapperStores(options =>
    {
        options.ConnectionString = sqlConnectionString;
    });

builder.Services.AddTransient<Environment2DRepository, Environment2DRepository>(o => new Environment2DRepository(sqlConnectionString));
builder.Services.AddTransient<Object2DRepository, Object2DRepository>(o => new Object2DRepository(sqlConnectionString));

var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);

var app = builder.Build();

app.MapGet("/", () => $"The API is up success. Connection string found: {(sqlConnectionStringFound ? "yes" : "no")}");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGroup("/account")
.MapIdentityApi<IdentityUser>();

app.MapControllers()
    .RequireAuthorization();

app.Run();