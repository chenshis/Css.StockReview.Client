using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace StockReview.Infrastructure.Config
{
    /// <summary>
    /// 认证服务配置
    /// </summary>
    public static class IdentityServerConfiguration
    {
        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new[] {
                new Client{
                    ClientId=SystemConstant.IdentityServerClient,
                    ClientSecrets= new []{ new Secret(SystemConstant.IdentityServerSecret.Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = new[] {SystemConstant.IdentityServerScope}
                }
            };
        }

        /// <summary>
        /// 获取api作用域
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new[]
            {
                new ApiScope(SystemConstant.IdentityServerScope, SystemConstant.IdentityServerScopeDisplayName)
            };
        }

        /// <summary>
        /// 资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource(SystemConstant.IdentityServerResource,SystemConstant.IdentityServerResourceDisplayName)
                {
                    Scopes = new[]
                    {
                        SystemConstant.IdentityServerScope
                    }
                }
            };
        }

    }
}
