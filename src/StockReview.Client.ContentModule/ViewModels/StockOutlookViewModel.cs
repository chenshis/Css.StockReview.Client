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
using System.Windows.Threading;
using System.Windows.Input;
using Prism.Commands;
using LiveChartsCore.ConditionalDraw;

namespace StockReview.Client.ContentModule.ViewModels
{
    /// <summary>
    /// 看盘视图模型
    /// </summary>
    public class StockOutlookViewModel : NavigationAwareViewModelBase
    {
        private readonly IStockOutlookApiService _stockOutlookApiService;
        private DispatcherTimer _timer;
        private bool _isInit = true;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="unityContainer"></param>
        /// <param name="regionManager"></param>
        /// <param name="stockOutlookApiService"></param>
        public StockOutlookViewModel(IUnityContainer unityContainer,
                                     IRegionManager regionManager,
                                     IStockOutlookApiService stockOutlookApiService)
            : base(unityContainer, regionManager)
        {
            // 开始初始化
            _isInit = true;
            this.PageTitle = "股市看盘";
            this._stockOutlookApiService = stockOutlookApiService;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(30);
            _timer.Tick += Timer_Tick;
            _timer.Start();
            // 数据刷新
            Refresh();
            // 初始化结束
            _isInit = false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            RealTimeBulletinBoard();
        }

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

        private ISeries[] _histogramSeries;
        /// <summary>
        /// 柱状图序列
        /// </summary>
        public ISeries[] HistogramSeries
        {
            get { return _histogramSeries; }
            set { SetProperty(ref _histogramSeries, value); }
        }

        private Axis[] _histogramXAxes;
        /// <summary>
        /// X坐标轴显示内容
        /// </summary>
        public Axis[] HistogramXAxes
        {
            get { return _histogramXAxes; }
            set { SetProperty(ref _histogramXAxes, value); }
        }


        private Axis[] _candleXAxes;
        /// <summary>
        /// 蜡烛图x坐标
        /// </summary>
        public Axis[] CandleXAxes
        {
            get { return _candleXAxes; }
            set { SetProperty(ref _candleXAxes, value); }
        }

        private Axis[] _candleYAxes;
        /// <summary>
        /// 蜡烛图y坐标
        /// </summary>
        public Axis[] CandleYAxes
        {
            get { return _candleYAxes; }
            set { SetProperty(ref _candleYAxes, value); }
        }

        private ISeries[] _candleSeries;
        /// <summary>
        /// 蜡烛图序列
        /// </summary>
        public ISeries[] CandleSeries
        {
            get { return _candleSeries; }
            set { SetProperty(ref _candleSeries, value); }
        }


        private Axis[] _timeXAxes;
        /// <summary>
        /// 折线x坐标
        /// </summary>
        public Axis[] TimeXAxes
        {
            get { return _timeXAxes; }
            set { SetProperty(ref _timeXAxes, value); }
        }

        private ISeries[] _timeSeries;
        /// <summary>
        /// 折线图序列
        /// </summary>
        public ISeries[] TimeSeries
        {
            get { return _timeSeries; }
            set { SetProperty(ref _timeSeries, value); }
        }

        private ISeries[] _timeVolumeSeries;
        /// <summary>
        /// 分时图成交量
        /// </summary>
        public ISeries[] TimeVolumeSeries
        {
            get { return _timeVolumeSeries; }
            set { SetProperty(ref _timeVolumeSeries, value); }
        }



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
            string day;
            if (!CurrentDate.HasValue)
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

                if (string.IsNullOrWhiteSpace(apiToday.Data))
                {
                    day = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    day = apiToday.Data;
                }
                CurrentDate = DateTime.Parse(day);
            }
            else
            {
                day = CurrentDate.Value.ToString("yyyy-MM-dd");
            }

            // 看板数据
            RealTimeBulletinBoard();
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
                HandyControl.Controls.Growl.Info(new HandyControl.Data.GrowlInfo
                {
                    Message = "情绪详情数据等待加载中……",
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

            LimitUpDetails.Clear();
            LimitUpDetails.Add(todayLimitUpDetail);
            LimitUpDetails.Add(yesterdayLimitUpDetail);

            // 股票明细
            LimitUpStockDetails.Clear();
            var infos = emotionDetail.root.data.info;
            if (infos != null)
            {
                int k = 0;
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
                    if (k == 0 && _isInit)
                    {
                        RealTimeSeries(stockDetail);
                    }
                    k++;
                }
            }

        }

