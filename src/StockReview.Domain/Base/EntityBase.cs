using StockReview.Infrastructure.Config.Domain;
using StockReview.Infrastructure.Config.Snowflake;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StockReview.Domain.Base
{
    public class EntityBase : IEntity<long>, ISoftDeletable, IUpdateTimeEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; } = IdWorker.NewDefaultId;

        /// <summary>
        /// 软删除状态
        /// </summary>
        public int Status { get; set; }

        public object[] GetKeys()
        {
            throw new NotImplementedException();
        }

        [Column(TypeName = "datetime")]
        public virtual DateTime UpdateTime { get; set; } = DateTime.Now;

        [Column(TypeName = "datetime")]
        public virtual DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
