using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.EntityFrameworkCore;
using StockReview.Server.Exceptions;
using System.Reflection;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// assembly
var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
var serverVersion = new MySqlServerVersion(new Version(8, 0, 36));
builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = builder =>
        {
            builder.UseMySql(connectionString, serverVersion, sql => sql.MigrationsAssembly(migrationsAssembly));
        };
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = builder =>
        {
            builder.UseMySql(connectionString, serverVersion, sql => sql.MigrationsAssembly(migrationsAssembly));
        };
        options.EnableTokenCleanup = true;
        options.TokenCleanupInterval = 3600;
    })
    .AddTestUsers(GetUsers());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//异常中间件
app.UseException();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();

app.Run();


static List<TestUser> GetUsers()
{
    return new List<TestUser>()
            {
                new TestUser()
                {
                     Username="Eleven",
                     Password="123456",
                     SubjectId="0",
                     Claims=new List<Claim>(){
                        new Claim(IdentityModel.JwtClaimTypes.Role,"Admin"),
                        new Claim(IdentityModel.JwtClaimTypes.NickName,"Eleven"),
                        new Claim("eMail","57265177@qq.com")
                    }
                }
            };
}