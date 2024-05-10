using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using StockReview.Infrastructure.Config;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unity;

namespace StockReview.Client.ContentModule.ViewModels
{
    public class UserManagementViewModel : NavigationAwareViewModelBase
    {
        /// <summary>
        /// 对话框服务
        /// </summary>
        private readonly IDialogService _dialogService;
        private readonly ILoginApiService _loginApiService;

        public UserManagementViewModel(IUnityContainer unityContainer, IRegionManager regionManager,
                                       IDialogService dialogService, ILoginApiService loginApiService)
            : base(unityContainer, regionManager)
        {
            this.PageTitle = "系统用户管理";
            this._dialogService = dialogService;
            this._loginApiService = loginApiService;
            Init();
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        public ObservableCollection<UserDto> users { get; set; } = new ObservableCollection<UserDto>();


        private void Init()
        {
            var apiReponse = _loginApiService.GetUsers(null);
            if (apiReponse.Code != 0)
            {
                HandyControl.Controls.Growl.Error(apiReponse.Msg);
                return;
            }
            if (apiReponse.Data == null)
            {
                return;
            }
            int i = 0;
            foreach (var user in apiReponse.Data)
            {
                i++;
                user.Index = i;
                users.Add(user);
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
        /// 新增命令
        /// </summary>
        public ICommand AddCommand =>
            new DelegateCommand(() => SetAddActive()).ObservesCanExecute(() => IsEnable);

        /// <summary>
        /// 新增
        /// </summary>
        private void SetAddActive()
        {
            IsEnable = false;
            _dialogService.ShowDialog(SystemConstant.ModifyUserDialogView);
            IsEnable = true;
        }
    }
}
