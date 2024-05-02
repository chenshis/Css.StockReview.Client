using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using StockReview.Infrastructure.Config;
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
        public UserManagementViewModel(IUnityContainer unityContainer,
                                       IRegionManager regionManager,
                                       IDialogService dialogService)
            : base(unityContainer, regionManager)
        {
            this.PageTitle = "系统用户管理";
            this._dialogService = dialogService;
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
