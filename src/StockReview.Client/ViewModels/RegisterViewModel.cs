using Microsoft.Extensions.Caching.Memory;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using StockReview.Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Namotion.Reflection;
using System.Windows.Input;

namespace StockReview.Client.ViewModels
{
    public class RegisterViewModel : BindableBase, IDialogAware
    {
        public string Title => "欢迎注册";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
        private readonly IMemoryCache _memoryCache;
        public RegisterViewModel(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            GenerateCaptcha();
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
        /// 重复密码
        /// </summary>
        public string RepeatPassword
        {
            get { return _repeatPassword; }
            set { SetProperty(ref _repeatPassword, value); }
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

        private string _verificationCode;
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerificationCode
        {
            get { return _verificationCode; }
            set { SetProperty(ref _verificationCode, value); }
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



        private System.Windows.Media.Imaging.BitmapImage _imageSource;

        /// <summary>
        /// 图像源
        /// </summary>
        public System.Windows.Media.Imaging.BitmapImage ImageSource
        {
            get { return _imageSource; }
            set { SetProperty(ref _imageSource, value); }
        }

        /// <summary>
        /// 验证码命令
        /// </summary>
        public ICommand VerificationCodeCommand { get => new DelegateCommand(() => GenerateCaptcha()); }

        #region 生成验证码

        private Random _random = new Random();

        private void GenerateCaptcha()
        {
            // 生成随机验证码字符串
            string captchaText = GenerateRandomString(4);
            // 创建验证码图片
            Bitmap captchaBitmap = new Bitmap(100, 40);
            using (Graphics g = Graphics.FromImage(captchaBitmap))
            {
                Font font = new Font("Arial", 16, System.Drawing.FontStyle.Bold);
                Brush brush = new SolidBrush(Color.Black);
                PointF pointF = new PointF(2f, 2f);
                g.DrawString(captchaText, font, brush, pointF);
            }
            // 缓存验证码
            _memoryCache.Set(SystemConstant.VerificationCode, captchaText);
            // 将Bitmap转换为BitmapSource并显示在Image控件上
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                captchaBitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                ImageSource = bitmapImage;
            }

        }
        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        #endregion


        /// <summary>
        /// 注册命令
        /// </summary>
        public ICommand RegisterCommand
        {
            get => new DelegateCommand(() => SetRegister()).ObservesCanExecute(() => IsEnable);
        }

        private void SetRegister()
        {
            IsEnable = false;
            if (!Valiedate())
            {
                IsEnable = true;
                return;
            }

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
            if (string.IsNullOrWhiteSpace(RepeatPassword))
            {
                ErrorMessage = GetErrorMessage(nameof(RepeatPassword), SystemConstant.ErrorEmptyMessage);
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
            if (string.IsNullOrWhiteSpace(VerificationCode))
            {
                ErrorMessage = GetErrorMessage(nameof(VerificationCode), SystemConstant.ErrorEmptyMessage);
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

