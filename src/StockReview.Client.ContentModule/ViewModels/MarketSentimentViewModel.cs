﻿using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace StockReview.Client.ContentModule.ViewModels
{
    public class MarketSentimentViewModel : NavigationAwareViewModelBase
    {
        public ObservableCollection<string> DateItem { get; set; } = new ObservableCollection<string>();



        public MarketSentimentViewModel(IUnityContainer unityContainer, IRegionManager regionManager) : base(unityContainer, regionManager)
        {
            this.PageTitle = "市场情绪";

            for (int i = 0; i < 5; i++)
            {
                DateItem.Add(DateTime.Now.AddYears(-i).Year.ToString());
            }

            //var data = new FinancialData[]
            //{
            //   new(new DateTime(2021, 1, 1), 523, 500, 450, 400),
            //   new(new DateTime(2021, 1, 2), 500, 450, 425, 400),
            //   new(new DateTime(2021, 1, 3), 490, 425, 400, 380),
            //   new(new DateTime(2021, 1, 4), 420, 400, 420, 380),
            //   new(new DateTime(2021, 1, 5), 520, 420, 490, 400),
            //   new(new DateTime(2021, 1, 6), 580, 490, 560, 440),
            //   new(new DateTime(2021, 1, 7), 570, 560, 350, 340),
            //   new(new DateTime(2021, 1, 8), 380, 350, 380, 330),
            //   new(new DateTime(2021, 1, 9), 440, 380, 420, 350),
            //   new(new DateTime(2021, 1, 10), 490, 420, 460, 400),
            //   new(new DateTime(2021, 1, 11), 520, 460, 510, 460),
            //   new(new DateTime(2021, 1, 12), 580, 510, 560, 500),
            //   new(new DateTime(2021, 1, 13), 600, 560, 540, 510),
            //   new(new DateTime(2021, 1, 14), 580, 540, 520, 500),
            //   new(new DateTime(2021, 1, 15), 580, 520, 560, 520),
            //   new(new DateTime(2021, 1, 16), 590, 560, 580, 520),
            //   new(new DateTime(2021, 1, 17), 650, 580, 630, 550),
            //   new(new DateTime(2021, 1, 18), 680, 630, 650, 600),
            //   new(new DateTime(2021, 1, 19), 670, 650, 600, 570),
            //   new(new DateTime(2021, 1, 20), 640, 600, 610, 560),
            //   new(new DateTime(2021, 1, 21), 630, 610, 630, 590)
            //};

            //CandleSeries = new ISeries[]
            //{
            //    new CandlesticksSeries<FinancialPointI>
            //{
            //    Values = data
            //        .Select(x => new FinancialPointI(x.High, x.Open, x.Close, x.Low))
            //        .ToArray()
            //}
            //};

            //XAxes = new[]
            //{
            //    new Axis
            //    {
            //        LabelsRotation = 15,
            //        Labels = data
            //            .Select(x => x.Date.ToString("yyyy MMM dd"))
            //            .ToArray()
            //    }
            //};


        }
        public ISeries[] CandleSeries { get; set; }
        public Axis[] XAxes { get; set; }

        public Axis[] YAxes { get; set; } = null;

    }
}
