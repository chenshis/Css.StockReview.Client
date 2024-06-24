using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Prism.Events;
using Prism.Regions;
using SkiaSharp;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
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
        
        private readonly IReplayService _replayService;
        private readonly IEventAggregator _eventAggregator;

        public MarketSentimentViewModel(IUnityContainer unityContainer, IRegionManager regionManager
             , IReplayService replayService, IEventAggregator eventAggregator) : base(unityContainer, regionManager)
        {
            this.PageTitle = "市场情绪";
            this._eventAggregator = eventAggregator;
            this._replayService = replayService;
            for (int i = 0; i < 5; i++)
            {
                DateItem.Add(DateTime.Now.AddYears(-i).Year.ToString());
            }

            InitTableHeader(DateTime.Now.Year);
        }
        
        public void InitTableHeader(int year) 
        {
            var highestList = _replayService.GetHighest(year);

            MarketSentimentDataDtos.Clear();
            MarketSentimentDataDtos.AddRange(highestList);
        
        }

        private static readonly SKColor s_gray = new(195, 195, 195);
        private static readonly SKColor s_gray1 = new(160, 160, 160);
        private static readonly SKColor s_gray2 = new(90, 90, 90);
        private static readonly SKColor s_dark3 = new(60, 60, 60);

        public ISeries[] Series { get; set; } =
        {
            new LineSeries<ObservablePoint>
            {
                Values = Fetch(),
                Stroke = new SolidColorPaint(new SKColor(255, 208, 0)),
                Fill = null,
                GeometryFill = new SolidColorPaint(new SKColor(255, 208, 0)),
                GeometryStroke = new SolidColorPaint(new SKColor(255, 208, 0)) { StrokeThickness = 2 },
                DataLabelsSize = 25,
                DataLabelsPaint=new SolidColorPaint(new SKColor(255, 208, 0)),
                DataLabelsPosition=LiveChartsCore.Measure.DataLabelsPosition.Top,
                DataLabelsFormatter=(point)=>point.PrimaryValue.ToString()
            }
        };

        public Axis[] XAxes { get; set; } =
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
               Labels = new string[] { },
               MinLimit=0,
               MaxLimit=50,  
               MinStep=1
            }
        };

        public Axis[] YAxes { get; set; } =
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
                Labels = new string[] { },
                MinLimit =0,
                MaxLimit=15,
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

        private static List<ObservablePoint> Fetch()
        {
            var list = new List<ObservablePoint>();

            Random random = new Random(10);

            for (int i = 0; i <= 50; i++)
            {
                list.Add(new ObservablePoint(i > 0 ? i + 0.5 : i + 0.5, random.Next(0, 8)));
            }
            return list;
        }
    }

    public class DataPoint
    {
       
        public List<string> Head1 { get; set; }

        public List<string> Head2 { get; set; }

    }
}
