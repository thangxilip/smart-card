using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SmartCard.Domain.Configurations;

namespace SmartCard.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    AppDbContext IDesignTimeDbContextFactory<AppDbContext>.CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var appConfig = configuration.Get<AppConfig>() ?? throw new NullReferenceException("Invalid configuration");
        Console.WriteLine(appConfig.ConnectionStrings.Default);
        // var httpContextAccessor = new HttpContextAccessor();
        // var requestContext = new RequestContext(httpContextAccessor);
        // var options = Options.Create<AppConfig>(appConfig);

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(appConfig.ConnectionStrings.Default);
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();
        //
        // var npgsqlBuilderFunc = new Action<NpgsqlDbContextOptionsBuilder>(builder => builder.MigrationsHistoryTable("__EFMigrationsHistory", "public"));
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.
            UseNpgsql(dataSource).
            UseSnakeCaseNamingConvention();

        return new AppDbContext(builder.Options);
    }
}