using Prism.Ioc;
using Prism.Modularity;
using StockReview.Client.ContentModule.Views;
using StockReview.Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Client.ContentModule
{
    public class ContentInfoModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<UserManagementView>();
            containerRegistry.RegisterDialog<ModifyUserDialogView>();
            containerRegistry.RegisterForNavigation<StockOutlookView>();
            containerRegistry.RegisterForNavigation<DragonTigerView>();

            containerRegistry.RegisterForNavigation<LeadingGroupPromotionView>();
            containerRegistry.RegisterForNavigation<MarketLadderView>();
            containerRegistry.RegisterForNavigation<PlateRotationView>();
            containerRegistry.RegisterForNavigation<ExplosiveBoardLImitDownView>();
            containerRegistry.RegisterForNavigation<MarketSentimentView>();
        }
    }
}
