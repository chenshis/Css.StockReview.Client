using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using Prism.Regions;
using SkiaSharp;
using Unity;
using StockReview.Infrastructure.Config;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.VisualElements;
using LiveChartsCore.SkiaSharpView.VisualElements;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using System.Collections.ObjectModel;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

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


        private DateTime? _currentDate;
        /// <summary>
        /// 选中日期
        /// </summary>
        public DateTime? CurrentDate
        {
            get { return _currentDate; }
            set { SetProperty(ref _currentDate, value); }
        }

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
        /// 涨停明细
        /// </summary>
        public ObservableCollection<LimitUpDetailModel> LimitUpDetails { get; set; } = new ObservableCollection<LimitUpDetailModel>();

        /// <summary>
        /// 涨停股票明细
        /// </summary>
        public ObservableCollection<LimitUpStockDetailModel> LimitUpStockDetails { get; set; } = new ObservableCollection<LimitUpStockDetailModel>();

        /// <summary>
        /// 刷新
        /// </summary>
        public override void Refresh()
        {
            var apiToday = _stockOutlookApiService.GetToday();
            if (apiToday.Code != 0)
            {
                HandyControl.Controls.Growl.Error(new HandyControl.Data.GrowlInfo
                {
                    Message = apiToday.Msg,
                    Token = SystemConstant.headerGrowl,
                    IsCustom = true,
                    WaitTime = 0
                });
                return;
            }
            string day;
            if (string.IsNullOrWhiteSpace(apiToday.Data))
            {
                day = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                day = apiToday.Data;
            }

            CurrentDate = DateTime.Parse(day);

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
            if (BulletinBoard == null)
            {
                HandyControl.Controls.Growl.Error(new HandyControl.Data.GrowlInfo
                {
                    Message = "看板数据等待加载中！",
                    Token = SystemConstant.headerGrowl,
                    IsCustom = true,
                    WaitTime = 0
                });
                return;
            }
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
            if (emotionDetail == null)
            {
                HandyControl.Controls.Growl.Error(new HandyControl.Data.GrowlInfo
                {
                    Message = "情绪详情数据不存在！",
                    Token = SystemConstant.headerGrowl,
                    IsCustom = true,
                    WaitTime = 0
                });
                return;
            }


            HistogramXAxes = new Axis[1];
            HistogramXAxes[0] = new Axis();
            HistogramXAxes[0].Labels = new List<string>();
            HistogramXAxes[0].LabelsRotation = -30;
            HistogramSeries = new ISeries[3];
            // 正
            var positiveSeries = new ColumnSeries<ObservablePoint>();
            positiveSeries.Fill = new SolidColorPaint(SKColors.Red);
            positiveSeries.DataLabelsPaint = new SolidColorPaint(SKColors.Gray);
            var positiveSeriesValues = new List<ObservablePoint>();
            // 零
            var zeroSeries = new ColumnSeries<ObservablePoint>();
            zeroSeries.Fill = new SolidColorPaint(SKColors.Gray);
            zeroSeries.DataLabelsPaint = new SolidColorPaint(SKColors.Gray);
            var zeroSeriesValues = new List<ObservablePoint>();
            // 负
            var negativeSeries = new ColumnSeries<ObservablePoint>();
            negativeSeries.Fill = new SolidColorPaint(SKColors.Green);
            negativeSeries.DataLabelsPaint = new SolidColorPaint(SKColors.Gray);
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

            // 清空明细
            LimitUpDetails.Clear();
            var todayLimitUpDetail = new LimitUpDetailModel();
            todayLimitUpDetail.Time = "今日";
            todayLimitUpDetail.ZTBan = emotionDetail.root.data.limit_up_count.today.num.ToString();
            todayLimitUpDetail.FBRate = GetPercentage(Convert.ToDouble(emotionDetail.root.data.limit_up_count.today.rate)).ToString("F0") + "%";
            todayLimitUpDetail.ZBan = emotionDetail.root.data.limit_up_count.today.open_num.ToString();
            todayLimitUpDetail.DTBan = emotionDetail.root.data.limit_down_count.today.num.ToString();
            var dFRate = emotionDetail.root.data.limit_down_count.today.rate;
            if (!string.IsNullOrWhiteSpace(dFRate))
            {
                todayLimitUpDetail.DFRate = GetPercentage(Convert.ToDouble(dFRate)) + "%";
            }
            todayLimitUpDetail.QBan = emotionDetail.root.data.limit_down_count.today.open_num.ToString();

            var yesterdayLimitUpDetail = new LimitUpDetailModel();
            yesterdayLimitUpDetail.Time = "昨日";
            yesterdayLimitUpDetail.ZTBan = emotionDetail.root.data.limit_up_count.yesterday.num.ToString();
            yesterdayLimitUpDetail.FBRate = GetPercentage(Convert.ToDouble(emotionDetail.root.data.limit_up_count.yesterday.rate)).ToString("F0") + "%";
            yesterdayLimitUpDetail.ZBan = emotionDetail.root.data.limit_up_count.yesterday.open_num.ToString();
            yesterdayLimitUpDetail.DTBan = emotionDetail.root.data.limit_down_count.yesterday.num.ToString();
            var yesterdayDfRate = emotionDetail.root.data.limit_down_count.yesterday.rate;
            if (!string.IsNullOrWhiteSpace(yesterdayDfRate))
            {
                yesterdayLimitUpDetail.DFRate = GetPercentage(Convert.ToDouble(yesterdayDfRate)) + "%";
            }
            yesterdayLimitUpDetail.QBan = emotionDetail.root.data.limit_down_count.yesterday.open_num.ToString();

            LimitUpDetails.Add(todayLimitUpDetail);
            LimitUpDetails.Add(yesterdayLimitUpDetail);

            // 股票明细
            LimitUpStockDetails.Clear();
            var infos = emotionDetail.root.data.info;
            if (infos != null)
            {
                foreach (var item in infos)
                {
                    LimitUpStockDetailModel stockDetail = new();
                    stockDetail.Code = item.code;
                    var imageValue = !(item.code.Substring(0, 1) == "6") ? "sz" + item.code : "sh" + item.code;
                    stockDetail.ImageUrl = "http://image.sinajs.cn/newchart/min/n/" + imageValue + ".gif";
                    stockDetail.Name = item.name;
                    stockDetail.ZF = Convert.ToDouble(item.change_rate).ToString("0.00");
                    stockDetail.Latest = item.latest;
                    stockDetail.Reason = item.reason_type;
                    stockDetail.FirstLetterTime = GetDateTime(item.first_limit_up_time).ToString("HH:mm:ss");
                    stockDetail.LastFBan = GetDateTime(item.last_limit_up_time).ToString("HH:mm:ss");
                    stockDetail.LimitUpType = item.limit_up_type;
                    stockDetail.OpenNum = item.open_num?.ToString();
                    stockDetail.SeveralDaysBan = item.high_days;
                    stockDetail.FDanMoney = zWy(Convert.ToDouble(item.order_amount)).ToString();
                    stockDetail.TurnoverRate = Convert.ToDouble(item.turnover_rate).ToString("F2") + "%";
                    stockDetail.CurrencyMoney = item.currency_value != null ? zEy(Convert.ToDouble(item.currency_value)).ToString() : null;
                    stockDetail.LBanNum = item.LBanNum;
                    LimitUpStockDetails.Add(stockDetail);
                }
            }

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

        private double GetPercentage(double strlongdou)
        {
            new NumberFormatInfo().PercentDecimalDigits = 2;
            return Math.Round(Convert.ToDouble(strlongdou) * 100.0, 2);
        }
        private DateTime GetDateTime(string strLongTime)
        {
            long num = Convert.ToInt64(strLongTime) * 10000000L;
            long ticks = new DateTime(1970, 1, 1, 8, 0, 0).Ticks + num;
            return new DateTime(ticks);
        }
        private double zWy(double e)
        {
            return Math.Round(e * 0.0001, 0);
        }
        private double zEy(double e)
        {
            return Math.Round(e * 1E-08, 2);
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



    /// <summary>
    /// 涨停明细
    /// </summary>
    public class LimitUpDetailModel
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 涨停板
        /// </summary>
        public string ZTBan { get; set; }

        /// <summary>
        /// 封板率
        /// </summary>
        public string FBRate { get; set; }

        /// <summary>
        /// 炸板
        /// </summary>
        public string ZBan { get; set; }

        /// <summary>
        /// 跌停板
        /// </summary>
        public string DTBan { get; set; }

        /// <summary>
        /// 跌封率
        /// </summary>
        public string DFRate { get; set; }

        /// <summary>
        /// 翘班
        /// </summary>
        public string QBan { get; set; }
    }

    /// <summary>
    /// 涨停股票
    /// </summary>
    public class LimitUpStockDetailModel
    {
        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 涨幅
        /// </summary>
        public string ZF { get; set; }
        /// <summary>
        /// 最新
        /// </summary>
        public string Latest { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 首封时间
        /// </summary>
        public string FirstLetterTime { get; set; }
        /// <summary>
        /// 最后封板
        /// </summary>
        public string LastFBan { get; set; }
        /// <summary>
        /// 涨停形态
        /// </summary>
        public string LimitUpType { get; set; }
        /// <summary>
        /// 开板
        /// </summary>
        public string OpenNum { get; set; }
        /// <summary>
        /// 几天几板
        /// </summary>
        public string SeveralDaysBan { get; set; }
        /// <summary>
        /// 封单额
        /// </summary>
        public string FDanMoney { get; set; }
        /// <summary>
        /// 换手率
        /// </summary>
        public string TurnoverRate { get; set; }
        /// <summary>
        /// 流通值亿
        /// </summary>
        public string CurrencyMoney { get; set; }
        /// <summary>
        /// 连板
        /// </summary>
        public string LBanNum { get; set; }
    }
}
