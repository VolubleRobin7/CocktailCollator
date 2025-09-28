using CocktailCollator.Application;
using CocktailCollator.Infrastructure;
using CocktailCollator.Infrastructure.Persistence;
using CocktailCollator.Web;
using CocktailCollator.Web.Views;
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