        /// <summary>
        /// 看板实时数据
        /// </summary>
        private void RealTimeBulletinBoard()
        {
            if (CurrentDate == null)
            {
                return;
            }
            string day = CurrentDate.Value.ToString("yyyy-MM-dd");
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
                HandyControl.Controls.Growl.Info(new HandyControl.Data.GrowlInfo
                {
                    Message = "看板数据等待加载中……",
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
        }


        /// <summary>
        /// 选中命令
        /// </summary>
        public ICommand SelectionChangedCommand
        {
            get
            {
                return new DelegateCommand<object>(p => RealTimeSeries(p));
            }
        }

        /// <summary>
        /// 分时图
        /// </summary>
        private void RealTimeSeries(object parameter)
        {
            if (!CurrentDate.HasValue)
            {
                return;
            }
            if (parameter is LimitUpStockDetailModel model)
            {
                var apiResponse = _stockOutlookApiService.GetStock(new StockRequestDto
                {
                    Day = CurrentDate.Value.ToString("yyyy-MM-dd"),
                    StockId = model.Code
                });

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
                // 序列赋值
                var candleSeries = new CandlesticksSeries<FinancialPointI>
                {
                    Name = "日线",
                    UpFill = new SolidColorPaint(SKColors.Red),
                    UpStroke = null,
                    DownFill = new SolidColorPaint(SKColors.Green),
                    DownStroke = null,
                    Values = apiResponse.Data.StockDatas.Select(item => new
                    FinancialPointI(
                        item.High,
                        item.Open,
                        item.Close,
                        item.Low)).ToArray()
                };
                CandleSeries = new ISeries[] { candleSeries };
                // x坐标轴
                var xAxis = new Axis
                {
                    MinStep = 1,
                    MinLimit = 0,
                    MaxLimit = 10,
                    Labels = apiResponse.Data.StockDatas.Select(t => t.Date.ToString("yyyy-MM-dd")).ToList(),
                    CrosshairPaint = new SolidColorPaint(SKColors.DarkOrange, 1),
                    CrosshairSnapEnabled = true
                };
                CandleXAxes = new[] { xAxis };
                // y坐标轴
                var yAxis = new Axis
                {
                    CrosshairLabelsBackground = SKColors.DarkOrange.AsLvcColor(),
                    CrosshairLabelsPaint = new SolidColorPaint(SKColors.DarkRed, 1),
                    CrosshairPaint = new SolidColorPaint(SKColors.DarkOrange, 1),
                    CrosshairSnapEnabled = true
                };
                CandleYAxes = new[] { yAxis };

                //折线
                var lineLatestSeries = new LineSeries<double>
                {
                    Values = apiResponse.Data.StockDetailData.Latests.ToArray(),
                    Name = "最新",
                    GeometryFill = null,
                    GeometryStroke = null,
                    LineSmoothness = 0.8,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 1f }
                };
                var lineAvgSeries = new LineSeries<double>
                {
                    Values = apiResponse.Data.StockDetailData.Avgs.ToArray(),
                    Name = "均价",
                    GeometryFill = null,
                    GeometryStroke = null,
                    LineSmoothness = 0.8,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.Orange) { StrokeThickness = 1f }
                };
                TimeSeries = new ISeries[] { lineLatestSeries, lineAvgSeries };
                // x轴
                var timeAxis = new Axis
                {
                    MinLimit = 0,
                    MaxLimit = 10,
                    Labels = apiResponse.Data.StockDetailData.Times.ToArray()
                };
                TimeXAxes = new[] { timeAxis };
                // 分时图成交量
                var volumeColumnSeries = new ColumnSeries<double>
                {
                    Values = apiResponse.Data.StockDetailData.Volumes.ToArray(),
                    Name = "成交量"
                };
                //volumeColumnSeries.OnPointMeasured(point =>
                //{
                //    if (point.Visual is null) return;
                //    var color = apiResponse.Data
                //                           .StockDetailData
                //                           .Colors
                //                           .Where(t => t.Turnover.ToString() == point.AsDataLabel)
                //                           .Select(t => t.Color)
                //                           .FirstOrDefault();
                //    switch (color)
                //    {
                //        case ColorEnum.Red:
                //            point.Visual.Fill = new SolidColorPaint(SKColors.Red);
                //            break;
                //        case ColorEnum.Green:
                //            point.Visual.Fill = new SolidColorPaint(SKColors.Green);
                //            break;
                //        case ColorEnum.Gray:
                //            point.Visual.Fill = new SolidColorPaint(SKColors.Gray);
                //            break;
                //        default:
                //            break;
                //    }
                //});
                TimeVolumeSeries = new ISeries[]
                {
                    volumeColumnSeries
                };
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
