

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Infrastructure.Extentions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var conn = configuration.GetConnectionString("TaskManagerConnection");
        services.AddDbContext<TaskManagerDbContext>(options => options.UseSqlServer(conn));

        //services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();
        //services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
    }
}
