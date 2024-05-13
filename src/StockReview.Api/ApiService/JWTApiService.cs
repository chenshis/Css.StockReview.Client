using IdentityModel;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StockReview.Api.IApiService;
using StockReview.Domain;
using StockReview.Domain.Entities;
using StockReview.Infrastructure.Config;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace StockReview.Api.ApiService
{
    public class JWTApiService : IJWTApiService
    {

        private readonly JwtSecurityTokenHandler _jwtHandler = new JwtSecurityTokenHandler();
        private readonly StockReviewDbContext _dbContext;
        private readonly ILogger<JWTApiService> _logger;

        public JWTApiService(StockReviewDbContext dbContext, ILogger<JWTApiService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="userEntity"></param>
        /// <returns></returns>
        public string GetToken(UserEntity userEntity)
        {
            var claims = new Claim[5];
            claims[0] = new Claim(JwtClaimTypes.JwtId, userEntity.Jti);
            claims[1] = new Claim(ClaimTypes.Actor, userEntity.Contacts);
            claims[2] = new Claim(ClaimTypes.Name, userEntity.UserName);
            claims[3] = new Claim(ClaimTypes.PrimarySid, userEntity.Id.ToString());
            if (userEntity.Expires != null && userEntity.Expires < DateTime.Now)
            {
                claims[4] = new Claim(ClaimTypes.Role, RoleEnum.Free.ToString());
            }
            else
            {
                claims[4] = new Claim(ClaimTypes.Role, userEntity.Role.ToString());
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SystemConstant.JwtSecurityKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(SystemConstant.JwtIssuer, SystemConstant.JwtAudience, claims, null, DateTime.Now.AddHours(10).AddMinutes(1), credentials);
            var returnToken = _jwtHandler.WriteToken(token);
            return returnToken;
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="oldToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        string IJWTApiService.RefreshToken(string oldToken)
        {
            var jti = GetJtiByToken(oldToken);
            if (string.IsNullOrWhiteSpace(jti))
            {
                throw new Exception(SystemConstant.ErrorRefreshTokenFailMessage);
            }
            var userEntity = _dbContext.UserEntities.FirstOrDefault(t => t.Jti == jti);
            if (userEntity == null)
            {
                throw new Exception(SystemConstant.ErrorRefreshTokenFailMessage);
            }
            userEntity.Jti = Guid.NewGuid().ToString("N");
            var result = _dbContext.SaveChanges();
            if (result <= 0)
            {
                throw new Exception(SystemConstant.ErrorRefreshTokenFailMessage);
            }
            return GetToken(userEntity);
        }


        private string GetJtiByToken(string token)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = SystemConstant.JwtAudience,
                ValidIssuer = SystemConstant.JwtIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SystemConstant.JwtSecurityKey))
            };
            try
            {
                var securityToken = _jwtHandler.ValidateToken(token, validationParameters, out var validatedToken);
                var jwtToken = validatedToken as JwtSecurityToken;

                if (jwtToken != null)
                {
                    return jwtToken.Claims.Where(t => t.Type == JwtClaimTypes.JwtId).First().Value;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetJtiByToken)}：{ex.Message}");
                return null;
            }
        }

    }
}
