using Prism.Events;
using StockReview.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StockReview.Client.Views
{
    /// <summary>
    /// TestView.xaml 的交互逻辑
    /// </summary>
    public partial class TestView : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ea"></param>
        public TestView(IEventAggregator ea)
        {
            InitializeComponent();

            // 注册 事件聚合
            ea.GetEvent<MessageEvent>().Subscribe(() =>
            {

            });

            // 触发
            ea.GetEvent<MessageEvent>().Publish();

        }
    }
}
