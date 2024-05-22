using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.Dtos
{
    public class DragonTigerDto
    {
        public List<DragonTigerGetInfo> DragonTigerGetInfos { get; set; } = new List<DragonTigerGetInfo>();
        public List<DragonTigerGetInfo> DragonTigerGetInfosOne { get; set; } = new List<DragonTigerGetInfo>();
        public List<DragonTigerGetInfo> DragonTigerGetInfosTwo { get; set; } = new List<DragonTigerGetInfo>();
        public List<DragonTigerGetInfo> DragonTigerGetInfosThree { get; set; } = new List<DragonTigerGetInfo>();
        public List<DragonTigerGetInfo> DragonTigerGetInfosFous { get; set; } = new List<DragonTigerGetInfo>();
    }

    public class DragonTigerGetInfo
    {
        /// <summary>
        /// 股票名称
        /// </summary>
        public string DragonName { get; set; }
        /// <summary>
        /// 游资
        /// </summary>
        public string DragonSpeculative { get; set; }
        /// <summary>
        /// 涨幅
        /// </summary>
        public string DragonIncrease { get; set; }

        /// <summary>
        /// 涨幅 颜色
        /// </summary>
        public string DragonIncreaseColor { get; set; }
        /// <summary>
        /// 净买
        /// </summary>
        public string DragonPurchase { get; set; }
         /// <summary>
        /// 净买 颜色
        /// </summary>
        public string DragonPurchaseColor { get; set; }
        /// <summary>
        /// 板
        /// </summary>
        public string DragonPlate { get; set; }
    }
}
