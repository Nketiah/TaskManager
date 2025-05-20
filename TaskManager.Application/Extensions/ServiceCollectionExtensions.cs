
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Account;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Services;
using TaskManager.Application.Teams;

namespace TaskManager.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITeamService, TeamService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
    }
}
