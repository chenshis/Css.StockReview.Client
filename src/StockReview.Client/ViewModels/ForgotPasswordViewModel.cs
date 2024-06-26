﻿using Namotion.Reflection;
using Prism.Commands;
using Prism.Services.Dialogs;
using StockReview.Api.IApiService;
using StockReview.Client.ContentModule;
using StockReview.Infrastructure.Config;
using System.Windows.Controls;
using System.Windows.Input;

namespace StockReview.Client.ViewModels
{
    /// <summary>
    /// 找回密码
    /// </summary>
    public class ForgotPasswordViewModel : DialogAwareViewModelBase
    {
        private readonly ILoginApiService _loginApiService;
        public ForgotPasswordViewModel(ILoginApiService loginApiService)
        {
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

        private string _repeatPassword;
        /// <summary>
        /// 确认密码
        /// </summary>
        public string RepeatPassword
        {
            get { return _repeatPassword; }
            set { SetProperty(ref _repeatPassword, value); }
        }

        private string _qq;
        /// <summary>
        /// QQ号码
        /// </summary>
        public string QQ
        {
            get { return _qq; }
            set { SetProperty(ref _qq, value); }
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
        /// 修改密码命令
        /// </summary>
        public ICommand UpdatePasswordCommand { get => new DelegateCommand<UserControl>((u)=>SetUpdatePassword(u)).ObservesCanExecute(() => IsEnable); }

        private void SetUpdatePassword(UserControl uControl)
        {
            HandyControl.Controls.PasswordBox newPwd = null;
            newPwd = uControl.FindName(nameof(newPwd)) as HandyControl.Controls.PasswordBox;
            Password = newPwd.Password;

            HandyControl.Controls.PasswordBox confirmPwd = null;
            confirmPwd = uControl.FindName(nameof(confirmPwd)) as HandyControl.Controls.PasswordBox;
            RepeatPassword = confirmPwd.Password;
            IsEnable = false;
            if (!Valiedate())
            {
                IsEnable = true;
                return;
            }
            var apiResponse = _loginApiService.ForgotPassword(new()
            {
                UserName = UserName,
                Password = Password,
                ConfirmPassword = RepeatPassword,
                QQ = QQ
            });

            if (apiResponse.Code != 0)
            {
                ErrorMessage = apiResponse.Msg;
                IsEnable = true;
                return;
            }
            if (apiResponse.Data != true)
            {
                ErrorMessage = SystemConstant.ErrorDataSumbit;
                IsEnable = true;
                return;
            }
            HandyControl.Controls.MessageBox.Success(SystemConstant.ForgotPasswordSuccess, SystemConstant.ForgotPasswordWindow);
            // 关闭忘记密码窗口
            IDialogParameters dialogParameters = new DialogParameters();
            dialogParameters.Add(nameof(UserName), UserName);
            dialogParameters.Add(nameof(Password), Password);
            SuccessCommand.Execute(dialogParameters);
        }



        /// <summary>
        /// 验证
        /// </summary>
        private bool Valiedate()
        {
            ErrorMessage = string.Empty;
            string GetErrorMessage(string propertyName, string errorTemplate)
            {
                var errorName = GetDocSummary(propertyName);
                return string.Concat(SystemConstant.ErrorIcon, string.Format(errorTemplate, errorName));
            }
            if (string.IsNullOrWhiteSpace(UserName))
            {
                ErrorMessage = GetErrorMessage(nameof(UserName), SystemConstant.ErrorEmptyMessage);
                return false;
            }
            if (string.IsNullOrWhiteSpace(QQ))
            {
                ErrorMessage = GetErrorMessage(nameof(QQ), SystemConstant.ErrorEmptyMessage);
                return false;
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = GetErrorMessage(nameof(Password), SystemConstant.ErrorEmptyMessage);
                return false;
            }
            if (Password.Length < 6 || Password.Length > 20)
            {
                ErrorMessage = SystemConstant.ErrorPasswordLengthMessage;
                return false;
            }
            if (string.IsNullOrWhiteSpace(RepeatPassword))
            {
                ErrorMessage = GetErrorMessage(nameof(RepeatPassword), SystemConstant.ErrorEmptyMessage);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取属性名称
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        private string GetDocSummary(string propertyName)
        {
            return this.GetType().GetProperty(propertyName).GetXmlDocsSummary();
        }
    }
}
