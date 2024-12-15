using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using SmartCard.Domain.Repositories.Base;
using SmartCard.Infrastructure.Identity;
using SmartCard.Infrastructure.Repositories.Base;

namespace SmartCard.Infrastructure.Data;

public static class RegisterDataService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString, IHostEnvironment env)
    {
        var dataSource = new NpgsqlDataSourceBuilder(connectionString).Build();
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(dataSource)
                .UseSnakeCaseNamingConvention());
        
        // services.AddScoped<ApplicationDbContextInitializer>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services
            .AddIdentityCore<User>()
            .AddRoles<Role>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints();

        return services;
    }
}