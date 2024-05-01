using System;
using System.Collections.Generic;
using System.Text;

namespace StockReview.Domain.Entities
{
    /// <summary>
    /// 菜单实体
    /// </summary>
    public class MenuEntity
    {
        /// <summary>
        /// 菜单主键
        /// </summary>
        public int MenuId { get; set; }
        /// <summary>
        /// 菜单头部
        /// </summary>
        public string MenuHeader { get; set; }
        /// <summary>
        /// 目标视图
        /// </summary>
        public string TargetView { get; set; }
        /// <summary>
        /// 父级主键
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string MenuIcon { get; set; }
        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 菜单角色
        /// </summary>
        public MenuRoleEnum MenuRole { get; set; }
    }

    /// <summary>
    /// 菜单角色枚举
    /// </summary>
    public enum MenuRoleEnum
    {
        /// <summary>
        /// 免费
        /// </summary>
        Free,
        /// <summary>
        /// 普通
        /// </summary>
        Ordinary,
        /// <summary>
        /// vip会员
        /// </summary>
        VIP
    }
}
