
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Teams;

namespace TaskManager.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITeamService, TeamService>();
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
    }
}
