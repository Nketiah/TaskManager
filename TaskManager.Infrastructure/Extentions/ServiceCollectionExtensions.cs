

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Interfaces;
using TaskManager.Infrastructure.Persistence;
using TaskManager.Infrastructure.Repositories;

namespace TaskManager.Infrastructure.Extentions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var conn = configuration.GetConnectionString("TaskManagerConnection");
        services.AddDbContext<TaskManagerDbContext>(options => options.UseSqlServer(conn));
        services.AddScoped<ITeamRepository, TeamRepository>();
        //services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
    }
}
