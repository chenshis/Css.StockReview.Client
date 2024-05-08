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
                 new Claim(JwtClaimTypes.JwtId,Guid.NewGuid().ToString()),
                 new Claim(JwtClaimTypes.Actor,SystemConstant.JwtActor),
                 new Claim(JwtClaimTypes.NickName,userEntity.Contacts),
                 new Claim(JwtClaimTypes.Id,userEntity.Id.ToString()),
                 new Claim(JwtClaimTypes.Role,userEntity.Role.ToString())
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SystemConstant.JwtSecurityKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: SystemConstant.JwtIssuer,
                audience: SystemConstant.JwtAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(1),//5分钟有效期
                signingCredentials: credentials);
            var returnToken = new JwtSecurityTokenHandler().WriteToken(token);
            return returnToken;
        }
    }
}
