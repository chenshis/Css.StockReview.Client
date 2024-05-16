using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Infrastructure.Config.HttpClients.HeepHelper
{
    public class HttpResult
    {
        private string _html = string.Empty;

        public string Cookie { get; set; }

        public CookieCollection CookieCollection { get; set; }

        public string Html
        {
            get
            {
                return _html;
            }
            set
            {
                _html = value;
            }
        }

        public byte[] ResultByte { get; set; }

        public WebHeaderCollection Header { get; set; }

        public string StatusDescription { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string ResponseUri { get; set; }

        public string RedirectUrl
        {
            get
            {
                try
                {
                    if (Header != null && Header.Count > 0 && Header.AllKeys.Any((k) => k.ToLower().Contains("location")))
                    {
                        string text = Header["location"].ToString().Trim();
                        string text2 = text.ToLower();
                        if (!string.IsNullOrWhiteSpace(text2) && !text2.StartsWith("http://") && !text2.StartsWith("https://"))
                        {
                            text = new Uri(new Uri(ResponseUri), text).AbsoluteUri;
                        }
                        return text;
                    }
                }
                catch
                {
                }
                return string.Empty;
            }
        }
    }
}
