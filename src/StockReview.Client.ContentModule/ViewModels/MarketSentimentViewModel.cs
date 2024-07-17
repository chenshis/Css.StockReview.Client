using HandyControl.Tools.Extension;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using NPOI.SS.Formula.Atp;
using Prism.Events;
using Prism.Regions;
using SkiaSharp;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using StockReview.Client.ContentModule.Common;
using StockReview.Domain.Events;
using System.Collections.ObjectModel;
using Unity;


namespace StockReview.Client.ContentModule.ViewModels
{
    public class MarketSentimentViewModel : NavigationAwareViewModelBase
    {
        public ObservableCollection<string> DateItem { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<MarketSentimentDataDto> MarketSentimentDataDtos { get; set; } = new ObservableCollection<MarketSentimentDataDto>();
        public ObservableCollection<string> DataPoints { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<ObservablePoint> ObservablePoints { get; set; }=new ObservableCollection<ObservablePoint>();

        public ObservableCollection<DataItem> DataItems { get; set; }= new ObservableCollection<DataItem>();
        public ObservableCollection<DateTime> Dates { get; set; } = new ObservableCollection<DateTime>();
        public ObservableCollection<Dictionary<DateTime, string>> TransformedData { get; set; } = new ObservableCollection<Dictionary<DateTime, string>>();

        private ISeries[] _pointSeries;
        /// <summary>
        /// 折线图序列
        /// </summary>
        public ISeries[] PointSeries
        {
            get { return _pointSeries; }
            set { SetProperty(ref _pointSeries, value); }
        }

        private Axis[] _pointXAxes;
        /// <summary>
        /// 蜡烛图x坐标
        /// </summary>
        public Axis[] PointXAxes
        {
            get { return _pointXAxes; }
            set { SetProperty(ref _pointXAxes, value); }
        }

        private readonly IReplayService _replayService;
        private readonly IEventAggregator _eventAggregator;


        public MarketSentimentViewModel(IUnityContainer unityContainer, IRegionManager regionManager
             , IReplayService replayService, IEventAggregator eventAggregator) : base(unityContainer, regionManager)
        {
            this.PageTitle = "市场情绪";
            this._eventAggregator = eventAggregator;
            this._replayService = replayService;
            for (int i = 0; i < 1; i++)
            {
                DateItem.Add(DateTime.Now.AddYears(-i).Year.ToString());
            }

            InitTableHeader();

           
        }

        private IEnumerable<Dictionary<DateTime, string>> TransformData()
        {
            var groupedData = DataItems.GroupBy(item => item.Date)
                                       .ToDictionary(g => g.Key, g => g.Select(item => item.Value).ToList());

            var maxRows = groupedData.Values.Max(v => v.Count);
            var transformed = new List<Dictionary<DateTime, string>>();

            for (int i = 0; i < maxRows; i++)
            {
                var row = new Dictionary<DateTime, string>();
                foreach (var date in Dates)
                {
                    row[date] = groupedData.ContainsKey(date) && groupedData[date].Count > i ? groupedData[date][i] : string.Empty;
                }
                transformed.Add(row);
            }

            return transformed;
        }

        public void InitTableHeader()
        {
            var dataList = JsonCacheManager.GetMarDataList();

            if (dataList.MarkDataInfos == null)
            {
                dataList.MarkDataInfos = new List<Common.Model.MarkDataToInfo>();
            }

            if (dataList.MarkDataInfos.Count <= 0)
            {
                dataList.MarkDataInfos.Clear();//测试用
                for (int i = 0; i < DateItem.Count; i++)
                {
                    var highestList = _replayService.GetHighest(Convert.ToInt32(DateItem[i]));

                    dataList.MarkDataInfos.Add(new Common.Model.MarkDataToInfo
                    {
                        Year = Convert.ToInt32(DateItem[i]),
                        MarketSentimentDataDtos = highestList
                    });
                }
            }
            JsonCacheManager.SetMarDataList(dataList);

            MarketSentimentDataDtos.Clear();
            var dataToList = dataList.MarkDataInfos.FirstOrDefault(x => x.Year == DateTime.Now.Year);
            MarketSentimentDataDtos.AddRange(dataToList.MarketSentimentDataDtos);

            var pointSeries = new LineSeries<ObservablePoint>
            {
                Values = Fetch(1),
                Stroke = new SolidColorPaint(new SKColor(255, 208, 0)),
                Fill = null,
                GeometryFill = new SolidColorPaint(new SKColor(255, 208, 0)),
                GeometryStroke = new SolidColorPaint(new SKColor(255, 208, 0)) { StrokeThickness = 2 },
                DataLabelsSize = 25,
                DataLabelsPaint = new SolidColorPaint(new SKColor(255, 208, 0)),
                DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
                DataLabelsFormatter = (point) => point.PrimaryValue.ToString()
            };

            var pointSeries2 = new LineSeries<ObservablePoint>
            {
                Values = Fetch(2),
                Stroke = new SolidColorPaint(new SKColor(255, 0, 0)),
                Fill = null,
                GeometryFill = new SolidColorPaint(new SKColor(255, 0, 0)),
                GeometryStroke = new SolidColorPaint(new SKColor(255, 0, 0)) { StrokeThickness = 2 },
                DataLabelsSize = 25,
                DataLabelsPaint = new SolidColorPaint(new SKColor(255, 0, 0)),
                DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
                DataLabelsFormatter = (point) => point.PrimaryValue.ToString()
            };

            var pointSeries3 = new LineSeries<ObservablePoint>
            {
                Values = Fetch(3),
                Stroke = new SolidColorPaint(new SKColor(0, 0, 255)),
                Fill = null,
                GeometryFill = new SolidColorPaint(new SKColor(0, 0, 255)),
                GeometryStroke = new SolidColorPaint(new SKColor(0, 0, 255)) { StrokeThickness = 2 },
                DataLabelsSize = 25,
                DataLabelsPaint = new SolidColorPaint(new SKColor(0, 0, 255)),
                DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
                DataLabelsFormatter = (point) => point.PrimaryValue.ToString()
            };

            var pointSeries4 = new LineSeries<ObservablePoint>
            {
                Values = Fetch(4),
                Stroke = new SolidColorPaint(new SKColor(0, 255, 0)),
                Fill = null,
                GeometryFill = new SolidColorPaint(new SKColor(0, 255, 0)),
                GeometryStroke = new SolidColorPaint(new SKColor(0, 255, 0)) { StrokeThickness = 2 },
                DataLabelsSize = 25,
                DataLabelsPaint = new SolidColorPaint(new SKColor(0, 255, 0)),
                DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
                DataLabelsFormatter = (point) => point.PrimaryValue.ToString()
            };

            PointSeries = new ISeries[] { pointSeries, pointSeries2, pointSeries3, pointSeries4 };

            var pointXAxes = new Axis
            {
                SeparatorsPaint = new SolidColorPaint
                {

                    Color = s_gray,
                    StrokeThickness = 1,
                },
                SubseparatorsPaint = new SolidColorPaint
                {
                    Color = s_gray2,
                    StrokeThickness = 0.5f
                },
                SubseparatorsCount = 9,
                ZeroPaint = new SolidColorPaint
                {
                    Color = s_gray1,
                    StrokeThickness = 2
                },
                TicksPaint = new SolidColorPaint
                {
                    Color = s_gray,
                    StrokeThickness = 1.5f
                },
                SubticksPaint = new SolidColorPaint
                {
                    Color = s_gray,
                    StrokeThickness = 1
                },
                //Labels = new string[] { },
                MinLimit = 0,
                MaxLimit = MarketSentimentDataDtos.Where(x=>x.type==1).Count(),
                MinStep = 1
            };
            PointXAxes = new[] { pointXAxes };


        }

        private static readonly SKColor s_gray = new(195, 195, 195);
        private static readonly SKColor s_gray1 = new(160, 160, 160);
        private static readonly SKColor s_gray2 = new(90, 90, 90);
        private static readonly SKColor s_dark3 = new(60, 60, 60);

        public Axis[] PointYAxes { get; set; } =
        {
            new Axis
            {
                SeparatorsPaint = new SolidColorPaint
                {
                    Color = s_gray,
                    StrokeThickness = 1,
                },
                SubseparatorsPaint = new SolidColorPaint
                {
                    Color = s_gray2,
                    StrokeThickness = 0.5f
                },
                SubseparatorsCount = 9,
                ZeroPaint = new SolidColorPaint
                {
                    Color = s_gray1,
                    StrokeThickness = 2
                },
                TicksPaint = new SolidColorPaint
                {
                    Color = s_gray,
                    StrokeThickness = 1.5f
                },
                SubticksPaint = new SolidColorPaint
                {
                    Color = s_gray,
                    StrokeThickness = 1
                },
                //Labels = new string[] { },
                MinLimit =0,
                MaxLimit=150,
                MinStep=1
            }
        };

        public DrawMarginFrame Frame { get; set; } =
        new()
        {
            Fill = new SolidColorPaint(s_dark3),
            Stroke = new SolidColorPaint
            {
                Color = s_gray,
                StrokeThickness = 1
            }
        };

        private  List<ObservablePoint> Fetch(int type)
        {
            var list = new List<ObservablePoint>();

            var markOne = MarketSentimentDataDtos.Where(x => x.type == type).ToList();

            for (int i = 0; i < markOne.Count; i++)
            {
                list.Add(new ObservablePoint(i <= 0 ? 0.5 : i - 0.5, markOne[i].name.Count()));

                if (type==1)
                {
                    for (int j = 0; j < markOne[i].name.Count; j++)
                    {
                        DataItems.Add(new DataItem
                        {
                            Date = Convert.ToDateTime(markOne[i].date),
                            Value = markOne[i].name[j].ToString()
                        });
                    }
                }
                
              
            }
            if (type==1)
            {
                Dates = new ObservableCollection<DateTime>(DataItems.Select(item => item.Date).Distinct());

                TransformedData = new ObservableCollection<Dictionary<DateTime, string>>(TransformData());
            }

            return list;
        }
    }

    public class DataItem
    {
        public DateTime Date { get; set; }
        public string Value { get; set; }
    }
}
