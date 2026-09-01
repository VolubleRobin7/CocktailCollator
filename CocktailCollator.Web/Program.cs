using CocktailCollator.Application;
using CocktailCollator.Infrastructure;
using CocktailCollator.Infrastructure.Persistence;
using CocktailCollator.Infrastructure.Persistence.Models;
using CocktailCollator.Web;
using CocktailCollator.Web.Infrastructure.Authentication;
using CocktailCollator.Web.Views;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .InjectApplication()
    .InjectInfrastructure(builder.Configuration)
    .InjectWeb(builder.Configuration);

builder.Services.AddAutoMapper(
    typeof(CocktailCollator.Web.DependencyInjector),
    typeof(CocktailCollator.Application.DependencyInjector));

builder.Services.AddDbContext<CocktailDbContext>(options
    => options.UseSqlite(builder.Configuration.GetConnectionString("CocktailCollator")));

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

using (var _Scope = app.Services.CreateScope())
{
    var _UserManager = _Scope.ServiceProvider.GetRequiredService<UserManager<CocktailUser>>();
    var _RoleManager = _Scope.ServiceProvider.GetRequiredService<RoleManager<CocktailRole>>();
    var _DbContext = _Scope.ServiceProvider.GetRequiredService<CocktailDbContext>();

    // Create default admin user and roles if they don't exist
    if (!_RoleManager.Roles.Any())
    {
        var adminRole = new CocktailRole { Name = "Admin", HasEveryPermissionClaim = true };
        _ = await _RoleManager.CreateAsync(adminRole);

        var userRole = new CocktailRole { Name = "User", DefaultRole = true };
        _ = await _RoleManager.CreateAsync(userRole);
        _ = await _RoleManager.AddClaimAsync(userRole, new Claim(CocktailCollator.Web.Infrastructure.Authentication.ClaimTypes.Permission, ClaimValues.Permissions.Ingredients.View));
        _ = await _RoleManager.AddClaimAsync(userRole, new Claim(CocktailCollator.Web.Infrastructure.Authentication.ClaimTypes.Permission, ClaimValues.Permissions.Measurements.View));
    }

    if (!_DbContext.Users.Any())
    {
        var _AdminUser = new CocktailUser
        {
            UserName = "Admin",
            Email = "admin@cocktailcollator.local",
            EmailConfirmed = true
        };

        var _Result = await _UserManager.CreateAsync(_AdminUser, "Cockt@!1");
        if (_Result.Succeeded)
        {
            Console.WriteLine("Default admin user 'Admin' created successfully.");
            _ = await _UserManager.AddToRoleAsync(_AdminUser, "Admin");
        }
        else
        {
            Console.WriteLine("Failed to create default admin user:");
            foreach (var _Error in _Result.Errors)
            {
                Console.WriteLine($"  - {_Error.Code}: {_Error.Description}");
            }
        }
    }

    // Ensure roles with HasEveryPermissionClaim have all possible claims
    var _RolesWithAllClaims = await _RoleManager.Roles.Where(r => r.HasEveryPermissionClaim).ToListAsync();
    var _AllPossibleClaims = ClaimValues.Permissions.GetAll();

    foreach (var _Role in _RolesWithAllClaims)
    {
        var _CurrentClaims = await _RoleManager.GetClaimsAsync(_Role);
        var _CurrentClaimValues = _CurrentClaims.Where(c => c.Type == CocktailCollator.Web.Infrastructure.Authentication.ClaimTypes.Permission).Select(c => c.Value).ToList();

        var _MissingClaims = _AllPossibleClaims.Except(_CurrentClaimValues);
        foreach (var _Claim in _MissingClaims)
        {
            await _RoleManager.AddClaimAsync(_Role, new Claim(CocktailCollator.Web.Infrastructure.Authentication.ClaimTypes.Permission, _Claim));
        }
    }
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

var fileStorePath = builder.Configuration.GetValue<string>("FileStorePath");
if (!string.IsNullOrEmpty(fileStorePath))
{
    if (!Directory.Exists(fileStorePath))
        throw new DirectoryNotFoundException($"The configured FileStorePath '{fileStorePath}' does not exist. Please ensure the directory exists and is accessible.");

    _ = app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(fileStorePath),
        RequestPath = "/files"
    });
}
else
{
    throw new InvalidOperationException("FileStorePath is not configured.");
}

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
