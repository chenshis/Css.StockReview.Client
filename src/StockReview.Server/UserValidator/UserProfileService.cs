using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Security.Claims;

namespace StockReview.Server.UserValidator
{
    public class UserProfileService : IProfileService
    {
        private readonly UserStore Users;
        private readonly ILogger<UserProfileService> Logger;

        public UserProfileService(UserStore users, ILogger<UserProfileService> logger)
        {
            this.Users = users;
            this.Logger = logger;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.LogProfileRequest(Logger);
            if (context.RequestedClaimTypes.Any())
            {
                var user = Users.FindBySubjectId(context.Subject.GetSubjectId());
                if (user != null)
                {
                    var claims = new HashSet<Claim>(new ClaimComparer());
                    claims.Add(new Claim(JwtClaimTypes.Role, user.Role.ToString()));
                    claims.Add(new Claim(JwtClaimTypes.NickName, user.Contacts));
                    context.AddRequestedClaims(claims);
                }
            }

            context.LogIssuedClaims(Logger);
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = Users.FindBySubjectId(context.Subject.GetSubjectId()) == null ? false : true;
            return Task.CompletedTask;
        }
    }
}
