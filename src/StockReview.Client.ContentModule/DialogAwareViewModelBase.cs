using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StockReview.Client.ContentModule
{
    /// <summary>
    /// 对话框基类
    /// </summary>
    public class DialogAwareViewModelBase : BindableBase, IDialogAware
    {
        private string _isMinimized = "Normal";
        /// <summary>
        /// 最小化
        /// </summary>
        public string IsMinimized
        {
            get { return _isMinimized; }
            set { SetProperty(ref _isMinimized, value); }
        }

        /// <summary>
        /// 最小化命令
        /// </summary>
        public ICommand MinimizedCommand { get => new DelegateCommand(() => IsMinimized = "Minimized"); }

        /// <summary>
        /// 关闭用户窗口命令
        /// </summary>
        public ICommand CloseCommand { get => new DelegateCommand(() => RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel))); }

        /// <summary>
        /// 执行成功窗口命令
        /// </summary>
        public ICommand SuccessCommand
        {
            get =>
                new DelegateCommand<IDialogParameters>((parameters) => RequestClose?.Invoke(new DialogResult(ButtonResult.OK, parameters)));
        }

        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// 请求关闭事件
        /// </summary>
        public event Action<IDialogResult> RequestClose;

        /// <summary>
        /// 是否可用关闭窗口
        /// </summary>
        /// <returns></returns>
        public virtual bool CanCloseDialog() => true;

        /// <summary>
        /// 关闭窗口调用函数
        /// </summary>
        public virtual void OnDialogClosed()
        {

        }

        /// <summary>
        /// 打开窗口调用函数
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}