using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using StockReview.Api.IApiService;
using StockReview.Domain.Entities;
using StockReview.Infrastructure.Config;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StockReview.Api.ApiService
{
    public class JWTApiService : IJWTApiService
    {
        string IJWTApiService.GetToken(UserEntity userEntity)
        {
            var claims = new Claim[]
            {
                 new Claim(JwtClaimTypes.Id,userEntity.Id.ToString()),
                 new Claim(JwtClaimTypes.Role,userEntity.Role.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SystemConstant.JwtSecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: SystemConstant.JwtIssuer,
                audience: SystemConstant.JwtAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),//5分钟有效期
                notBefore: DateTime.Now.AddMinutes(1),//1分钟后有效
                signingCredentials: creds);
            var returnToken = new JwtSecurityTokenHandler().WriteToken(token);
            return returnToken;
        }
    }
}
