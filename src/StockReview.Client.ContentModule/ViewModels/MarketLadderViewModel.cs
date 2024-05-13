using Prism.Regions;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace StockReview.Client.ContentModule.ViewModels
{
    public class MarketLadderViewModel : NavigationAwareViewModelBase
    {
        public string MarketTitle { get; set; }

        public ObservableCollection<MarketLadderNewsList> MarketLadderNewsLists { get; set; } = new ObservableCollection<MarketLadderNewsList>();
        public ObservableCollection<MarketLadderList> MarketLadderLists { get; set; } = new ObservableCollection<MarketLadderList>();


        private readonly IReplayService _replayService;

        public MarketLadderViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IReplayService replayService) : base(unityContainer, regionManager)
        {
            this.PageTitle = "市场天梯";
            this._replayService = replayService;
            InitTableHeader();
        }

        private void InitTableHeader()
        {
            var markList = this._replayService.GetMarketLadder(DateTime.Now);

            if (markList!=null)
            {
                this.MarketTitle = markList.MarketTitle;
            }

            foreach (var item in markList.MarketLadderLists)
            {
                MarketLadderLists.Add(item);
            }

            foreach (var item in markList.MarketLadderNewsLists)
            {
                MarketLadderNewsLists.Add(item);
            }

        }
    }

}
