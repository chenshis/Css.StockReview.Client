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
            Refresh();
        }

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
        /// 视觉元素
        /// </summary>
        public IEnumerable<VisualElement<SkiaSharpDrawingContext>> VisualElements { get; set; }

        /// <summary>
        /// 指针指向
        /// </summary>
        public NeedleVisual Needle { get; set; }

        /// <summary>
        /// 柱状图序列
        /// </summary>
        public ISeries[] HistogramSeries { get; set; }
        /// <summary>
        /// X坐标轴显示内容
        /// </summary>
        public Axis[] HistogramXAxes { get; set; }

        /// <summary>
        /// 刷新
        /// </summary>
        public override void Refresh()
        {
            var day = "2024-05-20";
            var apiResponse = _stockOutlookApiService.GetBulletinBoard(day);
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
            Needle = new NeedleVisual { Value = emotionValue };
            EmotionSeries = GaugeGenerator.BuildAngularGaugeSections(
                new GaugeItem(50, s => SetStyle(s, SKColors.Green)),
                new GaugeItem(50, s => SetStyle(s, SKColors.Red)));
            VisualElements = new VisualElement<SkiaSharpDrawingContext>[]
            {
                new AngularTicksVisual
                {
                    LabelsSize = 10,
                    LabelsOuterOffset = 5,
                    OuterOffset = 30,
                    TicksLength = 5
                },
                Needle
            };

            //情绪明细
            var apiEmotionDetail = _stockOutlookApiService.GetEmotionDetail(day);
            if (apiEmotionDetail.Code != 0)
            {
                HandyControl.Controls.Growl.Error(new HandyControl.Data.GrowlInfo
                {
                    Message = apiEmotionDetail.Msg,
                    Token = SystemConstant.headerGrowl,
                    IsCustom = true,
                    WaitTime = 0
                });
                return;
            }
            var emotionDetail = apiEmotionDetail.Data;

            HistogramXAxes = new Axis[1];
            HistogramXAxes[0] = new Axis();
            HistogramXAxes[0].Labels = new List<string>();
            HistogramXAxes[0].TextSize = 8;
            HistogramSeries = new ISeries[3];
            // 正
            var positiveSeries = new ColumnSeries<ObservablePoint>();
            positiveSeries.Fill = new SolidColorPaint(SKColors.Red);
            var positiveSeriesValues = new List<ObservablePoint>();
            // 零
            var zeroSeries = new ColumnSeries<ObservablePoint>();
            zeroSeries.Fill = new SolidColorPaint(SKColors.Gray);
            var zeroSeriesValues = new List<ObservablePoint>();
            // 负
            var negativeSeries = new ColumnSeries<ObservablePoint>();
            negativeSeries.Fill = new SolidColorPaint(SKColors.Green);
            var negativeSeriesValues = new List<ObservablePoint>();

            for (int i = 0; i < emotionDetail.histogram.Count; i++)
            {
                double.TryParse(emotionDetail.histogram[i].yvalue, out var yValue);
                if (i < 5)
                {
                    positiveSeriesValues.Add(new ObservablePoint(i, yValue));
                }
                else if (i == 5)
                {
                    zeroSeriesValues.Add(new ObservablePoint(i, yValue));
                }
                else
                {
                    negativeSeriesValues.Add(new ObservablePoint(i, yValue));
                }

                HistogramXAxes[0].Labels.Add(emotionDetail.histogram[i].xvalue);
            }

            // 正值
            positiveSeries.Values = positiveSeriesValues;
            HistogramSeries[0] = positiveSeries;
            // 零值
            zeroSeries.Values = zeroSeriesValues;
            HistogramSeries[1] = zeroSeries;
            // 负值
            negativeSeries.Values = negativeSeriesValues;
            HistogramSeries[2] = negativeSeries;
        }

        /// <summary>
        /// 设定转盘样式
        /// </summary>
        /// <param name="pieSeries"></param>
        /// <param name="color"></param>
        private void SetStyle(PieSeries<ObservableValue> pieSeries, SKColor color)
        {
            pieSeries.Fill = new SolidColorPaint(color);
            pieSeries.OuterRadiusOffset = 45;
            pieSeries.MaxRadialColumnWidth = 10;
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
