using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public RegisterViewModel()
        {
            GenerateCaptcha();
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

    }
}

