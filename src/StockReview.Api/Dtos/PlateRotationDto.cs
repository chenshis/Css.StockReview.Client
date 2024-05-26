using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.Dtos
{
    public class PlateRotationDto
    {
        public PlateRotationHeaderTitle PlateRotationHeaderTitle { get; set; }

        public List<PlateRotationInfo> PlateRotationInfosOne { get; set; }
        public List<PlateRotationInfo> PlateRotationInfosTwo { get; set; }
        public List<PlateRotationInfo> PlateRotationInfosThree { get; set; } 
        public List<PlateRotationInfo> PlateRotationInfosFour { get; set; } 
        public List<PlateRotationInfo> PlateRotationInfosFive { get; set; }
        public List<PlateRotationInfo> PlateRotationInfosSix { get; set; } 

        public List<PlateSharesLimitUpInfo> PlateSharesLimitUpInfosOne { get; set; } 
        public List<PlateSharesLimitUpInfo> PlateSharesLimitUpInfosTwo { get; set; }
        public List<PlateSharesLimitUpInfo> PlateSharesLimitUpInfosThree { get; set; } 
        public List<PlateSharesLimitUpInfo> PlateSharesLimitUpInfosFour { get; set; }
        public List<PlateSharesLimitUpInfo> PlateSharesLimitUpInfosFive { get; set; } 
        public List<PlateSharesLimitUpInfo> PlateSharesLimitUpInfosSix { get; set; } 
    }

    public class PlateSharesLimitUpInfo
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        public string PlateSharesCode { get; set; }

        /// <summary>
        /// 股票名称
        /// </summary>
        public string PlateSharesName { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public string PlateSharesPrice { get; set; }

        /// <summary>
        /// 涨幅
        /// </summary>
        public string PlateSharesIncrease { get; set; }

        /// <summary>
        /// 地位
        /// </summary>
        public string PlateSharesStatus { get; set; }

        /// <summary>
        /// 连板数
        /// </summary>
        public string PlateSharesNumberBoards { get; set; }

        /// <summary>
        /// 主力
        /// </summary>
        public string PlateSharesMainForce { get; set; }

        /// <summary>
        /// 板块名称
        /// </summary>
        public string PlateSharesToName { get; set; }

        /// <summary>
        /// 成交额
        /// </summary>
        public string PlateSharesTranVoume { get; set; }

        /// <summary>
        /// 流通值
        /// </summary>
        public string PlateSharesCirculationValue { get; set; }

        public string PlateSharesTest { get; set; }
    }


    public class PlateRotationInfo
    {

        /// <summary>
        /// 板块代码
        /// </summary>
        public string PlateCode { get; set; }
        /// <summary>
        /// 板块名称
        /// </summary>
        public string PlateName { get; set; }

        /// <summary>
        /// 强度
        /// </summary>
        public string PlateStrength { get; set; }
        /// <summary>
        /// 主力净额(亿)
        /// </summary>
        public string PlateMainForce { get; set; }

        /// <summary>
        /// 主力买(亿)
        /// </summary>
        public string PlateMainBuy { get; set; }

        /// <summary>
        /// 主力卖(亿)
        /// </summary>
        public string PlateMainSell { get; set; }
    }


    public class PlateRotationHeaderTitle
    {
        /// <summary>
        /// 标题1-6
        /// </summary>
        public string PlateDateOne { get; set; }
        public string PlateDateTwo { get; set; }
        public string PlateDateThree { get; set; }
        public string PlateDateFour { get; set; }
        public string PlateDateFive { get; set; }
        public string PlateDateSix { get; set; }
    }
}
