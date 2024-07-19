using HandyControl.Tools.Extension;
using Microsoft.Win32;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using StockReview.Client.ContentModule.Views;
using StockReview.Domain.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Unity;
using static System.Net.Mime.MediaTypeNames;

namespace StockReview.Client.ContentModule.ViewModels
{
    public class MarketLadderViewModel : NavigationAwareViewModelBase
    {
      
        public ObservableCollection<MarketLadderNewsList> MarketLadderNewsLists { get; set; } = new ObservableCollection<MarketLadderNewsList>();
        public ObservableCollection<MarketLadderList> MarketLadderLists { get; set; } = new ObservableCollection<MarketLadderList>();

        public MarketLadderToInfo MarketLadderInfosOne { get; set; } = new MarketLadderToInfo();
        public ObservableCollection<MarketLadderInfo> MarketLadderListOne { get; set; } = new ObservableCollection<MarketLadderInfo>();

        public MarketLadderToInfo MarketLadderInfosTwo { get; set; } = new MarketLadderToInfo();
        public ObservableCollection<MarketLadderInfo> MarketLadderListTwo { get; set; } = new ObservableCollection<MarketLadderInfo>();

        public MarketLadderToInfo MarketLadderInfosThree { get; set; } = new MarketLadderToInfo();
        public ObservableCollection<MarketLadderInfo> MarketLadderListThree { get; set; } = new ObservableCollection<MarketLadderInfo>();

        public MarketLadderToInfo MarketLadderInfosFours { get; set; } = new MarketLadderToInfo();
        public ObservableCollection<MarketLadderInfo> MarketLadderListFours { get; set; } = new ObservableCollection<MarketLadderInfo>();

        private DateTime? _currentDate;
        /// <summary>
        /// 选中日期
        /// </summary>
        public DateTime? CurrentDate
        {
            get { return _currentDate; }
            set { SetProperty(ref _currentDate, value); }
        }

        private string _marketTitle;
        /// <summary>
        /// 错误消息
        /// </summary>
        public string MarketTitle
        {
            get { return _marketTitle; }
            set { SetProperty(ref _marketTitle, value); }
        }


        /// <summary>
        /// 连板
        /// </summary>
        private string _marketLadderBoardOne;
       
        public string MarketLadderBoardOne
        {
            get { return _marketLadderBoardOne; }
            set { SetProperty(ref _marketLadderBoardOne, value); }
        }


        /// <summary>
        /// 数量
        /// </summary>
        private string _marketLadderNumberOne;

        public string MarketLadderNumberOne
        {
            get { return _marketLadderNumberOne; }
            set { SetProperty(ref _marketLadderNumberOne, value); }
        }

        /// <summary>
        /// 描述
        /// </summary>
        private string _marketLadderDescibeOne;

        public string MarketLadderDescibeOne
        {
            get { return _marketLadderDescibeOne; }
            set { SetProperty(ref _marketLadderDescibeOne, value); }
        }

        /// <summary>
        /// 连板
        /// </summary>
        private string _marketLadderBoardTwo;

        public string MarketLadderBoardTwo
        {
            get { return _marketLadderBoardTwo; }
            set { SetProperty(ref _marketLadderBoardTwo, value); }
        }


        /// <summary>
        /// 数量
        /// </summary>
        private string _marketLadderNumberTwo;

        public string MarketLadderNumberTwo
        {
            get { return _marketLadderNumberTwo; }
            set { SetProperty(ref _marketLadderNumberTwo, value); }
        }

        /// <summary>
        /// 描述
        /// </summary>
        private string _marketLadderDescibeTwo;

        public string MarketLadderDescibeTwo
        {
            get { return _marketLadderDescibeTwo; }
            set { SetProperty(ref _marketLadderDescibeTwo, value); }
        }

        /// <summary>
        /// 连板
        /// </summary>
        private string _marketLadderBoardThree;

        public string MarketLadderBoardThree
        {
            get { return _marketLadderBoardThree; }
            set { SetProperty(ref _marketLadderBoardThree, value); }
        }


        /// <summary>
        /// 数量
        /// </summary>
        private string _marketLadderNumberThree;

        public string MarketLadderNumberThree
        {
            get { return _marketLadderNumberThree; }
            set { SetProperty(ref _marketLadderNumberThree, value); }
        }

