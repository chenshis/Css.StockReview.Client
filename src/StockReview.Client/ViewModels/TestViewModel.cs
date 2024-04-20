using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Client.ViewModels
{
    public class TestViewModel:BindableBase, INavigationAware
    {
        public TestViewModel()
        {
            
        }

        private string _title="Test View Content";

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
           
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
           
        }
    }
}
