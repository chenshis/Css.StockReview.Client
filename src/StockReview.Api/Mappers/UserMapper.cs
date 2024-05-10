using StockReview.Api.Dtos;
using StockReview.Domain.Entities;
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
            return userEntity;
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
                    Role = RoleEnum.Free,
                    UserName = userEntity.UserName
                });
            }
            return userDtos;
        }
    }
}