        /// <summary>
        /// 描述
        /// </summary>
        private string _marketLadderDescibeThree;

        public string MarketLadderDescibeThree
        {
            get { return _marketLadderDescibeThree; }
            set { SetProperty(ref _marketLadderDescibeThree, value); }
        }

        /// <summary>
        /// 连板
        /// </summary>
        private string _marketLadderBoardFours;

        public string MarketLadderBoardFours
        {
            get { return _marketLadderBoardFours; }
            set { SetProperty(ref _marketLadderBoardFours, value); }
        }


        /// <summary>
        /// 数量
        /// </summary>
        private string _marketLadderNumberFours;

        public string MarketLadderNumberFours
        {
            get { return _marketLadderNumberFours; }
            set { SetProperty(ref _marketLadderNumberFours, value); }
        }

        /// <summary>
        /// 描述
        /// </summary>
        private string _marketLadderDescibeFours;

        public string MarketLadderDescibeFours
        {
            get { return _marketLadderDescibeFours; }
            set { SetProperty(ref _marketLadderDescibeFours, value); }
        }
        private readonly IReplayService _replayService;
        private readonly IEventAggregator _eventAggregator;

        private  MarketLadderView MarketLadderView;

        #region 命令
        public ICommand RefreshCommand => new DelegateCommand<string>((k) =>
        {
            Refresh();
        });
        public ICommand ExportCommand => new DelegateCommand<string>((k) => 
        {
            Export();
        });
        #endregion
        public MarketLadderViewModel(IUnityContainer unityContainer, IRegionManager regionManager
            , IReplayService replayService, IEventAggregator eventAggregator) : base(unityContainer, regionManager)
        {
            this.PageTitle = "市场天梯";
            this._eventAggregator = eventAggregator;
            this._replayService = replayService;
            CurrentDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            this.CurrentDate = CurrentDate;
            _eventAggregator.GetEvent<LoadingEvent>().Publish(true);
            InitTableHeader(this.CurrentDate ?? DateTime.Now); //组织头部
        }

