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
// �����ַ���
var connectionString = builder.Configuration.GetConnectionString(SystemConstant.DefaultConnection);
// Add services to the container.
builder.Services.AddScoped<IJWTApiService, JWTApiService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,//�Ƿ���֤Issuer
        ValidateAudience = true,//�Ƿ���֤Audience
        ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
        ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
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
//�쳣�м��
app.UseException();
app.MapControllers();

app.Run();
