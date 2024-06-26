﻿using StockReview.Domain.Base;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockReview.Domain.Entities
{
    [Table("sys_user")]
    public class UserEntity : BizEntityBase
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// 角色;1，2，4，8 免费用户 普通用户 vip 管理员
        /// </summary>
        public RoleEnum Role { get; set; }

        /// <summary>
        /// jwt id 用于刷新token
        /// </summary>
        public string Jti { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? Expires { get; set; }

    }

    /// <summary>
    /// 角色枚举
    /// </summary>
    [Flags]
    public enum RoleEnum
    {
        /// <summary>
        /// 免费
        /// </summary>
        [Description("免费")]
        Free = 1,
        /// <summary>
        /// 普通
        /// </summary>
        [Description("普通")]
        Ordinary = 2,
        /// <summary>
        /// vip
        /// </summary>
        [Description("会员")]
        VIP = 4,
        /// <summary>
        /// 管理员
        /// </summary>
        [Description("管理员")]
        Admin = 8
    }
}
