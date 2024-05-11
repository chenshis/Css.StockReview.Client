using HandyControl.Data;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Services.Dialogs;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using StockReview.Infrastructure.Config;
using System.Windows.Input;

namespace StockReview.Client.ContentModule.ViewModels
{
    /// <summary>
    /// 修改用户窗体视图模型
    /// </summary>
    public class ModifyUserDialogViewModel : DialogAwareViewModelBase
    {
        private readonly ILoginApiService _loginApiService;
        public ModifyUserDialogViewModel(ILoginApiService loginApiService)
        {
            this._loginApiService = loginApiService;
        }

        public override string Title { get; set; } = "用户信息编辑";

        private UserDto _userModel;


        /// <summary>
        /// 用户模型
        /// </summary>
        public UserDto UserModel
        {
            get { return _userModel; }
            set { SetProperty(ref _userModel, value); }
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


        public override void OnDialogOpened(IDialogParameters parameters)
        {
            UserModel = parameters.GetValue<UserDto>(nameof(UserDto));
            if (UserModel != null && UserModel.Role == 0)
            {
                UserModel.Role = Domain.Entities.RoleEnum.Ordinary;
            }
        }

        public ICommand ConfirmCommand { get => new DelegateCommand(SetRoleEditActive); }

        private void SetRoleEditActive()
        {
            var apiResponse = _loginApiService.UpdateUserRole(new UpdateUserRoleRequestDto
            {
                UserName = UserModel.UserName,
                Role = UserModel.Role,
                Expires = UserModel.Expires
            });
            if (apiResponse.Code != 0)
            {
                ErrorMessage = apiResponse.Msg;
                return;
            }
            if (apiResponse.Data != true)
            {
                ErrorMessage = SystemConstant.ErrorDataSumbit;
                return;
            }
            CloseCommand.Execute(ButtonResult.OK);
        }
    }
}
