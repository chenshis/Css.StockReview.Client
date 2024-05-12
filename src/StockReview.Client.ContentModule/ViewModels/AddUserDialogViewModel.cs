using Namotion.Reflection;
using Prism.Commands;
using Prism.Services.Dialogs;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using StockReview.Domain.Entities;
using StockReview.Infrastructure.Config;
using System.Windows.Controls;
using System.Windows.Input;

namespace StockReview.Client.ContentModule.ViewModels
{
    /// <summary>
    /// 添加用户视图模型
    /// </summary>
    public class AddUserDialogViewModel : DialogAwareViewModelBase
    {

        private readonly ILoginApiService _loginApiService;

        public AddUserDialogViewModel(ILoginApiService loginApiService)
        {
            this._loginApiService = loginApiService;
        }

        public override string Title { get; set; } = "用户信息新增";

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

        private string _contacts;
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts
        {
            get { return _contacts; }
            set { SetProperty(ref _contacts, value); }
        }

        private string _phone;
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
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

        private RoleEnum _role;
        /// <summary>
        /// 角色;1，2，4，8 免费用户 普通用户 vip 管理员
        /// </summary>

        public RoleEnum Role
        {
            get { return _role; }
            set { SetProperty(ref _role, value); }
        }

        private string _expires;

        /// <summary>
        /// 过期时间
        /// </summary>
        public string Expires
        {
            get { return _expires; }
            set { SetProperty(ref _expires, value); }
        }


        public ICommand ConfirmCommand { get => new DelegateCommand<UserControl>((u => SetAddUserActive(u))).ObservesCanExecute(() => IsEnable); }

        private void SetAddUserActive(UserControl control)
        {
            IsEnable = false;
            HandyControl.Controls.PasswordBox userPwd = null;
            userPwd = control.FindName(nameof(userPwd)) as HandyControl.Controls.PasswordBox;
            Password = userPwd.Password;

            DateTime? expiresDate = null;
            if (!string.IsNullOrWhiteSpace(Expires))
            {
                if (DateTime.TryParse(Expires, out DateTime dateTime))
                {
                    expiresDate = dateTime;
                }
            }
            if (!Valiedate())
            {
                IsEnable = true;
                return;
            }


            var apiResponse = _loginApiService.AddUser(new UserRequestDto
            {
                UserName = UserName,
                Password = Password,
                Contacts = Contacts,
                Phone = Phone,
                QQ = QQ,
                Role = Role,
                Expires = expiresDate
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

            CloseCommand.Execute(ButtonResult.OK);
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
            if (string.IsNullOrWhiteSpace(Contacts))
            {
                ErrorMessage = GetErrorMessage(nameof(Contacts), SystemConstant.ErrorEmptyMessage);
                return false;
            }
            if (string.IsNullOrWhiteSpace(Phone))
            {
                ErrorMessage = GetErrorMessage(nameof(Phone), SystemConstant.ErrorEmptyMessage);
                return false;
            }
            if (string.IsNullOrWhiteSpace(QQ))
            {
                ErrorMessage = GetErrorMessage(nameof(QQ), SystemConstant.ErrorEmptyMessage);
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
