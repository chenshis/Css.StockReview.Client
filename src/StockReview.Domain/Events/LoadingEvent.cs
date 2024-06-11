using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Domain.Events
{
    /// <summary>
    /// loading 加载
    /// </summary>
    public class LoadingEvent : PubSubEvent<bool>
    {

    }
}
