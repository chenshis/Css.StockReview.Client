﻿using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using StockReview.Domain.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using Unity;

namespace StockReview.Client.ContentModule.ViewModels
{
    public class DragonTigerViewModel : NavigationAwareViewModelBase
    {
        public List<DragonTigerGetInfo> DragonTigerAllGetInfos { get; set; } = new List<DragonTigerGetInfo>();
        public ObservableCollection<DragonTigerGetInfo> DragonTigerGetInfos { get; set; } = new ObservableCollection<DragonTigerGetInfo>();
        public ObservableCollection<DragonTigerGetInfo> DragonTigerGetInfosOne { get; set; } = new ObservableCollection<DragonTigerGetInfo>();
        public ObservableCollection<DragonTigerGetInfo> DragonTigerGetInfosTwo { get; set; } = new ObservableCollection<DragonTigerGetInfo>();
        public ObservableCollection<DragonTigerGetInfo> DragonTigerGetInfosThree { get; set; } = new ObservableCollection<DragonTigerGetInfo>();
        public ObservableCollection<DragonTigerGetInfo> DragonTigerGetInfosFous { get; set; } = new ObservableCollection<DragonTigerGetInfo>();
        public ObservableCollection<SpeculatvieGroupsInfo> SpeculatvieGroups { get; set; } = new ObservableCollection<SpeculatvieGroupsInfo>();
        public DateInfo DateInfo { get; set; } = new DateInfo();

        private readonly IReplayService _replayService;
        private readonly IEventAggregator _eventAggregator;
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
        public ICommand RecoveryCommand => new DelegateCommand<string>((k) =>
        {
            Recovery();
        });
        #endregion
        public DragonTigerViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IReplayService replayService, IEventAggregator eventAggregator) : base(unityContainer, regionManager)
        {
            this.PageTitle = "龙虎榜";
            this._eventAggregator = eventAggregator;
            this._replayService = replayService;
            CurrentDate = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            this.CurrentDate = CurrentDate;

            _eventAggregator.GetEvent<LoadingEvent>().Publish(true);
            InitTableHeader(this.CurrentDate ?? DateTime.Now); 
        }

        private void InitTableHeader(DateTime date)
        {
            Task.Run(() => 
            {
                try
                {
                    var dragonTigerList = this._replayService.GetDragonTiger(date);

                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        this.DragonTigerGetInfos.Clear();
                        this.DragonTigerAllGetInfos.Clear();
                        this.DragonTigerGetInfosOne.Clear();
                        this.DragonTigerGetInfosTwo.Clear();
                        this.DragonTigerGetInfosThree.Clear();
                        this.DragonTigerGetInfosFous.Clear();
                        this.SpeculatvieGroups.Clear();

                        this.DragonTigerGetInfos.AddRange(dragonTigerList.DragonTigerGetInfos);
                        this.DragonTigerAllGetInfos.AddRange(dragonTigerList.DragonTigerGetInfos);
                        this.DragonTigerGetInfosOne.AddRange(dragonTigerList.DragonTigerGetInfosOne);
                        this.DragonTigerGetInfosTwo.AddRange(dragonTigerList.DragonTigerGetInfosTwo);
                        this.DragonTigerGetInfosThree.AddRange(dragonTigerList.DragonTigerGetInfosThree);
                        this.DragonTigerGetInfosFous.AddRange(dragonTigerList.DragonTigerGetInfosFous);
                        this.SpeculatvieGroups.AddRange(dragonTigerList.SpeculatvieGroups);
                      
                    }));
                    this.DateInfo = dragonTigerList.DateInfo;
                }
                catch (Exception)
                {
                }
                // 关闭loading
                _eventAggregator.GetEvent<LoadingEvent>().Publish(false);
            });
        }

        private void Refresh()
        {
            _eventAggregator.GetEvent<LoadingEvent>().Publish(true);
            InitTableHeader(this.CurrentDate ?? DateTime.Now);
        }

        private void Select()
        {
            var speculatvieGroupsInfo = this.SpeculatvieGroups.Where(x => x.IsChecked == true).ToList();
            if (speculatvieGroupsInfo.Count > 0)
            {
                DragonTigerGetInfos.Clear();
                for (int i = 0; i < speculatvieGroupsInfo.Count; i++)
                {
                    var dragInfo = DragonTigerAllGetInfos.Where(x => !string.IsNullOrEmpty(x.DragonSpeculative)
                    && x.DragonSpeculative.Contains(speculatvieGroupsInfo[i].Name)).ToList();

                    DragonTigerGetInfos.AddRange(dragInfo);
                }
            }
        }

        private void Recovery() 
        {
            DragonTigerGetInfos.Clear();
            DragonTigerGetInfos.AddRange(DragonTigerAllGetInfos);
        }
    }
}
