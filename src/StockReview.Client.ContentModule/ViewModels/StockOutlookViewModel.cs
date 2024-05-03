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

namespace StockReview.Client.ContentModule.ViewModels
{
    /// <summary>
    /// 看盘视图模型
    /// </summary>
    public class StockOutlookViewModel : NavigationAwareViewModelBase
    {
        public StockOutlookViewModel(IUnityContainer unityContainer,
                                     IRegionManager regionManager)
            : base(unityContainer, regionManager)
        {
            this.PageTitle = "股市看盘";
            VisualElements = new VisualElement<SkiaSharpDrawingContext>[]
          {
            new AngularTicksVisual
            {
                LabelsSize = 16,
                LabelsOuterOffset = 15,
                OuterOffset = 65,
                TicksLength = 20
            },
            Needle
          };
        }


        public IEnumerable<ISeries> Series { get; set; } = GaugeGenerator.BuildAngularGaugeSections(
                new GaugeItem(60, s => SetStyle(130, 20, s)),
                new GaugeItem(30, s => SetStyle(130, 20, s)),
                new GaugeItem(10, s => SetStyle(130, 20, s)));

        public IEnumerable<VisualElement<SkiaSharpDrawingContext>> VisualElements { get; set; }

        public NeedleVisual Needle { get; set; } =
        new NeedleVisual
        {
            Value = 45
        };
        private static void SetStyle(double sectionsOuter,
                                         double sectionsWidth,
                                         PieSeries<ObservableValue> series)
        {
            series.OuterRadiusOffset = sectionsOuter;
            series.MaxRadialColumnWidth = sectionsWidth;
        }
    }
}
