using Microsoft.Extensions.Caching.Memory;
using Prism.Mvvm;
using StockReview.Infrastructure.Config;

namespace StockReview.Client.ViewModels
{
    public class MainHeaderViewModel : BindableBase
    {
        public MainHeaderViewModel(IMemoryCache memoryCache)
        {
            UserName = memoryCache.Get<string>(SystemConstant.GlobalUserName);
        }

        private string _userName;
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

    }
}
