using IdentityServer4.Test;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;

namespace StockReview.Server.UserValidator
{
    public class UserResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserStore _users;

        private readonly ISystemClock _clock;


        public UserResourceOwnerPasswordValidator(UserStore users, ISystemClock clock)
        {
            _users = users;
            _clock = clock;
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (_users.ValidateCredentials(context.UserName, context.Password))
            {
                var user = _users.FindByUsername(context.UserName);
                context.Result = new GrantValidationResult(user.Id.ToString() ?? throw new ArgumentException("Subject ID not set", "SubjectId"), "pwd", _clock.UtcNow.UtcDateTime);
            }
            return Task.CompletedTask;
        }
    }
}
