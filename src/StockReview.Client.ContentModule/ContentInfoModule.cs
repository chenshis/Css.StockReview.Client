using Prism.Ioc;
using Prism.Modularity;
using StockReview.Client.ContentModule.Views;

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
            containerRegistry.RegisterDialog<AddUserDialogView>();
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
