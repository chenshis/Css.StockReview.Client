using Prism.Mvvm;
using Prism.Regions;
using StockReview.Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Client.ViewModels
{
    /// <summary>
    /// 主体视图模型
    /// </summary>
    public class MainViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public MainViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        /// <summary>
        /// 初始化区域管理
        /// </summary>
        public void LoadRegionManager()
        {
            // 头部
            _regionManager.RegisterViewWithRegion(SystemConstant.MainHeaderRegion, SystemConstant.MainHeaderView);
            _regionManager.RegisterViewWithRegion(SystemConstant.TreeMenuViewRegion, SystemConstant.TreeMenuView);
        }
    }
}
