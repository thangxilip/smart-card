using System.Text.Json.Serialization;
using SmartCard.Application;
using SmartCard.Domain.Configurations;
using SmartCard.Infrastructure.Data;
using SmartCard.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration.Get<AppConfig>() ?? throw new NullReferenceException("Invalid configuration");
builder.Services.Configure<GoogleSettings>(builder.Configuration.GetSection("Google"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddInfrastructureServices(config.ConnectionStrings.Default, builder.Environment);
builder.Services.AddApplicationServices();

builder.Services.AddAuthentication();
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AppDbContext>();

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
    x.AddPolicy("CorsPolicy", policyBuilder => 
        policyBuilder.WithOrigins("http://localhost:3000", "https://google.com")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
        )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors();

app.Run();