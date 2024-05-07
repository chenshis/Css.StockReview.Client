using IdentityModel;
using IdentityServer4.Test;
using StockReview.Domain;
using StockReview.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StockReview.Server.UserValidator
{
    public class UserStore
    {
        private readonly StockReviewDbContext _dbContext;

        public UserStore(StockReviewDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public bool ValidateCredentials(string username, string password)
        {
            var user = FindByUsername(username);
            if (user != null)
            {
                if (string.IsNullOrWhiteSpace(user.Password) && string.IsNullOrWhiteSpace(password))
                {
                    return true;
                }

                return user.Password.Equals(password);
            }

            return false;
        }

        //
        // 摘要:
        //     Finds the user by subject identifier.
        //
        // 参数:
        //   subjectId:
        //     The subject identifier.
        public UserEntity FindBySubjectId(string subjectId)
        {
            if (long.TryParse(subjectId, out var id))
            {
                return _dbContext.UserEntities.FirstOrDefault(t => t.Id == id);
            }
            return default;
        }

        //
        // 摘要:
        //     Finds the user by username.
        //
        // 参数:
        //   username:
        //     The username.
        public UserEntity FindByUsername(string username)
        {
            return _dbContext.UserEntities.FirstOrDefault((x) => x.UserName == username);
        }

    }
}
