using StockReview.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockReview.Api.IApiService
{
    /// <summary>
    /// 菜单接口
    /// </summary>
    public interface ILoginApiService
    {
        List<MenuEntity> GetMenus();
    }
}
