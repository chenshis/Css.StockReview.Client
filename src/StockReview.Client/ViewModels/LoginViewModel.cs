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
            get
            {
                return _userName;
            }
            set
            {
                SetProperty(ref _userName, value);
            }
        }

        private string _password;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get { return _password; }
            set
            {
                SetProperty(ref _password, value);
            }
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
        /// 忘记密码命令
        /// </summary>
        public ICommand ForgotPasswordCommand
        {
            get => new DelegateCommand(SetForgotPassword).ObservesCanExecute(() => IsEnable);
        }

        private void SetForgotPassword()
        {
            // 禁用按钮
            IsEnable = false;
            _dialogService.ShowDialog(SystemConstant.ForgotPasswordView, dialogResult =>
            {

            });
            // 启用按钮
            IsEnable = true;
        }

        /// <summary>
        /// 修改密码命令
        /// </summary>
        public ICommand UpdatePasswordCommand
        {
            get => new DelegateCommand(UpdateForgotPassword).ObservesCanExecute(() => IsEnable);
        }

        private void UpdateForgotPassword()
        {
            // 禁用按钮
            IsEnable = false;
            _dialogService.ShowDialog(SystemConstant.UpdatePasswordView, dialogResult =>
            {

            });
            // 启用按钮
            IsEnable = true;
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
            ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(UserName))
            {
                ErrorMessage = GetErrorMessage(SystemConstant.ErrorEmptyUserNameMessage);
                return;
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = GetErrorMessage(SystemConstant.ErrorEmptyPasswordMessage);
                return;
            }
        }

        /// <summary>
        /// 注册命令
        /// </summary>
        public ICommand RegisterCommand
        {
            get => new DelegateCommand(SetRegistration).ObservesCanExecute(() => IsEnable);
        }

        private void SetRegistration()
        {
            // 禁用按钮
            IsEnable = false;
            _dialogService.ShowDialog(SystemConstant.RegisterView, dialogResult =>
            {

            });
            // 启用按钮
            IsEnable = true;
        }


    }
}
