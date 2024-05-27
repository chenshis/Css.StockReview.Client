using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace StockReview.Client.ContentModule.ViewModels
{
    public class MarketSentimentViewModel : NavigationAwareViewModelBase
    {
        public ObservableCollection<string> DateItem { get; set; } = new ObservableCollection<string>();

        public MarketSentimentViewModel(IUnityContainer unityContainer, IRegionManager regionManager) : base(unityContainer, regionManager)
        {
            this.PageTitle = "市场情绪";

            for (int i = 0; i < 5; i++)
            {
                DateItem.Add(DateTime.Now.AddYears(-i).Year.ToString());
            }
        }
    }
}
