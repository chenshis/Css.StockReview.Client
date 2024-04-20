using Prism.Ioc;
using Prism.Unity;
using StockReview.Client.Views;
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
            return Container.Resolve<LoginView>();
        }

        // 初始化Shell（主窗口）的时候执行这个方法
        protected override void InitializeShell(Window shell)
        {
            if (shell == null || shell.ShowDialog() != true)// 以模态窗口的方式打开这个窗口对象
            {
                // 不需要App的属性ShutdownMode配合
                Application.Current.Shutdown();
                // 如果没有强行退出的话，打开主窗口
            }
        }


        /// <summary>
        /// 用于注册一些内容
        /// </summary>
        /// <param name="containerRegistry"></param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<TestView>();
            containerRegistry.RegisterForNavigation<MenuView>();
            containerRegistry.RegisterForNavigation<ContentView>();
        }

    }

}
