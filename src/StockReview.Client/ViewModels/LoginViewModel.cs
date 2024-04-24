using HandyControl.Tools.Extension;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Prism.Unity;
using StockReview.Client.Views;
using StockReview.Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StockReview.Client.ViewModels
{
    /// <summary>
    /// 登录视图模型
    /// </summary>
    public class LoginViewModel : BindableBase
    {
        private readonly IDialogService _dialogService;

        public LoginViewModel(IDialogService dialogService)
        {
            this._dialogService = dialogService;
        }

        private string _userName;
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _password;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private string _errorMessage;
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }


        private bool _isEnable = true;


        /// <summary>
        /// 是否启用命令
        /// </summary>
        public bool IsEnable
        {
            get { return _isEnable; }
            set { SetProperty(ref _isEnable, value); }
        }


        /// <summary>
        /// 登录命令
        /// </summary>
        public ICommand LoginCommand
        {
            get => new DelegateCommand(SetLogin).ObservesCanExecute(() => IsEnable);
        }

        /// <summary>
        /// 登录方法
        /// </summary>
        private void SetLogin()
        {
            string GetErrorMessage(string msg)
            {
                return string.Concat(SystemConstant.ErrorIcon, msg);
            }

            if (string.IsNullOrWhiteSpace(UserName))
            {
                ErrorMessage = GetErrorMessage(SystemConstant.ErrorUserNameMessage);
                return;
            }

            IsEnable = false;
            //_dialogService.ShowDialog("TestView");
            // todo 登录逻辑
        }

        /// <summary>
        /// 注册命令
        /// </summary>
        public ICommand RegisterCommand
        {
            get => new DelegateCommand<object>((parameter) => SetRegistration(parameter)).ObservesCanExecute(() => IsEnable);
        }

        private void SetRegistration(object parameter)
        {
            // 禁用按钮
            IsEnable = false;

            var loginWindow = parameter as Window;
            if (loginWindow == null)
            {
                return;
            }
            loginWindow.Collapse();

            bool flag = false;
            _dialogService.ShowDialog(SystemConstant.RegisterView, result =>
            {
                // 处理对话框关闭后的结果
            });

            //loginWindow.Show();

            // 启用按钮
            IsEnable = true;
        }


    }
}
