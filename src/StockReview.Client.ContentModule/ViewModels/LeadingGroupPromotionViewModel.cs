using Prism.Events;
using Prism.Regions;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using StockReview.Domain.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace StockReview.Client.ContentModule.ViewModels
{
    public class LeadingGroupPromotionViewModel : NavigationAwareViewModelBase
    {
        public ObservableCollection<LeadingDateHeaderDto> LeadingDateHeaderLists { get; set; } = new ObservableCollection<LeadingDateHeaderDto>();

        private readonly IReplayService _replayService;
        private readonly IEventAggregator _eventAggregator;

        public LeadingGroupPromotionViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IReplayService replayService, IEventAggregator eventAggregator) : base(unityContainer, regionManager)
        {
            this.PageTitle = "龙头晋级";
            this._eventAggregator = eventAggregator;
            this._replayService = replayService;
            _eventAggregator.GetEvent<LoadingEvent>().Publish(true);
            InitTableHeader(); //组织头部
        }

        /// <summary>
        /// 初始化表头
        /// </summary>
        private void InitTableHeader()
        {
            Task.Run(() =>
            { 
                var leadingList = this._replayService.GetLeadingGroupPromotion(DateTime.Now);

                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    LeadingDateHeaderLists.AddRange(leadingList);
                }));
                _eventAggregator.GetEvent<LoadingEvent>().Publish(false);
            });

          
        }

    }
}
