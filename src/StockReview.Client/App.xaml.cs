using LiveChartsCore;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using StockReview.Api.ApiService;
using StockReview.Api.IApiService;
using StockReview.Client.ContentModule;
using StockReview.Client.ContentModule.Views;
using StockReview.Client.ViewModels;
using StockReview.Client.Views;
using StockReview.Infrastructure.Config;
using StockReview.Infrastructure.Config.HttpClients;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Net.Http;
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
            {
                Container.Resolve<MainViewModel>().LoadRegionManager();
                base.InitializeShell(shell);
            }

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
            containerRegistry.Register<MainHeaderView>(SystemConstant.MainHeaderView);
            containerRegistry.Register<TreeMenuView>(SystemConstant.TreeMenuView);
            containerRegistry.Register<LeadingGroupPromotionView>(SystemConstant.LeadingGroupPromotionView);
            // 缓存引入
            var options = Options.Create(new MemoryCacheOptions() { ExpirationScanFrequency = TimeSpan.FromSeconds(30), CompactionPercentage = 0.2 });
            containerRegistry.RegisterSingleton<IMemoryCache>(() => new MemoryCache(options));
            // IConfiguration 注入
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile(SystemConstant.AppSettings)
                .Build();
            containerRegistry.RegisterSingleton<IConfiguration>(() => configuration);
            // service collection 集合转换
            if (containerRegistry is IContainerExtension container)
            {
                // 注入 httpClient
                container.CreateServiceProvider((services) => services.AddHttpClient());
            }
            // 注册业务逻辑
            containerRegistry.RegisterScoped<ILoginApiService, LoginApiService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ContentInfoModule>();
            base.ConfigureModuleCatalog(moduleCatalog);
        }
    }

}
