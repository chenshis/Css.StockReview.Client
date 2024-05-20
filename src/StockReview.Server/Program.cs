using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StockReview.Api.ApiService;
using StockReview.Api.IApiService;
using StockReview.Domain;
using StockReview.Domain.Entities;
using StockReview.Infrastructure.Config;
using StockReview.Server;
using StockReview.Server.BackgroundServices;
using StockReview.Server.Exceptions;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// 连接字符串
var connectionString = builder.Configuration.GetConnectionString(SystemConstant.DefaultConnection);
// Add services to the container.
builder.Services.AddScoped<IStockOutlookServerApiService, StockOutlookServerApiService>();
builder.Services.AddScoped<IJWTApiService, JWTApiService>();
builder.Services.AddScoped<ILoginServerApiService, LoginServerApiService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IReplayService, ReplayService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = SystemConstant.JwtAudience,
        ValidIssuer = SystemConstant.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SystemConstant.JwtSecurityKey)),
        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
        {
            return expires >= DateTime.Now;
        }
    };
});
builder.Services.AddAuthorization(options =>
{
    //options.AddPolicy(nameof(RoleEnum.), policyBuilder => policyBuilder.Requirements.Add(new AdminRequirement()));
});
builder.Services.AddDbContext<StockReviewDbContext>((options) =>
{
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36)));
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient(SystemConstant.TodayLonghuVipUrl, configClient =>
{
    configClient.BaseAddress = new Uri(SystemConstant.TodayLonghuVipUrl);
    configClient.DefaultRequestHeaders.UserAgent.ParseAdd(SystemConstant.UserAgent);
});
builder.Services.AddHttpClient(SystemConstant.HistoryLonghuVipUrl, configClient =>
{
    configClient.BaseAddress = new Uri(SystemConstant.HistoryLonghuVipUrl);
    configClient.DefaultRequestHeaders.UserAgent.ParseAdd(SystemConstant.UserAgent);
});
builder.Services.AddHttpClient(SystemConstant.SpecialLonghuVipUrl, configClient =>
{
    configClient.BaseAddress = new Uri(SystemConstant.SpecialLonghuVipUrl);
    configClient.DefaultRequestHeaders.UserAgent.ParseAdd(SystemConstant.UserAgent);
});
builder.Services.AddMemoryCache();
// 后台服务
builder.Services.AddHostedService<BulletinBoardBackgroundService>();
builder.Services.AddHostedService<StockFilterDatesBackgroundService>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();
//异常中间件
app.UseException();
app.MapControllers();

app.Run();