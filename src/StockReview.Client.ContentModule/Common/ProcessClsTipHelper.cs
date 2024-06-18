using System.Diagnostics;
using System.IO;

namespace StockReview.Client.ContentModule.Common
{
    /// <summary>
    /// 财联社进程帮助类
    /// </summary>
    public static class ProcessClsTipHelper
    {
        /// <summary>
        /// 进程名称
        /// </summary>
        private static string ProcessName = "clsTip";

        /// <summary>
        /// 杀死财联社进程
        /// </summary>
        public static void KillProcess()
        {
            var processes = Process.GetProcessesByName(ProcessName);
            if (processes != null && processes.Count() > 0)
            {
                foreach (var process in processes)
                {
                    process.Kill();
                }
            }
        }

        /// <summary>
        /// 启动进程
        /// </summary>
        public static void StartProcess()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ProcessName);
            if (!Directory.Exists(path))
            {
                return;
            }
            var fileName = Path.Combine(path, $"{ProcessName}.exe");
            if (!File.Exists(fileName))
            {
                return;
            }
            // 杀死进程
            KillProcess();
            // 启动进程
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = fileName;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.Verb = "RunAs";//以管理员身份运行
            Process.Start(startInfo);
        }
    }
}
