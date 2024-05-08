using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StockReview.Api.ApiService;
using StockReview.Api.IApiService;
using StockReview.Domain;
using StockReview.Infrastructure.Config;
using StockReview.Server.Exceptions;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// 连接字符串
var connectionString = builder.Configuration.GetConnectionString(SystemConstant.DefaultConnection);
// Add services to the container.
builder.Services.AddScoped<IJWTApiService, JWTApiService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,//是否验证Issuer
        ValidateAudience = true,//是否验证Audience
        ValidateLifetime = true,//是否验证失效时间
        ValidateIssuerSigningKey = true,//是否验证SecurityKey
        ValidAudience = SystemConstant.JwtAudience,
        ValidIssuer = SystemConstant.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SystemConstant.JwtSecurityKey))
    };
});
builder.Services.AddDbContext<StockReviewDbContext>((options) =>
{
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36)));
});
builder.Services.AddAuthorization();
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
app.UseAuthentication();
app.UseAuthorization();
//异常中间件
app.UseException();
app.MapControllers();

app.Run();
