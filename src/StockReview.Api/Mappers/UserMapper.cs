using StockReview.Api.Dtos;
using StockReview.Domain.Entities;
using System;
using System.Collections.Generic;

namespace StockReview.Api.Mappers
{
    /// <summary>
    /// 用户映射类
    /// </summary>
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
            userEntity.Role = RoleEnum.Free;
            userEntity.Password = dto.Password;
            userEntity.Jti = Guid.NewGuid().ToString("N");
            return userEntity;
        }

        public static UserDto ToDto(this UserEntity userEntity)
        {
            if (userEntity == null)
            {
                return new UserDto();
            }
            return new UserDto
            {
                Contacts = userEntity.Contacts,
                CreateTime = userEntity.CreateTime,
                UserName = userEntity.UserName,
                Expires = userEntity.Expires
            };
        }

        public static List<UserDto> ToDtos(this List<UserEntity> userEntities)
        {
            if (userEntities == null || userEntities.Count <= 0)
            {
                return new List<UserDto>();
            }
            List<UserDto> userDtos = new();
            foreach (UserEntity userEntity in userEntities)
            {
                userDtos.Add(new UserDto
                {
                    Contacts = userEntity.Contacts,
                    CreateTime = userEntity.CreateTime,
                    Phone = userEntity.Phone,
                    QQ = userEntity.QQ,
                    Role = userEntity.Role,
                    UserName = userEntity.UserName,
                    Expires = userEntity.Expires
                });
            }
            return userDtos;
        }
    }
}
