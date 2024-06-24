using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockReview.Domain.Entities;
using StockReview.Infrastructure.Config.Domain;
using StockReview.Infrastructure.Config.Snowflake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Domain
{
    /// <summary>
    /// 股市数据库上下文
    /// </summary>
    public class StockReviewDbContext : DbContext
    {
        private readonly ILogger<StockReviewDbContext> _logger;

        public StockReviewDbContext(DbContextOptions<StockReviewDbContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            _logger = loggerFactory.CreateLogger<StockReviewDbContext>();
        }

        /// <summary>
        /// 用户实体Db
        /// </summary>
        public DbSet<UserEntity> UserEntities { get; set; }

        /// <summary>
        /// 看板实体Db
        /// </summary>
        public DbSet<BulletinBoardEntity> BulletinBoardEntities { get; set; }

        /// <summary>
        /// 分时实体Db
        /// </summary>
        public DbSet<TimeIndexChartEntity> TimeIndexChartEntities { get; set; }

        /// <summary>
        /// 股票明细实体Db
        /// </summary>
        public DbSet<StockDetailEntity> StockDetailEntities { get; set; }


        #region 软删除相关配置值

        private const string IsDeletedProperty = nameof(ISoftDeletable.Status);
        private const int DeleteValue = 1;

        #endregion

        #region 软删除查询实现相关方法

        private static readonly MethodInfo PropertyMethod =
            typeof(EF).GetMethod(nameof(EF.Property), BindingFlags.Static | BindingFlags.Public)
                ?.MakeGenericMethod(typeof(int));

        private static LambdaExpression GetIsDeletedRestriction(Type type)
        {
            var pram = Expression.Parameter(type, "it");
            var prop = Expression.Call(PropertyMethod, pram, Expression.Constant(IsDeletedProperty));
            var condition = Expression.MakeBinary(ExpressionType.NotEqual, prop, Expression.Constant(DeleteValue));
            var lambda = Expression.Lambda(condition, pram);
            return lambda;
        }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region 软删除查询实现

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(entity.ClrType))
                {
                    modelBuilder
                        .Entity(entity.ClrType)
                        .HasQueryFilter(GetIsDeletedRestriction(entity.ClrType));
                }
            }

            #endregion
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 软删除保存实现
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            var entries = ChangeTracker.Entries().ToList();

            #region markedAsModified

            var markedAsAdded = entries.Where(x => x.State == EntityState.Added);

            foreach (var item in markedAsAdded)
            {
                if (item.Entity is IEntity<long> entity)
                {
                    if (entity.Id == 0)
                    {
                        entity.Id = IdWorker.NewDefaultId;
                    }
                }
            }

            #endregion

            #region markedAsModified

            var markedAsModified = entries.Where(x => x.State == EntityState.Modified);

            foreach (var item in markedAsModified)
            {
                if (item.Entity is IUpdateTimeEntity entity)
                {
                    entity.UpdateTime = DateTime.Now;
                }
            }

            #endregion

            return base.SaveChanges();
        }
    }
}
