using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using StockReview.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockReview.Api.ApiService
{
    public class LoginApiService : ILoginApiService
    {

        public List<MenuDto> GetMenus()
        {
            var menuEntities = new List<MenuDto>();
            //todo 调用接口
            menuEntities.Add(new MenuDto()
            {
                MenuHeader = "系统用户",
                MenuId = 1,
                TargetView = "UserManagementView",
                ParentId = 0,
                Index = 1,
                MenuIcon = null,
                MenuRole = 0,
            });
            menuEntities.Add(new MenuDto()
            {
                MenuHeader = "股市看盘",
                MenuId = 2,
                TargetView = "StockOutlookView",
                ParentId = 0,
                Index = 2,
                MenuIcon = null,
                MenuRole = 0,
            });
            menuEntities.Add(new MenuDto()
            {
                MenuHeader = "龙虎榜",
                MenuId = 3,
                TargetView = "DragonTigerView",
                ParentId = 0,
                Index = 3,
                MenuIcon = null,
                MenuRole = 0,
            });
            menuEntities.Add(new MenuDto()
            {
                MenuHeader = "龙头晋级",
                MenuId = 4,
                TargetView = "LeadingGroupPromotionView",
                ParentId = 0,
                Index = 4,
                MenuIcon = null,
                MenuRole = 0,
            });
            menuEntities.Add(new MenuDto()
            {
                MenuHeader = "市场天梯",
                MenuId = 5,
                TargetView = "MarketLadderView",
                ParentId = 0,
                Index = 5,
                MenuIcon = null,
                MenuRole = 0,
            });
            menuEntities.Add(new MenuDto()
            {
                MenuHeader = "板块轮动",
                MenuId = 6,
                TargetView = "PlateRotationView",
                ParentId = 0,
                Index = 6,
                MenuIcon = null,
                MenuRole = 0,
            });
            menuEntities.Add(new MenuDto()
            {
                MenuHeader = "炸板与涨停板",
                MenuId = 7,
                TargetView = "ExplosiveBoardLImitDown",
                ParentId = 0,
                Index = 7,
                MenuIcon = null,
                MenuRole = 0,
            });
            menuEntities.Add(new MenuDto()
            {
                MenuHeader = "市场情绪",
                MenuId = 8,
                TargetView = "MarketSentimentView",
                ParentId = 0,
                Index = 8,
                MenuIcon = null,
                MenuRole = 0,
            });


            return menuEntities;
        }
    }
}
