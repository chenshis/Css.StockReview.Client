using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace StockReview.Client.ContentModule.ViewModels
{
    /// <summary>
    /// 看盘视图模型
    /// </summary>
    public class StockOutlookViewModel : NavigationAwareViewModelBase
    {
        public StockOutlookViewModel(IUnityContainer unityContainer,
                                     IRegionManager regionManager)
            : base(unityContainer, regionManager)
        {
            this.PageTitle = "股市看盘";
        }
    }
}
