﻿
namespace StockReview.Infrastructure.Config
{
    /// <summary>
    /// 系统常量
    /// </summary>
    public static class SystemConstant
    {
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

        /* IdentityServer4 系统常量 */
        public const string IdentityServerClient = "stock_review_client";
        public const string IdentityServerScope = "stock_review_scope";
        public const string IdentityServerResource = "stock_review_resource";
        public const string IdentityServerScopeDisplayName = "股市客户端请求作用域";
        public const string IdentityServerResourceDisplayName = "股市客户端请求资源";
        public const string IdentityServerSecret = "Css.StockReview.Client-StockReview.Server";

        /// <summary>
        /// 默认连接
        /// </summary>
        public const string DefaultConnection = nameof(DefaultConnection);

    }
}
