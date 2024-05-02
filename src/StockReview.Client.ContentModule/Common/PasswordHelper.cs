using HandyControl.Controls;
using System.Windows;

namespace StockReview.Client.ContentModule.Common
{
    /// <summary>
    /// 密码框帮助类
    /// </summary>
    public class PasswordHelper
    {
        private static Dictionary<PasswordBox, System.Windows.Controls.PasswordBox> PasswordDic =
            new Dictionary<PasswordBox, System.Windows.Controls.PasswordBox>();
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password",
                                                typeof(string),
                                                typeof(PasswordHelper),
                                                new PropertyMetadata(OnPropertyChanged));

        public static string GetPassword(DependencyObject d)
        {
            return (string)d.GetValue(PasswordProperty);
        }
        public static void SetPassword(DependencyObject d, string value)
        {
            d.SetValue(PasswordProperty, value);
        }

        /// <summary>
        /// 是否监测
        /// </summary>
        public static readonly DependencyProperty IsAttachProperty =
            DependencyProperty.RegisterAttached("IsAttach",
                                                typeof(bool),
                                                typeof(PasswordBoxAttach),
                                                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, OnIsAttachChanged));

        private static void OnIsAttachChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox pwdBox)
            {
                PasswordDic[pwdBox] = null;
            }
            if (d is System.Windows.Controls.PasswordBox passwordBox)
            {
                if (e.NewValue is bool boolValue)
                {
                    if (boolValue)
                    {
                        passwordBox.PasswordChanged += PasswordChanged;
                        var dic = PasswordDic.FirstOrDefault(t => t.Value == null);
                        PasswordDic[dic.Key] = passwordBox;
                    }
                    else
                    {
                        passwordBox.PasswordChanged -= PasswordChanged;
                    }
                }
            }
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PasswordBox)d).Password = e.NewValue.ToString();
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.PasswordBox passwordBox)
            {
                var pwdBox = PasswordDic.Where(t => t.Value == passwordBox).Select(t => t.Key).FirstOrDefault();
                if (pwdBox != null)
                {
                    SetPassword(pwdBox, passwordBox.Password);
                }
            }
        }

        public static void SetIsAttach(DependencyObject element, bool value) => element.SetValue(IsAttachProperty, value);

        public static bool GetIsAttach(DependencyObject element) => (bool)element.GetValue(IsAttachProperty);

    }
}
