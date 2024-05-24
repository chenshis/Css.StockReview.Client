using Microsoft.Win32;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
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
using Unity;
using static System.Net.Mime.MediaTypeNames;

namespace StockReview.Client.ContentModule.ViewModels
{
    public class MarketLadderViewModel : NavigationAwareViewModelBase
    {
        public string MarketTitle { get; set; }

        public ObservableCollection<MarketLadderNewsList> MarketLadderNewsLists { get; set; } = new ObservableCollection<MarketLadderNewsList>();
        public ObservableCollection<MarketLadderList> MarketLadderLists { get; set; } = new ObservableCollection<MarketLadderList>();

        private DateTime? _currentDate;
        /// <summary>
        /// 选中日期
        /// </summary>
        public DateTime? CurrentDate
        {
            get { return _currentDate; }
            set { SetProperty(ref _currentDate, value); }
        }

        private readonly IReplayService _replayService;

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
        public MarketLadderViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IReplayService replayService) : base(unityContainer, regionManager)
        {
            this.PageTitle = "市场天梯";
            this._replayService = replayService;
            CurrentDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            this.CurrentDate = CurrentDate;
            InitTableHeader(this.CurrentDate ?? DateTime.Now); //组织头部
        }

        private void InitTableHeader(DateTime date)
        {
            var markList = this._replayService.GetMarketLadder(date);

            if (markList!=null)
            {
                this.MarketTitle = markList.MarketTitle;
            }
            MarketLadderLists.AddRange(markList.MarketLadderLists);
            MarketLadderNewsLists.AddRange(markList.MarketLadderNewsLists);
        }
        
        private void Refresh() 
        {
            InitTableHeader(this.CurrentDate ?? DateTime.Now);
        }
        private void Export() 
        {
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
