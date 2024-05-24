using StockReview.Api.Dtos;
using StockReview.Client.ContentModule.ViewModels;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StockReview.Client.ContentModule.Views
{
    /// <summary>
    /// MarketLadderView.xaml 的交互逻辑
    /// </summary>
    public partial class MarketLadderView : UserControl
    {

        private MarketLadderViewModel MarketLadderViewModel { get; set; }

        public MarketLadderView(MarketLadderViewModel marketLadderViewModel)
        {
            InitializeComponent();
            MarketLadderViewModel = marketLadderViewModel;
            InitMarketLadderNewsView();
        }

        private void InitMarketLadderNewsView()
        {
            var marketNewsList = MarketLadderViewModel.MarketLadderNewsLists.ToList();

            var newsNumber = 0;

            for (int i = 0; i < marketNewsList.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    RowDefinition newRow = new RowDefinition();
                    this.MarketNewsGrid.RowDefinitions.Add(newRow);

                    System.Windows.Controls.Label label = new System.Windows.Controls.Label();
                    label.HorizontalAlignment = HorizontalAlignment.Left;
                    label.Background = Brushes.Transparent;
                    label.BorderBrush = Brushes.Transparent;
                    label.FontSize = 12;
                    label.Foreground = j == 0 ? new SolidColorBrush(Color.FromRgb(240, 102, 50)) : Brushes.Black;

                    label.Content = j == 0 ? marketNewsList[i].MarketNewsTitle : marketNewsList[i].MarketNewsType;
                    this.MarketNewsGrid.Children.Add(label);

                    newsNumber += i == 0 &&j==0? i : 1;
                    Grid.SetRow(label, newsNumber); // 设置在Grid的第一行
                    Grid.SetColumn(label, 0); // 设置在Grid的第一列
                }
            }


            var marketList = MarketLadderViewModel.MarketLadderLists.ToList();

            var rowTemp = 0;

            for (int i = 0; i < marketList.Count; i++)
            {
                rowTemp++;

                RowDefinition newRow = new RowDefinition();
                this.MarketGrid.RowDefinitions.Add(newRow);

                ColumnDefinition newColum = new ColumnDefinition();
                this.MarketGrid.ColumnDefinitions.Add(newColum);
                ColumnDefinition newColumTwo = new ColumnDefinition();
                this.MarketGrid.ColumnDefinitions.Add(newColumTwo);
                ColumnDefinition newColumThree = new ColumnDefinition();
                this.MarketGrid.ColumnDefinitions.Add(newColumThree);

                System.Windows.Controls.Label label = new System.Windows.Controls.Label();
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.Background = Brushes.Transparent;
                label.BorderBrush = Brushes.Transparent;
                label.FontSize = 12;
                label.Foreground = new SolidColorBrush(Color.FromRgb(240, 102, 50));
                label.Content = marketList[i].MarketLadderBoard;
                this.MarketGrid.Children.Add(label);
                Grid.SetRow(label, rowTemp);
                Grid.SetColumn(label, 0);

                System.Windows.Controls.Label labelTwo = new System.Windows.Controls.Label();
                labelTwo.HorizontalAlignment = HorizontalAlignment.Left;
                labelTwo.Background = Brushes.Transparent;
                labelTwo.BorderBrush = Brushes.Transparent;
                labelTwo.FontSize = 12;
                labelTwo.Foreground = new SolidColorBrush(Color.FromRgb(240, 102, 50));
                labelTwo.Content = marketList[i].MarketLadderNumber;
                this.MarketGrid.Children.Add(labelTwo);
                Grid.SetRow(labelTwo, rowTemp);
                Grid.SetColumn(labelTwo, 1);

                System.Windows.Controls.Label labelThree = new System.Windows.Controls.Label();
                labelThree.HorizontalAlignment = HorizontalAlignment.Left;
                labelThree.Background = Brushes.Transparent;
                labelThree.BorderBrush = Brushes.Transparent;
                labelThree.FontSize = 12;
                labelThree.Foreground = new SolidColorBrush(Color.FromRgb(240, 102, 50));
                labelThree.Content = marketList[i].MarketLadderDescibe;
                this.MarketGrid.Children.Add(labelThree);
                Grid.SetRow(labelThree, rowTemp);
                Grid.SetColumn(labelThree, 2);

                if (marketList[i].MarketLadderInfos.Count()>0)
                {
                    rowTemp++;

                    RowDefinition newRowTwo = new RowDefinition();
                    this.MarketGrid.RowDefinitions.Add(newRowTwo);

                    DataGrid dataGrid = new DataGrid();
                    dataGrid.AutoGenerateColumns = false; // 自动生成列

                    DataGridTextColumn marketLadderCodeColumn = new DataGridTextColumn();
                    marketLadderCodeColumn.Header = "代码";
                    marketLadderCodeColumn.Binding = new System.Windows.Data.Binding("MarketLadderCode");

                    DataGridTextColumn marketLadderNameColumn = new DataGridTextColumn();
                    marketLadderNameColumn.Header = "股票名称";
                    marketLadderNameColumn.Binding = new System.Windows.Data.Binding("MarketLadderName");

                    DataGridTextColumn marketLadderFirstLimitUpColumn = new DataGridTextColumn();
                    marketLadderFirstLimitUpColumn.Header = "首次涨停";
                    marketLadderFirstLimitUpColumn.Binding = new System.Windows.Data.Binding("MarketLadderFirstLimitUp");

                    DataGridTextColumn marketLadderReasonLimitUpColumn = new DataGridTextColumn();
                    marketLadderReasonLimitUpColumn.Header = "涨停原因";
                    marketLadderReasonLimitUpColumn.Binding = new System.Windows.Data.Binding("MarketLadderReasonLimitUp");

                    // 添加列到DataGrid的Columns集合中
                    dataGrid.Columns.Add(marketLadderCodeColumn);
                    dataGrid.Columns.Add(marketLadderNameColumn);
                    dataGrid.Columns.Add(marketLadderFirstLimitUpColumn);
                    dataGrid.Columns.Add(marketLadderReasonLimitUpColumn);

                    MarketLadderInfo[] data = marketList[i].MarketLadderInfos.ToArray();
                   
                    dataGrid.ItemsSource = data;
                    this.MarketGrid.Children.Add(dataGrid);

                    Grid.SetRow(dataGrid, rowTemp); 
                    Grid.SetColumn(dataGrid, 0); 
                    Grid.SetColumnSpan(dataGrid, 3);
                }

            }

        }
    }
}
