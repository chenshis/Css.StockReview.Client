using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Infrastructure.Config.ExtensionMethods
{
    public static class SecretExtension
    {
        #region MD5

        /// <summary>
        ///     Md5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMd5(this string str)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(str));
            var sBuilder = new StringBuilder();
            foreach (var t in data) sBuilder.Append(t.ToString("x2"));
            return sBuilder.ToString();
        }

        #endregion

    }
}
