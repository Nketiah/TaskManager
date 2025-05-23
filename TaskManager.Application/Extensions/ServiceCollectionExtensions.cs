
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Account;
using TaskManager.Application.ActivityLogs;
using TaskManager.Application.Services;
using TaskManager.Application.Tasks;
using TaskManager.Application.Teams;


namespace TaskManager.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITeamService, TeamService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IActivityLogService, ActivityLogService>();
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
    }
}
