using StockReview.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.Dtos
{
    public class UpdateUserRoleRequestDto
    {
        public string UserName { get; set; }

        public RoleEnum Role { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? Expires { get; set; }
    }
}
