using StockReview.Api.Dtos;
using StockReview.Domain.Entities;

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
            userEntity.Role = RoleEnum.Free;
            userEntity.Password = dto.Password;
            return userEntity;
        }
    }
}
