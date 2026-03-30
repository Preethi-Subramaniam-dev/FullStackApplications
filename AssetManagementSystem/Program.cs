using Serilog;
using AssetManagementSystem.Data;
using AssetManagementSystem.Data.Repository;
using AssetManagementSystem.MapperConfiguration;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.File("Logs/debuglogs.txt", rollingInterval: RollingInterval.Hour)
        .CreateLogger();

builder.Host.UseSerilog();
builder.Logging.AddSerilog();
builder.Services.AddDbContext<AssetManagementDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AssetManagementDBCOnnection"));
});
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(cfg => { }, typeof(AutoMapperConfig));
builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
    options.AddPolicy("AllowOnlyLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowOnlyLocalhost");
app.MapControllers();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/", () => "API is running...");

app.Run();
