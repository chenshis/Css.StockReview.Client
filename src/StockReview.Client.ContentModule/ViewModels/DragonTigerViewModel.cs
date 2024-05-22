﻿using Prism.Commands;
using Prism.Regions;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;

namespace StockReview.Client.ContentModule.ViewModels
{
    public class DragonTigerViewModel : NavigationAwareViewModelBase
    {
        public ObservableCollection<DragonTigerGetInfo> DragonTigerGetInfos { get; set; } = new ObservableCollection<DragonTigerGetInfo>();
        public ObservableCollection<DragonTigerGetInfo> DragonTigerGetInfosOne { get; set; } = new ObservableCollection<DragonTigerGetInfo>();
        public ObservableCollection<DragonTigerGetInfo> DragonTigerGetInfosTwo { get; set; } = new ObservableCollection<DragonTigerGetInfo>();
        public ObservableCollection<DragonTigerGetInfo> DragonTigerGetInfosThree { get; set; } = new ObservableCollection<DragonTigerGetInfo>();
        public ObservableCollection<DragonTigerGetInfo> DragonTigerGetInfosFous { get; set; } = new ObservableCollection<DragonTigerGetInfo>();
        public ObservableCollection<SpeculatvieGroupsInfo> SpeculatvieGroups { get; set; } = new ObservableCollection<SpeculatvieGroupsInfo>();
        public DateInfo DateInfo { get; set; } = new DateInfo();
        
        private readonly IReplayService _replayService;

        private DateTime? _currentDate;
        /// <summary>
        /// 选中日期
        /// </summary>
        public DateTime? CurrentDate
        {
            get { return _currentDate; }
            set { SetProperty(ref _currentDate, value); }
        }

        #region 命令
        public ICommand RefreshCommand => new DelegateCommand<string>((k) =>
        {
            Refresh();
        });
        public ICommand SelectCommand => new DelegateCommand<string>((k) => 
        {
            Select();
        });
        #endregion
        public DragonTigerViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IReplayService replayService) : base(unityContainer, regionManager)
        {
            this.PageTitle = "龙虎榜";
            this._replayService = replayService;
            CurrentDate = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            this.CurrentDate = CurrentDate;
            InitTableHeader(this.CurrentDate ?? DateTime.Now); //组织头部
        }

        private void InitTableHeader(DateTime date)
        {
            var dragonTigerList = this._replayService.GetDragonTiger(date);

            this.DragonTigerGetInfos.AddRange(dragonTigerList.DragonTigerGetInfos);
            this.DragonTigerGetInfosOne.AddRange(dragonTigerList.DragonTigerGetInfosOne);
            this.DragonTigerGetInfosTwo.AddRange(dragonTigerList.DragonTigerGetInfosTwo);
            this.DragonTigerGetInfosThree.AddRange(dragonTigerList.DragonTigerGetInfosThree);
            this.DragonTigerGetInfosFous.AddRange(dragonTigerList.DragonTigerGetInfosFous);
            this.SpeculatvieGroups.AddRange(dragonTigerList.SpeculatvieGroups);
            this.DateInfo = dragonTigerList.DateInfo;
        }

        public void Refresh()
        {
            InitTableHeader(this.CurrentDate ?? DateTime.Now);
        }

        public void Select() 
        {
            var speculatvieGroupsInfo = this.SpeculatvieGroups.Where(x => x.IsChecked = true).ToList();
        }
    }
}
