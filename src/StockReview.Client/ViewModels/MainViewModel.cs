using Prism.Events;
using Prism.Mvvm;
using StockReview.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Client.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        public MainViewModel(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<LoadingEvent>().Subscribe(OnLoadingEvent);
        }


        private bool _isLoading;
        /// <summary>
        /// loading加载
        /// </summary>
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        /// <summary>
        /// loading加载
        /// </summary>
        /// <param name="isLoading"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnLoadingEvent(bool isLoading)
        {
            IsLoading = isLoading;
        }


    }
}
