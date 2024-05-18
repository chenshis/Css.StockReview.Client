using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using Prism.Regions;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using StockReview.Infrastructure.Config;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.VisualElements;
using LiveChartsCore.SkiaSharpView.VisualElements;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;

namespace StockReview.Client.ContentModule.ViewModels
{
    /// <summary>
    /// 看盘视图模型
    /// </summary>
    public class StockOutlookViewModel : NavigationAwareViewModelBase
    {
        private readonly IStockOutlookApiService _stockOutlookApiService;
        public StockOutlookViewModel(IUnityContainer unityContainer,
                                     IRegionManager regionManager,
                                     IStockOutlookApiService stockOutlookApiService)
            : base(unityContainer, regionManager)
        {
            this.PageTitle = "股市看盘";

            var data = new FinancialData[]
       {
            new(new DateTime(2021, 1, 1), 523, 500, 450, 400),
            new(new DateTime(2021, 1, 2), 500, 450, 425, 400),
            new(new DateTime(2021, 1, 3), 490, 425, 400, 380),
            new(new DateTime(2021, 1, 4), 420, 400, 420, 380),
            new(new DateTime(2021, 1, 5), 520, 420, 490, 400),
            new(new DateTime(2021, 1, 6), 580, 490, 560, 440),
            new(new DateTime(2021, 1, 7), 570, 560, 350, 340),
            new(new DateTime(2021, 1, 8), 380, 350, 380, 330),
            new(new DateTime(2021, 1, 9), 440, 380, 420, 350),
            new(new DateTime(2021, 1, 10), 490, 420, 460, 400),
            new(new DateTime(2021, 1, 11), 520, 460, 510, 460),
            new(new DateTime(2021, 1, 12), 580, 510, 560, 500),
            new(new DateTime(2021, 1, 13), 600, 560, 540, 510),
            new(new DateTime(2021, 1, 14), 580, 540, 520, 500),
            new(new DateTime(2021, 1, 15), 580, 520, 560, 520),
            new(new DateTime(2021, 1, 16), 590, 560, 580, 520),
            new(new DateTime(2021, 1, 17), 650, 580, 630, 550),
            new(new DateTime(2021, 1, 18), 680, 630, 650, 600),
            new(new DateTime(2021, 1, 19), 670, 650, 600, 570),
            new(new DateTime(2021, 1, 20), 640, 600, 610, 560),
            new(new DateTime(2021, 1, 21), 630, 610, 630, 590)
       };

            CandleSeries = new ISeries[]
            {
                new CandlesticksSeries<FinancialPointI>
            {
                Values = data
                    .Select(x => new FinancialPointI(x.High, x.Open, x.Close, x.Low))
                    .ToArray()
            }
            };

            XAxes = new[]
            {
            new Axis
            {
                LabelsRotation = 15,
                Labels = data
                    .Select(x => x.Date.ToString("yyyy MMM dd"))
                    .ToArray()
            }
        };
            this._stockOutlookApiService = stockOutlookApiService;
            Init();
        }

        public ISeries[] LineSeries { get; set; } =
   {
        new ColumnSeries<double>
        {
            Values = new ObservableCollection<double> { 2, 5, 4, 3 ,7,1,10},
            IsVisible = true
        },
        new ColumnSeries<double>
        {
            Values = new ObservableCollection<double> { 6, 3, 10, 8,5,4,7 },
            IsVisible = true
        }
    };

        public Axis[] XAxes { get; set; }

        public Axis[] YAxes { get; set; } = null;

        public ISeries[] CandleSeries { get; set; }

        public ISeries[] CurveSeries { get; set; } =
    {
        new LineSeries<double>
        {
            Values = new double[] { 5, 0, 5, 0, 5, 0 },
            Fill = null,
            GeometrySize = 0,
            // use the line smoothness property to control the curve
            // it goes from 0 to 1
            // where 0 is a straight line and 1 the most curved
            LineSmoothness = 0.2
        },
        new LineSeries<double>
        {
            Values = new double[] { 7, 2, 7, 2, 7, 2 },
            Fill = null,
            GeometrySize = 0,
            LineSmoothness = 1
        }
    };


        private BulletinBoardDto _bulletinBoard;

        /// <summary>
        /// 看板
        /// </summary>
        public BulletinBoardDto BulletinBoard
        {
            get { return _bulletinBoard; }
            set { SetProperty(ref _bulletinBoard, value); }
        }

        private IEnumerable<ISeries> _emotionSeries;
        /// <summary>
        /// 情绪序列
        /// </summary>
        public IEnumerable<ISeries> EmotionSeries
        {
            get { return _emotionSeries; }
            set { SetProperty(ref _emotionSeries, value); }
        }

        /// <summary>
        /// 看盘初始化
        /// </summary>
        private void Init()
        {
            var apiResponse = _stockOutlookApiService.GetBulletinBoard("2024-05-17");
            if (apiResponse.Code != 0)
            {
                HandyControl.Controls.Growl.Error(new HandyControl.Data.GrowlInfo
                {
                    Message = apiResponse.Msg,
                    Token = SystemConstant.headerGrowl,
                    IsCustom = true,
                    WaitTime = 0
                });
                return;
            }
            BulletinBoard = apiResponse.Data;

            double.TryParse(BulletinBoard.EmotionPercent, out var emotionValue);
            EmotionSeries = GaugeGenerator.BuildSolidGauge(
                new GaugeItem(emotionValue, series =>
                {
                    series.MaxRadialColumnWidth = 15;
                    series.DataLabelsSize = 15;
                }));
        }
    }

    public class FinancialData
    {
        public FinancialData(DateTime date, double high, double open, double close, double low)
        {
            Date = date;
            High = high;
            Open = open;
            Close = close;
            Low = low;
        }

        public DateTime Date { get; set; }
        public double High { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double Low { get; set; }
    }
}
