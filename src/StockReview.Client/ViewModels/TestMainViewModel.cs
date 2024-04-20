using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Client.ViewModels
{
    public class TestMainViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public TestMainViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            // 加载菜单区域
            // RegionManger
            _regionManager.RegisterViewWithRegion("MenuRegion","MenuView");
        }
    }
}
