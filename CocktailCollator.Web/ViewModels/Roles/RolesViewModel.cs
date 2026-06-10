using AutoMapper;
using CocktailCollator.Infrastructure.Persistence.Models;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CocktailCollator.Web.ViewModels.Roles;

public class RolesViewModel
{
    private readonly IMapper _mapper;
    private readonly RoleManager<CocktailRole> _roleManager;

    public IAsyncRelayCommand GetCommand { get; }

    public List<RoleViewModel> Roles { get; private set; } = [];

    public string Error { get; private set; } = string.Empty;

    public RolesViewModel(
        RoleManager<CocktailRole> roleManager,
        IMapper mapper)
    {
        this._mapper = mapper;
        this._roleManager = roleManager;

        this.GetCommand = new AsyncRelayCommand(this.GetRolesAsync);
    }

    private async Task GetRolesAsync(CancellationToken cancellationToken)
    {
        try
        {
            this.Error = string.Empty;
            var _Roles = await this._roleManager.Roles.ToListAsync(cancellationToken);
            this.Roles = this._mapper.Map<List<RoleViewModel>>(_Roles);
        }
        catch (Exception ex)
        {
            this.Error = $"An error occurred while retrieving roles: {ex.Message}";
        }
    }
}
