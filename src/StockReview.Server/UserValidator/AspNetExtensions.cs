using IdentityServer4.Test;

namespace StockReview.Server.UserValidator
{
    public static class AspNetExtensions
    {
        public static IIdentityServerBuilder AddUsers(this IIdentityServerBuilder builder)
        {
            builder.Services.AddScoped<UserStore>();
            builder.AddProfileService<UserProfileService>();
            builder.AddResourceOwnerValidator<UserResourceOwnerPasswordValidator>();
            return builder;
        }
    }
}
