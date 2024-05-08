using StockReview.Api.Dtos;
using StockReview.Domain.Entities;
using StockReview.Infrastructure.Config.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.Mappers
{
    public static class UserMapper
    {
        /// <summary>
        /// 用户实体映射
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static UserEntity ToEntity(this RegisterRequestDto dto)
        {
            UserEntity userEntity = new UserEntity();
            userEntity.UserName = dto.UserName;
            userEntity.Contacts = dto.Contacts;
            userEntity.Phone = dto.Phone;
            userEntity.QQ = dto.QQ;
            userEntity.Password = dto.Password.GetMd5();
            return userEntity;
        }
    }
}
