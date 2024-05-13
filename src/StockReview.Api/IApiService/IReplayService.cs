﻿using StockReview.Api.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.IApiService
{
    /// <summary>
    /// 复盘接口
    /// </summary>
    public interface IReplayService
    {
        /// <summary>
        /// 获取龙头晋级数据
        /// </summary>
        /// <param name="date"></param>
        List<LeadingDateHeaderDao> GetLeadingGroupPromotion(DateTime date);
    }
}
