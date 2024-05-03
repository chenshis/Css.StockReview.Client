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
        }


        public IEnumerable<ISeries> Series { get; set; } =
         GaugeGenerator.BuildSolidGauge(
             new GaugeItem(
                 30,          // the gauge value
                 series =>    // the series style
                 {
                     series.MaxRadialColumnWidth = 20;
                     series.DataLabelsSize = 20;
                 }));

    }
}
