
namespace StockReview.Infrastructure.Config
{
    /// <summary>
    /// 系统常量
    /// </summary>
    public static class SystemConstant
    {
        /// <summary>
        /// 注册视图
        /// </summary>
        public const string RegisterView = nameof(RegisterView);
        /// <summary>
        /// 忘记密码视图
        /// </summary>
        public const string ForgotPasswordView = nameof(ForgotPasswordView);
        /// <summary>
        /// 修改密码视图
        /// </summary>
        public const string UpdatePasswordView = nameof(UpdatePasswordView);

        /// <summary>
        /// 异常图标
        /// </summary>
        public const string ErrorIcon = "\xe621 ";

        /// <summary>
        /// 错误用户提示消息
        /// </summary>
        public const string ErrorEmptyUserNameMessage = "用户名不能为空！";

        /// <summary>
        /// 出错密码提示消息
        /// </summary>
        public const string ErrorEmptyPasswordMessage = "密码不能为空！";

        /// <summary>
        /// 重复密码提示消息
        /// </summary>
        public const string ErrorEmptyRepeatPasswordMessage = "重复密码不能为空！";

        /// <summary>
        /// 异常空消息
        /// </summary>
        public const string ErrorEmptyMessage = "{0}不能为空！";

        /// <summary>
        /// 验证码
        /// </summary>
        public const string VerificationCode = nameof(VerificationCode);
    }
}
