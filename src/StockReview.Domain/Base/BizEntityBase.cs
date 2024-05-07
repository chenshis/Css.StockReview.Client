using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using StockReview.Infrastructure.Config.Snowflake;
using StockReview.Infrastructure.Config.Domain;

namespace StockReview.Domain.Base
{
    public class BizEntityBase : IEntity<long>, ISoftDeletable, IUpdateTimeEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; } = IdWorker.NewDefaultId;

        public string CreateUser { get; set; }

        public virtual string UpdateUser { get; set; }

        [Column(TypeName = "datetime")]
        public virtual DateTime UpdateTime { get; set; } = DateTime.Now;

        [Column(TypeName = "datetime")]
        public virtual DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 软删除状态
        /// </summary>
        public int Status { get; set; }

        public object[] GetKeys()
        {
            throw new NotImplementedException();
        }
    }
}
