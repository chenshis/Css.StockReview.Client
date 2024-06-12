using Microsoft.Extensions.Caching.Memory;
using Prism.Mvvm;
using StockReview.Api.Dtos;
using StockReview.Infrastructure.Config;

namespace StockReview.Client.ViewModels
{
    public class MainHeaderViewModel : BindableBase
    {
        public MainHeaderViewModel(IMemoryCache memoryCache)
        {
            var user = memoryCache.Get<UserDto>(SystemConstant.GlobalUserInfo);
            if (user != null)
            {
                UserName = user.UserName;
            }
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
