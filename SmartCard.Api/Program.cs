using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using SmartCard.Application;
using SmartCard.Domain.Configurations;
using SmartCard.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration.Get<AppConfig>() ?? throw new NullReferenceException("Invalid configuration");
builder.Services.AddInfrastructureServices(config.ConnectionStrings.Default, builder.Environment);
builder.Services.AddApplicationServices();

builder.Services.AddAuthentication()
    .AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

// var allowOrigins = Configuration.GetValue<string>("AllowOrigins");
builder.Services.AddCors(x => 
    x.AddPolicy("CorsPolicy", builder => 
        builder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
        )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // await app.InitialiseDatabaseAsync();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();