using NPOI.SS.Formula.Functions;
using StockReview.Api.Dtos;
using StockReview.Client.ContentModule.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StockReview.Client.ContentModule.Views
{
    /// <summary>
    /// MarketSentimentView.xaml 的交互逻辑
    /// </summary>
    public partial class MarketSentimentView : UserControl
    {

        private MarketSentimentViewModel MarketSentimentViewModels { get; set; }
        public MarketSentimentView(MarketSentimentViewModel marketLadderViewModels)//
        {
            InitializeComponent();


            GenerateDynamicColumns(marketLadderViewModels);
            //MarketSentimentViewModels = marketLadderViewModels;

            //var colTemp = 0;

            //foreach (var item in MarketSentimentViewModels.MarketSentimentDataDtos)
            //{
            //    var rowTemp = 0;

            //    var rows = new RowDefinition();
            //    this.MarketNewsGrid.RowDefinitions.Add(rows);
            //    var columns = new ColumnDefinition();

            //    this.MarketNewsGrid.ColumnDefinitions.Add(columns);

            //    TextBlock textBlock = new TextBlock();
            //    textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            //    //textBlock.Background = new SolidColorBrush(Color.FromRgb(160, 160, 160));
            //    textBlock.FontSize = 12;
            //    textBlock.Foreground = Brushes.White;
            //    textBlock.Text = item.date;

            //    this.MarketNewsGrid.Children.Add(textBlock);

            //    Grid.SetRow(textBlock, rowTemp);
            //    Grid.SetColumn(textBlock, colTemp);

            //    rowTemp++;

            //    foreach (var itemName in item.name)
            //    {
            //        this.MarketNewsGrid.ColumnDefinitions.Add(new ColumnDefinition());

            //        TextBlock textBlockTwo = new TextBlock();
            //        textBlockTwo.HorizontalAlignment = HorizontalAlignment.Left;
            //        textBlockTwo.Background = Brushes.Transparent;
            //        textBlockTwo.FontSize = 12;

            //        textBlockTwo.Foreground = Brushes.White; ;
            //        textBlockTwo.Text = itemName;
            //        this.MarketNewsGrid.Children.Add(textBlockTwo);
            //        Grid.SetRow(textBlockTwo, rowTemp);
            //        Grid.SetColumn(textBlockTwo, colTemp);
            //        rowTemp++;
            //    }
            //    colTemp++;
            //}

            //for (int row = 0; row < MarketNewsGrid.RowDefinitions.Count; row++)
            //{
            //    for (int col = 0; col < MarketNewsGrid.ColumnDefinitions.Count; col++)
            //    {
            //        Border border = new Border
            //        {
            //            BorderBrush = Brushes.White,
            //            BorderThickness = new Thickness(0.5)
            //        };

            //        Grid.SetRow(border, row);
            //        Grid.SetColumn(border, col);

            //        MarketNewsGrid.Children.Add(border);
            //    }
            //}
        }

        private void GenerateDynamicColumns(MarketSentimentViewModel viewModel)
        {

            // 清除现有列
            dataGrid.Columns.Clear();

            // 添加动态列
            foreach (var date in viewModel.Dates)
            {
                dataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = date.ToString("yyyy-MM-dd"),
                    Binding = new Binding($"[{date:yyyy-MM-dd}]") // 使用字符串格式化
                });
            }

            // 绑定数据
            dataGrid.ItemsSource = viewModel.TransformedData;
        }
    }
}
