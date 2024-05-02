using StockReview.Api.IApiService;
using StockReview.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockReview.Api.ApiService
{
    public class LoginApiService : ILoginApiService
    {
        public List<MenuEntity> GetMenus()
        {
            var menuEntities = new List<MenuEntity>();
            //todo 调用接口
            menuEntities.Add(new MenuEntity()
            {
                MenuHeader = "系统用户",
                MenuId = 1,
                TargetView = "UserManagementView",
                ParentId = 0,
                Index = 1,
                MenuIcon = null,
                MenuRole = 0,
            });
            menuEntities.Add(new MenuEntity()
            {
                MenuHeader = "股市看盘",
                MenuId = 2,
                TargetView = "StockOutlookView",
                ParentId = 0,
                Index = 2,
                MenuIcon = null,
                MenuRole = 0,
            });

            return menuEntities;
        }
    }
}
