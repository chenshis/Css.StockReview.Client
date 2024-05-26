using Prism.Regions;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
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

        public LeadingGroupPromotionViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IReplayService replayService) : base(unityContainer, regionManager)
        {
            this.PageTitle = "龙头晋级";
            this._replayService = replayService;
            InitTableHeader(); //组织头部
        }

        /// <summary>
        /// 初始化表头
        /// </summary>
        private void InitTableHeader()
        {
            var leadingList = this._replayService.GetLeadingGroupPromotion(DateTime.Now);

            LeadingDateHeaderLists.AddRange(leadingList);
          
        }

    }
}
