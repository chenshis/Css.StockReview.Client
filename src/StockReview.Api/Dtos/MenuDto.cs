using StockReview.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.Dtos
{
    public class MenuDto
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
        public RoleEnum MenuRole { get; set; }
    }
}
