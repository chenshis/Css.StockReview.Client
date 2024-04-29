using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Prism.Ioc;
using Prism.Unity;
using StockReview.Client.ViewModels;
using StockReview.Client.Views;
using StockReview.Infrastructure.Config;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace StockReview.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            // 通过容器创造主界面实例
            return Container.Resolve<MainView>();
        }

        // 初始化Shell（主窗口）的时候执行这个方法
        protected override void InitializeShell(Window shell)
        {
            // 以模态窗口的方式打开这个窗口对象
            if (Container.Resolve<LoginView>().ShowDialog() == false)
            {
                Application.Current.Shutdown();
            }
            else
                base.InitializeShell(shell);
        }


        /// <summary>
        /// 用于注册一些内容
        /// </summary>
        /// <param name="containerRegistry"></param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<RegisterView>(SystemConstant.RegisterView);
            containerRegistry.RegisterDialog<ForgotPasswordView>(SystemConstant.ForgotPasswordView);
            containerRegistry.RegisterDialog<UpdatePasswordView>(SystemConstant.UpdatePasswordView);
            containerRegistry.RegisterForNavigation<MainHeaderView>(SystemConstant.MainHeaderView);
            containerRegistry.RegisterForNavigation<TreeMenuView>(SystemConstant.TreeMenuView);
            // 缓存引入
            var options = Options.Create(new MemoryCacheOptions() { ExpirationScanFrequency = TimeSpan.FromSeconds(30), CompactionPercentage = 0.2 });
            containerRegistry.RegisterSingleton<IMemoryCache>(() => new MemoryCache(options));
        }
    }

}
