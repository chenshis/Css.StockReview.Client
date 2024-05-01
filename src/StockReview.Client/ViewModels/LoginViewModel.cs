using HandyControl.Tools.Extension;
using Microsoft.Extensions.Caching.Memory;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Prism.Unity;
using StockReview.Api.IApiService;
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
        private readonly IMemoryCache _memoryCache;
        private readonly ILoginApiService _loginApiService;

        public LoginViewModel(IDialogService dialogService,
                              IMemoryCache memoryCache,
                              ILoginApiService loginApiService)
        {
            this._dialogService = dialogService;
            this._memoryCache = memoryCache;
            this._loginApiService = loginApiService;
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
            get => new DelegateCommand<Window>((w) => SetLogin(w)).ObservesCanExecute(() => IsEnable);
        }

        /// <summary>
        /// 登录方法
        /// </summary>
        private void SetLogin(Window window)
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
            var menus = _loginApiService.GetMenus();
            _memoryCache.Set(SystemConstant.TreeMenuView, menus);

            // 设置弹窗结果
            window.DialogResult = true;
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