        private void InitTableHeader(DateTime date)
        {
            Task.Run(() =>
            {
                var markList = this._replayService.GetMarketLadder(date);

                if (markList != null)
                {
                    this.MarketTitle = markList.MarketTitle;
                }

                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    this.MarketLadderLists.Clear();
                    this.MarketLadderNewsLists.Clear();

                    for (int i = 0; i < markList.MarketLadderLists.Count; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                MarketLadderBoardOne = markList.MarketLadderLists[i].MarketLadderBoard;
                                MarketLadderDescibeOne = markList.MarketLadderLists[i].MarketLadderDescibe;
                                MarketLadderNumberOne = markList.MarketLadderLists[i].MarketLadderNumber;
                                //MarketLadderInfosOne = new MarketLadderToInfo
                                //{
                                //    MarketLadderBoard = ,
                                //    MarketLadderDescibe = ,
                                //    MarketLadderNumber = 
                                //};
                                MarketLadderListOne.AddRange(markList.MarketLadderLists[i].MarketLadderInfos);
                                break;
                            case 1:
                                MarketLadderBoardTwo= markList.MarketLadderLists[i].MarketLadderBoard;
                                MarketLadderDescibeTwo = markList.MarketLadderLists[i].MarketLadderDescibe;
                                MarketLadderNumberTwo = markList.MarketLadderLists[i].MarketLadderNumber;
                                //MarketLadderInfosTwo = new MarketLadderToInfo
                                //{
                                //    MarketLadderBoard = markList.MarketLadderLists[i].MarketLadderBoard,
                                //    MarketLadderDescibe = markList.MarketLadderLists[i].MarketLadderDescibe,
                                //    MarketLadderNumber = markList.MarketLadderLists[i].MarketLadderNumber
                                //};
                                MarketLadderListTwo.AddRange(markList.MarketLadderLists[i].MarketLadderInfos);
                                break;
                            case 2:
                                MarketLadderBoardThree = markList.MarketLadderLists[i].MarketLadderBoard;
                                MarketLadderDescibeThree = markList.MarketLadderLists[i].MarketLadderDescibe;
                                MarketLadderNumberThree = markList.MarketLadderLists[i].MarketLadderNumber;
                                //MarketLadderInfosThree = new MarketLadderToInfo
                                //{
                                //    MarketLadderBoard = markList.MarketLadderLists[i].MarketLadderBoard,
                                //    MarketLadderDescibe = markList.MarketLadderLists[i].MarketLadderDescibe,
                                //    MarketLadderNumber = markList.MarketLadderLists[i].MarketLadderNumber
                                //};
                                MarketLadderListThree.AddRange(markList.MarketLadderLists[i].MarketLadderInfos);
                                break;
                            case 3:
                                MarketLadderBoardFours = markList.MarketLadderLists[i].MarketLadderBoard;
                                MarketLadderDescibeFours = markList.MarketLadderLists[i].MarketLadderDescibe;
                                MarketLadderNumberFours = markList.MarketLadderLists[i].MarketLadderNumber;
                                //MarketLadderInfosFours = new MarketLadderToInfo
                                //{
                                //    MarketLadderBoard = markList.MarketLadderLists[i].MarketLadderBoard,
                                //    MarketLadderDescibe = markList.MarketLadderLists[i].MarketLadderDescibe,
                                //    MarketLadderNumber = markList.MarketLadderLists[i].MarketLadderNumber
                                //};
                                MarketLadderListFours.AddRange(markList.MarketLadderLists[i].MarketLadderInfos);
                                break;
                            default:
                                break;
                        }
                    }

                    for (int i = 0; i < markList.MarketLadderNewsLists.Count; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            MarketLadderNewsLists.Add(new MarketLadderNewsList
                            {
                                MarketNewsTitle = j == 0 ? markList.MarketLadderNewsLists[i].MarketNewsTitle : markList.MarketLadderNewsLists[i].MarketNewsType,
                                MarketColor = j == 0 ? "#F06632" : "Black"
                            });
                        }
                    }

                }));

                _eventAggregator.GetEvent<LoadingEvent>().Publish(false);
            });
        }

        private void Refresh() 
        {
            _eventAggregator.GetEvent<LoadingEvent>().Publish(true);
            InitTableHeader(this.CurrentDate ?? DateTime.Now);
        }
        private void Export() 
        {
            _eventAggregator.GetEvent<LoadingEvent>().Publish(true);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "data\\";
            saveFileDialog.Title = "导出到Excel";
            saveFileDialog.DefaultExt = "xlsx";
            saveFileDialog.FileName = "TT" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            saveFileDialog.Filter = "Excel文件 (*.xlsx)|*.xlsx|Excel2003(*.xls)|*.xls";
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == true)
            {
                ExportListToExcel(saveFileDialog.FileName);
            }
            _eventAggregator.GetEvent<LoadingEvent>().Publish(false);
        }

        public  void ExportListToExcel( string filePath)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("市场天梯");

            int rowIndex = 0;

            var data = this.MarketLadderLists.ToList();

            foreach (var item in data)
            {
                IRow row = sheet.CreateRow(rowIndex++);
                row.CreateCell(0).SetCellValue(item.MarketLadderBoard);
                row.CreateCell(1).SetCellValue(item.MarketLadderNumber);
                row.CreateCell(2).SetCellValue(item.MarketLadderDescibe);
                // 输出嵌套列表信息
                if (item.MarketLadderInfos != null)
                {
                    IRow titleRow = sheet.CreateRow(rowIndex++);
                    titleRow.CreateCell(0).SetCellValue("代码");
                    titleRow.CreateCell(1).SetCellValue("股票名称");
                    titleRow.CreateCell(2).SetCellValue("首次涨停");
                    titleRow.CreateCell(3).SetCellValue("涨停原因");
                    foreach (var info in item.MarketLadderInfos)
                    {
                        IRow infoRow = sheet.CreateRow(rowIndex++);
                        infoRow.CreateCell(0).SetCellValue(info.MarketLadderCode);
                        infoRow.CreateCell(1).SetCellValue(info.MarketLadderName);
                        infoRow.CreateCell(2).SetCellValue(info.MarketLadderFirstLimitUp);
                        infoRow.CreateCell(3).SetCellValue(info.MarketLadderReasonLimitUp);
                    }
                }
            }

            using (var file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(file);
            }

            if (MessageBox.Show("导出Excel文件完毕！文件在：\r\n" + filePath, "简单复盘导出Excel", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
                var argument = "/select, \"" + filePath + "\"";
                Process.Start("explorer.exe", argument);
            }

        }
    }

}
