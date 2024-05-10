﻿using Namotion.Reflection;
using Newtonsoft.Json;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using StockReview.Api.Mappers;
using StockReview.Domain;
using StockReview.Domain.Entities;
using StockReview.Infrastructure.Config;
using StockReview.Infrastructure.Config.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
            if (string.IsNullOrWhiteSpace(registerRequest.QQ))
            {
                throw new Exception(GetErrorMessage(nameof(registerRequest.QQ), SystemConstant.ErrorEmptyMessage));
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
    }
}
