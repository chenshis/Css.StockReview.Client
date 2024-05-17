using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NCrontab;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StockReview.Infrastructure.Config
{
    public abstract class StockReviewBackgroundService : BackgroundService
    {
        private CrontabSchedule _schedule;
        private DateTime _nextRun;
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// job执行Corn表达式 支持秒
        /// "*/30 * * * * *"
        /// </summary>
        protected abstract string Schedule { get; set; }

        /// <summary>
        /// 定时任务是否启动
        /// </summary>
        protected virtual bool IsStart { get; set; } = true;

        public StockReviewBackgroundService(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// 具体业务执行代码
        /// </summary>
        /// <returns></returns>
        protected abstract Task Process(IServiceProvider serviceProvider);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (string.IsNullOrEmpty(Schedule))
            {
                throw new Exception("Schedule 不能为空！");
            }

            #region 初始化           
            var key = $"{AppDomain.CurrentDomain.FriendlyName}.{GetType().Name}";
            _schedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions
            {
                IncludingSeconds = true
            });
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);

            #endregion
            do
            {
                try
                {
                    var now = DateTime.Now;
                    if (now >= _nextRun)
                    {
                        await ExecProcess(key);
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"{key}执行异常");
                }

                await Task.Delay(1000, stoppingToken); //1 seconds delay
            } while (!stoppingToken.IsCancellationRequested);

            await Task.CompletedTask;
        }


        private async Task ExecProcess(string key)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                await Process(scope.ServiceProvider);
            }
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
        }

        #region Logger

        /// <summary>
        /// 
        /// </summary>
        protected ILogger Logger => LazyLogger.Value;

        public ILoggerFactory LoggerFactory => ServiceProvider.GetRequiredService<ILoggerFactory>();

        private Lazy<ILogger> LazyLogger =>
            new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);

        #endregion
    }
}
