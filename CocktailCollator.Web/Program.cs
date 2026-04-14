using CocktailCollator.Application;
using CocktailCollator.Infrastructure;
using CocktailCollator.Infrastructure.Persistence;
using CocktailCollator.Web;
using CocktailCollator.Web.Views;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .InjectApplication()
    .InjectInfrastructure()
    .InjectWeb();

builder.Services.AddAutoMapper(
    typeof(CocktailCollator.Web.DependencyInjector),
    typeof(CocktailCollator.Application.DependencyInjector));

builder.Services.AddDbContext<CocktailDbContext>(options
    => options.UseSqlServer(builder.Configuration.GetConnectionString("CocktailCollator")));

var app = builder.Build();

// Check if the SQL server is available
if (builder.Configuration.GetValue<bool>("WaitForSqlServer", true))
{
    // this section needs improvement
    var _ConnectionBuilder = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("CocktailCollator"))
    {
        InitialCatalog = "master",
        ConnectTimeout = 5,
        ConnectRetryCount = 0,
        ConnectRetryInterval = 1
    };
    _ConnectionBuilder.DataSource = "tcp:" + _ConnectionBuilder.DataSource;

    var _Retries = 10;
    while (true)
    {
        try
        {
            using var _SqlConnection = new SqlConnection(_ConnectionBuilder.ConnectionString);
            _SqlConnection.Open();
            break;
        }
        catch
        {
            Console.WriteLine("Waiting for SQL Server. Retrying connection...");

            _Retries--;
            if (_Retries == 0)
                throw;

            Thread.Sleep(3000);
        }
    }
}

// Apply pending migrations at startup
if (builder.Configuration.GetValue<bool>("RunMigrationsOnStartup"))
{
    using var _Scope = app.Services.CreateScope();
    var _DbContext = _Scope.ServiceProvider.GetRequiredService<CocktailDbContext>();
    _DbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    _ = app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
