
namespace StockReview.Infrastructure.Config
{
    /// <summary>
    /// 系统常量
    /// </summary>
    public static class SystemConstant
    {
        public const int Zero = 0;

        /// <summary>
        /// 龙头晋级视图
        /// </summary>
        public const string LeadingGroupPromotionView = nameof(LeadingGroupPromotionView);
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
        /// 主体头部视图
        /// </summary>
        public const string MainHeaderView = nameof(MainHeaderView);
        /// <summary>
        /// 主体头部区域
        /// </summary>
        public const string MainHeaderRegion = nameof(MainHeaderRegion);
        /// <summary>
        /// 主体内容区域
        /// </summary>
        public const string MainContentRegion = nameof(MainContentRegion);
        /// <summary>
        /// 菜单树
        /// </summary>
        public const string TreeMenuView = nameof(TreeMenuView);
        /// <summary>
        /// 菜单树区域
        /// </summary>
        public const string TreeMenuViewRegion = nameof(TreeMenuViewRegion);
        /// <summary>
        /// 修改用户对话框窗口
        /// </summary>
        public const string ModifyUserDialogView = nameof(ModifyUserDialogView);
        /// <summary>
        /// 股票看盘头部视图
        /// </summary>
        public const string StockOutlookHeaderView = nameof(StockOutlookHeaderView);
        /// <summary>
        /// 股票看盘头部视图区域
        /// </summary>
        public const string StockOutlookHeaderViewRegion = nameof(StockOutlookHeaderViewRegion);
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
        /// 错误用户或密码
        /// </summary>
        public const string ErrorUserOrPasswordMessage = "用户名或密码不正确！";

        /// <summary>
        /// 重复密码提示消息
        /// </summary>
        public const string ErrorEmptyRepeatPasswordMessage = "重复密码不能为空！";

        /// <summary>
        /// 刷新token异常
        /// </summary>
        public const string ErrorRefreshTokenFailMessage = "token刷新失败，请重新登录！";

        /// <summary>
        /// 异常空消息
        /// </summary>
        public const string ErrorEmptyMessage = "{0}不能为空！";

        /// <summary>
        /// 异常存在消息
        /// </summary>
        public const string ErrorExistMessage = "{0}已存在！";

        /// <summary>
        /// 验证码
        /// </summary>
        public const string VerificationCode = nameof(VerificationCode);

        /// <summary>
        /// 401 说明
        /// </summary>
        public const string Unauthorized = "你没有请求服务的访问权限，请重新登录账户！";

        /* jwt 系统常量 */
        public const string JwtSecurityKey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDI2a2EJ7m872v0afyoSDJT2o1+SitIeJSWtLJU8/Wz2m7gStexajkeD+Lka6DSTy8gt9UwfgVQo6uKjVLG5Ex7PiGOODVqAEghBuS7JzIYU5RvI543nNDAPfnJsas96mSA7L/mD7RTE2drj6hf3oZjJpMPZUQI/B1Qjb5H3K3PNwIDAQAB";
        public const string JwtAudience = "http://localhost:5245";
        public const string JwtIssuer = "http://localhost:5245";
        public const string JwtActor = "This is a stock trading function, welcome to use!";

        /// <summary>
        /// 默认连接
        /// </summary>
        public const string DefaultConnection = nameof(DefaultConnection);
        /// <summary>
        /// appsetting 配置
        /// </summary>
        public const string AppSettings = "appsettings.json";
        /// <summary>
        /// 股市服务地址
        /// </summary>
        public const string StockServerUrl = nameof(StockServerUrl);

        /// <summary>
        /// 登录路由
        /// </summary>
        public const string LoginRoute = "v1/stockreview/account/login";
        /// <summary>
        /// 刷新token路由
        /// </summary>
        public const string RefreshTokenRoute = "v1/stockreview/account/refresh-token";
        /// <summary>
        /// 注册路由
        /// </summary>
        public const string RegisterRoute = "v1/stockreview/account/register";
    }
}
