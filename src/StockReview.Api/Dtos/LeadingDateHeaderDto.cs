using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.Dtos
{
    public class LeadingDateHeaderDto
    {
        /// <summary>
        /// 行
        /// </summary>
        public int HeadRow { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        public int HeadColumn { get; set; }

        /// <summary>
        /// 头部名称
        /// </summary>
        public string HeadName { get; set; }

        public string HeadColor { get; set; }
        public string HeadFontColor { get; set; }
    }
}
