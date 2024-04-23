using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StockReview.Client.ViewModels
{
    /// <summary>
    /// 验证码视图模型
    /// </summary>
    public class VerificationCodeViewModel : BindableBase
    {

        public VerificationCodeViewModel()
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


        private Random _random = new Random();
        private void GenerateCaptcha()
        {
            // 生成随机验证码字符串
            string captchaText = GenerateRandomString(4);

            // 创建验证码图片
            Bitmap captchaBitmap = new Bitmap(100, 40);
            using (Graphics g = Graphics.FromImage(captchaBitmap))
            {
                g.Clear(Color.White);
                Font font = new Font("Arial", 20, System.Drawing.FontStyle.Bold);
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

    }
}
