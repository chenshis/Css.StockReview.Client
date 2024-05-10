using Namotion.Reflection;
using Newtonsoft.Json;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using StockReview.Api.Mappers;
using StockReview.Domain;
using StockReview.Domain.Entities;
using StockReview.Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace StockReview.Api.ApiService
{
    public class LoginServerApiService : ILoginServerApiService
    {
        private readonly StockReviewDbContext _dbContext;

        public LoginServerApiService(StockReviewDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public UserEntity Login(AccountRequestDto accountRequest)
        {
            if (string.IsNullOrWhiteSpace(accountRequest.Password))
            {
                throw new Exception(SystemConstant.ErrorEmptyPasswordMessage);
            }
            if (string.IsNullOrWhiteSpace(accountRequest.UserName))
            {
                throw new Exception(SystemConstant.ErrorEmptyUserNameMessage);
            }
            var userPassword = accountRequest.Password;
            var userName = accountRequest.UserName;
            var userEntity = _dbContext.UserEntities.FirstOrDefault(t => t.UserName == userName && t.Password == userPassword);
            if (userEntity == null)
            {
                throw new Exception(SystemConstant.ErrorUserOrPasswordMessage);
            }
            return userEntity;
        }

        public bool Register(RegisterRequestDto registerRequest)
        {
            string GetErrorMessage(string propertyName, string errorTemplate)
            {
                var errorName = GetDocSummary(typeof(RegisterRequestDto), propertyName);
                return string.Concat(SystemConstant.ErrorIcon, string.Format(errorTemplate, errorName));
            }
            if (string.IsNullOrWhiteSpace(registerRequest.UserName))
            {
                throw new Exception(GetErrorMessage(nameof(registerRequest.UserName), SystemConstant.ErrorEmptyMessage));
            }
            var userNameLength = registerRequest.UserName.Length;
            if (userNameLength < 5 || userNameLength > 20)
            {
                throw new Exception(SystemConstant.ErrorUserNameLengthMessage);
            }
            if (string.IsNullOrWhiteSpace(registerRequest.Password))
            {
                throw new Exception(GetErrorMessage(nameof(registerRequest.Password), SystemConstant.ErrorEmptyMessage));
            }
            if (string.IsNullOrWhiteSpace(registerRequest.Contacts))
            {
                throw new Exception(GetErrorMessage(nameof(registerRequest.Contacts), SystemConstant.ErrorEmptyMessage));
            }
            if (string.IsNullOrWhiteSpace(registerRequest.Phone))
            {
                throw new Exception(GetErrorMessage(nameof(registerRequest.Phone), SystemConstant.ErrorEmptyMessage));
            }
            if (!Regex.IsMatch(registerRequest.Phone, @"^1[3-9][0-9]{9}$"))
            {
                throw new Exception(SystemConstant.ErrorPhoneRuleMessage);
            }
            if (string.IsNullOrWhiteSpace(registerRequest.QQ))
            {
                throw new Exception(GetErrorMessage(nameof(registerRequest.QQ), SystemConstant.ErrorEmptyMessage));
            }
            if (!Regex.IsMatch(registerRequest.QQ, @"^[1-9][0-9]{4,9}$"))
            {
                throw new Exception(SystemConstant.ErrorQQRuleMessage);
            }

            var userEntity = _dbContext.UserEntities.FirstOrDefault(t => t.UserName == registerRequest.UserName
                                                                         || t.Phone == registerRequest.Phone
                                                                         || t.QQ == registerRequest.QQ);
            if (userEntity != null)
            {
                if (userEntity.UserName == registerRequest.UserName)
                {
                    throw new Exception(GetErrorMessage(nameof(registerRequest.UserName), SystemConstant.ErrorExistMessage));
                }
                if (userEntity.Phone == registerRequest.Phone)
                {
                    throw new Exception(GetErrorMessage(nameof(registerRequest.Phone), SystemConstant.ErrorExistMessage));
                }
                if (userEntity.QQ == registerRequest.QQ)
                {
                    throw new Exception(GetErrorMessage(nameof(registerRequest.QQ), SystemConstant.ErrorExistMessage));
                }
            }
            var addUserEntity = registerRequest.ToEntity();

            _dbContext.UserEntities.Add(addUserEntity);
            var result = _dbContext.SaveChanges();

            return result > SystemConstant.Zero;
        }


        public List<MenuDto> GetMenus(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return new List<MenuDto>();
            }
            if (!Enum.TryParse(role, out RoleEnum result))
            {
                return new List<MenuDto>();
            }
            var menuFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SystemConstant.MenuJson);
            if (!File.Exists(menuFile))
            {
                throw new Exception(SystemConstant.ErrorMenuNotExist);
            }
            var strMenuInfo = File.ReadAllText(menuFile);

            try
            {
                var deserializeMenuDtos = JsonConvert.DeserializeObject<List<MenuDto>>(strMenuInfo);
                List<MenuDto> menuDtos = new List<MenuDto>();
                foreach (var item in deserializeMenuDtos)
                {
                    if ((item.MenuRole & result) > 0)
                    {
                        menuDtos.Add(item);
                    }
                }
                return menuDtos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 获取属性名称
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        private string GetDocSummary(Type type, string propertyName)
        {
            return type.GetProperty(propertyName).GetXmlDocsSummary();
        }

        public bool ForgotPassword(ForgotPasswordRequestDto request)
        {
            string GetErrorMessage(string propertyName, string errorTemplate)
            {
                var errorName = GetDocSummary(typeof(ForgotPasswordRequestDto), propertyName);
                return string.Concat(SystemConstant.ErrorIcon, string.Format(errorTemplate, errorName));
            }
            if (string.IsNullOrWhiteSpace(request.UserName))
            {
                throw new Exception(GetErrorMessage(nameof(request.UserName), SystemConstant.ErrorEmptyMessage));
            }
            if (string.IsNullOrWhiteSpace(request.QQ))
            {
                throw new Exception(GetErrorMessage(nameof(request.QQ), SystemConstant.ErrorEmptyMessage));
            }
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                throw new Exception(GetErrorMessage(nameof(request.Password), SystemConstant.ErrorEmptyMessage));
            }
            if (string.IsNullOrWhiteSpace(request.ConfirmPassword))
            {
                throw new Exception(GetErrorMessage(nameof(request.ConfirmPassword), SystemConstant.ErrorEmptyMessage));
            }
            if (request.ConfirmPassword != request.Password)
            {
                throw new Exception(SystemConstant.ErrorInconsistentConfirmPwd);
            }
            var userEntity = _dbContext.UserEntities.FirstOrDefault(t => t.UserName == request.UserName && t.QQ == request.QQ);
            if (userEntity == null)
            {
                throw new Exception(SystemConstant.ErrorInconsistentUserNameOrQQ);
            }
            userEntity.Password = request.Password;
            return _dbContext.SaveChanges() > SystemConstant.Zero;
        }

        public bool UpdatePassword(UpdatePasswordRequestDto request)
        {
            string GetErrorMessage(string propertyName, string errorTemplate)
            {
                var errorName = GetDocSummary(typeof(UpdatePasswordRequestDto), propertyName);
                return string.Concat(SystemConstant.ErrorIcon, string.Format(errorTemplate, errorName));
            }
            if (string.IsNullOrWhiteSpace(request.QQ))
            {
                throw new Exception(GetErrorMessage(nameof(request.QQ), SystemConstant.ErrorEmptyMessage));
            }
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                throw new Exception(GetErrorMessage(nameof(request.Password), SystemConstant.ErrorEmptyMessage));
            }
            if (string.IsNullOrWhiteSpace(request.NewPassword))
            {
                throw new Exception(GetErrorMessage(nameof(request.NewPassword), SystemConstant.ErrorEmptyMessage));
            }
            if (request.ConfirmPassword != request.NewPassword)
            {
                throw new Exception(SystemConstant.ErrorInconsistentConfirmPwd);
            }
            var userEntity = _dbContext.UserEntities.FirstOrDefault(t => t.UserName == request.UserName
                                                                         && t.QQ == request.QQ
                                                                         && t.Password == request.Password);
            if (userEntity == null)
            {
                throw new Exception(SystemConstant.ErrorInconsistentUserNameOrQQ);
            }
            userEntity.Password = request.NewPassword;
            return _dbContext.SaveChanges() > SystemConstant.Zero;
        }
    }
}
