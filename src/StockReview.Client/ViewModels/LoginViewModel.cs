using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StockReview.Client.ViewModels
{
    /// <summary>
    /// 登录视图模型
    /// </summary>
    public class LoginViewModel : BindableBase
    {

        private string _userName = "Test";
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

        /// <summary>
        /// 是否启用命令
        /// </summary>
        public bool _isEnable = true;

        /// <summary>
        /// 登录命令
        /// </summary>
        public ICommand LoginCommand
        {
            get => new DelegateCommand(SetLogin);
        }

        /// <summary>
        /// 登录方法
        /// </summary>
        private void SetLogin()
        {
           // todo 登录逻辑
        }
    }
}
