using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using StockReview.Client.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StockReview.Client.ViewModels
{
    public class MenuViewModel
    {
        private readonly IRegionManager _regionManager;

        public MenuViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
        public ICommand OpenCommand
        {
            get => new DelegateCommand<object>(obj =>
            {
                // 执行加载页面操作
                // 页面对象需要在ContentRegion里面显示

                //var region = _regionManager.Regions["ContentRegion"];
                //region.Add
                //_regionManager.RegisterViewWithRegion("ContentRegion", obj.ToString());
                //_containerRegistry.RegisterForNavigation<ContentView>();

                string param = "123";
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("xxx", param);
                _regionManager.RequestNavigate("TabContentRegion", obj.ToString(), parameters);
            });
        }
    }
}
