
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
        /// 全局用户信息
        /// </summary>
        public const string GlobalUserName = nameof(GlobalUserName);

        /// <summary>
        /// 菜单树区域
        /// </summary>
        public const string TreeMenuViewRegion = nameof(TreeMenuViewRegion);
        /// <summary>
        /// 修改用户对话框窗口
        /// </summary>
        public const string ModifyUserDialogView = nameof(ModifyUserDialogView);
        public const string AddUserDialogView = nameof(AddUserDialogView);
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
        /// 错误用户提示消息
        /// </summary>
        public const string ErrorNotExistUserNameMessage = "用户信息不存在！";

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
        /// 标记
        /// </summary>
        public const string headerGrowl = nameof(headerGrowl);

        /// <summary>
        /// 异常空消息
        /// </summary>
        public const string ErrorEmptyMessage = "{0}不能为空！";
        public const string RegisterSuccess = "注册成功！";
        public const string RegisterWindow = "注册窗口";
        public const string ForgotPasswordSuccess = "找回密码成功！";
        public const string ForgotPasswordWindow = "找回密码窗口";
        public const string UpdatePasswordSuccess = "修改密码成功！";
        public const string UpdatePasswordWindow = "修改密码窗口";
        /// <summary>
        /// 异常存在消息
        /// </summary>
        public const string ErrorExistMessage = "{0}已存在！";
        public const string ErrorInconsistentVerificationCode = "验证码输入不正确！";
        /// <summary>
        /// 密码错误
        /// </summary>
        public const string ErrorInconsistentConfirmPwd = "新密码与确认密码不一致！";

        public const string ErrorInconsistentUserNameOrQQ = "登录信息和密码不匹配！";
        /// <summary>
        /// 用户名长度
        /// </summary>
        public const string ErrorUserNameLengthMessage = "登录名长度不在指定范围内！";

        /// <summary>
        /// 密码长度
        /// </summary>
        public const string ErrorPasswordLengthMessage = "密码长度不在指定范围内";

        /// <summary>
        /// QQ不合法消息
        /// </summary>
        public const string ErrorQQRuleMessage = "QQ号码不符合实际规则！";
        /// <summary>
        /// phone不合法消息
        /// </summary>
        public const string ErrorPhoneRuleMessage = "手机号码不符合实际规则！";
        /// <summary>
        /// 验证码
        /// </summary>
        public const string VerificationCode = nameof(VerificationCode);

        public const string ErrorDataSumbit = "数据提交失败！";

        public const string SuccessDataSumbit = "数据保存成功!";
        public const string ErrorDeleteData = "数据删除失败!";
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
        /// menu 配置文件
        /// </summary>
        public const string MenuJson = "Menu.json";
        /// <summary>
        /// 看板key
        /// </summary>
        public const string BulletinBoardKey = nameof(BulletinBoardKey);
        /// <summary>
        /// 情绪明细key
        /// </summary>
        public const string EmotionDetailKey = nameof(EmotionDetailKey);
        /// <summary>
        /// 选中日期
        /// </summary>
        public const string StockSelectedDayKey = nameof(StockSelectedDayKey);
        /// <summary>
        /// 过滤日期
        /// </summary>
        public const string StockFilterDaysKey = nameof(StockFilterDaysKey);
        /// <summary>
        /// 没有菜单
        /// </summary>
        public const string ErrorMenuNotExist = "菜单信息不存在！";
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
        /// <summary>
        /// 忘记密码路由
        /// </summary>
        public const string ForgotPasswordRoute = "v1/stockreview/account/forgot-password";
        /// <summary>
        /// 修改密码路由
        /// </summary>
        public const string UpdatePasswordRoute = "v1/stockreview/account/update-password";
        /// <summary>
        /// 菜单路由
        /// </summary>
        public const string MenuRoute = "v1/stockreview/account/menu";
        /// <summary>
        /// 用户列表集合
        /// </summary>
        public const string UsersRoute = "v1/stockreview/user/list";
        public const string AddUserRoute = "v1/stockreview/user/add";
        public const string UpdateUserRoleRoute = "v1/stockreview/user/update-role";
        public const string DeleteUserRoute = "v1/stockreview/user/delete";
        public const string UsersRouteQuery = "v1/stockreview/user/list?keyword={0}";
        public const string BulletinBoardRoute = "v1/stockreview/stockOutlook/bulletin-board";
        public const string EmotionDetailRoute = "v1/stockreview/stockOutlook/emotion";
        public const string TodayRoute = "v1/stockreview/stockOutlook/today";

        public const string TodayLonghuVipUrl = "https://apphq.longhuvip.com/w1/api/index.php";
        public const string HistoryLonghuVipUrl = "https://apphis.longhuvip.com/w1/api/index.php";
        public const string SpecialLonghuVipUrl = "https://apphwhq.longhuvip.com/w1/api/index.php";
        public const string DeviceID = "929417d2-bbbb-3418-b83a-827effe0b778";
        public const string apivW36 = "w36";
        public const string apivW31 = "w31";
        public const string HomeDingPan = nameof(HomeDingPan);
        public const string VerSion51404 = "5.14.0.4";
        public const string VerSion57012 = "5.7.0.12";
        public const string PhoneOSNew = "1";
        public const string UserAgent = "Opera/9.27 (Linux; U; Android 8.1.0; zh-cn; BLA-AL00 Build/HUAWEIBLA-AL00) Chrome/57.0.2987.132 Mobile Safari/437.26";
        public const string HqstatsJqkaUrl = "http://hqstats.10jqka.com.cn";
        public const string TongHuaXValue = "涨停,≥7%,5～7%,3～5%,0～3%,0,-0～3%,-3～5%,-5～7%,≥-7%,跌停";
    }
}
