using Microsoft.EntityFrameworkCore;
using StockReview.Domain;
using StockReview.Infrastructure.Config;
using StockReview.Server.Exceptions;
using StockReview.Server.UserValidator;

var builder = WebApplication.CreateBuilder(args);
// 连接字符串
var connectionString = builder.Configuration.GetConnectionString(SystemConstant.DefaultConnection);
// Add services to the container.
builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryApiResources(IdentityServerConfiguration.GetApiResources())
    .AddInMemoryApiScopes(IdentityServerConfiguration.GetApiScopes())
    .AddInMemoryClients(IdentityServerConfiguration.GetClients())
    .AddUsers();
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
//异常中间件
app.UseException();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();

app.Run();
