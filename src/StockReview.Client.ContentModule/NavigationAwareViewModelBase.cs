using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using StockReview.Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace StockReview.Client.ContentModule
{
    public abstract class NavigationAwareViewModelBase : BindableBase, INavigationAware
    {
        private readonly IUnityContainer _unityContainer;
        private readonly IRegionManager _regionManager;


        private string _pageTitle;
        /// <summary>
        /// 切换页标题
        /// </summary>
        public string PageTitle
        {
            get { return _pageTitle; }
            set { SetProperty(ref _pageTitle, value); }
        }

        /// <summary>
        /// 能否关闭
        /// </summary>
        public bool IsCanClose { get; set; } = true;

        private string _navigationUri;
        /// <summary>
        /// 导航地址
        /// </summary>
        public string NavigationUri
        {
            get { return _navigationUri; }
            set { SetProperty(ref _navigationUri, value); }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            NavigationUri = navigationContext.Uri.ToString();
        }
        public NavigationAwareViewModelBase(IUnityContainer unityContainer, IRegionManager regionManager)
        {
            this._unityContainer = unityContainer;
            this._regionManager = regionManager;
        }

        public DelegateCommand CloseCommand
        {
            get => new DelegateCommand(() =>
            {
                var naviRegion = _unityContainer.Registrations.FirstOrDefault(v => v.Name == NavigationUri);
                var name = naviRegion.MappedToType.Name;
                // 根据对象名称再从Region的Views里面找到对象
                if (!string.IsNullOrEmpty(name))
                {
                    var region = _regionManager.Regions[SystemConstant.MainContentRegion];
                    var view = region.Views.FirstOrDefault(v => v.GetType().Name == name);
                    // 把这个对象从Region的Views里移除
                    if (view != null)
                        region.Remove(view);
                }
            });
        }

        /// <summary>
        /// 刷新命令
        /// </summary>
        public DelegateCommand RefreshCommand => new DelegateCommand(Refresh);
        public virtual void Refresh() { }
    }
}

