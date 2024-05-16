using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Infrastructure.Config.HttpClients.HeepHelper
{
    public class HttpItem
    {
        private string _Method = "GET";

        private int _Timeout = 100000;

        private int _ReadWriteTimeout = 30000;

        private bool _KeepAlive = true;

        private string _Accept = "text/html, application/xhtml+xml, */*";

        private string _ContentType = "text/html";

        private string _UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";

        private PostDataType _PostDataType;

        private bool isToLower;

        private bool allowautoredirect;

        private int connectionlimit = 1024;

        private ResultType resulttype;

        private WebHeaderCollection header = new WebHeaderCollection();

        private bool _expect100continue;

        private ResultCookieType _ResultCookieType;

        private ICredentials _ICredentials = CredentialCache.DefaultCredentials;

        private DateTime? _IfModifiedSince;

        private IPEndPoint _IPEndPoint;

        public string URL { get; set; }

        public string Method
        {
            get
            {
                return _Method;
            }
            set
            {
                _Method = value;
            }
        }

        public int Timeout
        {
            get
            {
                return _Timeout;
            }
            set
            {
                _Timeout = value;
            }
        }

        public int ReadWriteTimeout
        {
            get
            {
                return _ReadWriteTimeout;
            }
            set
            {
                _ReadWriteTimeout = value;
            }
        }

        public string Host { get; set; }

        public bool KeepAlive
        {
            get
            {
                return _KeepAlive;
            }
            set
            {
                _KeepAlive = value;
            }
        }

        public string Accept
        {
            get
            {
                return _Accept;
            }
            set
            {
                _Accept = value;
            }
        }

        public string ContentType
        {
            get
            {
                return _ContentType;
            }
            set
            {
                _ContentType = value;
            }
        }

        public string UserAgent
        {
            get
            {
                return _UserAgent;
            }
            set
            {
                _UserAgent = value;
            }
        }

        public Encoding Encoding { get; set; }

        public PostDataType PostDataType
        {
            get
            {
                return _PostDataType;
            }
            set
            {
                _PostDataType = value;
            }
        }

        public string Postdata { get; set; }

        public byte[] PostdataByte { get; set; }

        public CookieCollection CookieCollection { get; set; }

        public string Cookie { get; set; }

        public string Referer { get; set; }

        public string CerPath { get; set; }

        public WebProxy WebProxy { get; set; }

        public bool IsToLower
        {
            get
            {
                return isToLower;
            }
            set
            {
                isToLower = value;
            }
        }

        public bool Allowautoredirect
        {
            get
            {
                return allowautoredirect;
            }
            set
            {
                allowautoredirect = value;
            }
        }

        public int Connectionlimit
        {
            get
            {
                return connectionlimit;
            }
            set
            {
                connectionlimit = value;
            }
        }

        public string ProxyUserName { get; set; }

        public string ProxyPwd { get; set; }

        public string ProxyIp { get; set; }

        public ResultType ResultType
        {
            get
            {
                return resulttype;
            }
            set
            {
                resulttype = value;
            }
        }

        public WebHeaderCollection Header
        {
            get
            {
                return header;
            }
            set
            {
                header = value;
            }
        }

        public Version ProtocolVersion { get; set; }

        public bool Expect100Continue
        {
            get
            {
                return _expect100continue;
            }
            set
            {
                _expect100continue = value;
            }
        }

        public X509CertificateCollection ClentCertificates { get; set; }

        public Encoding PostEncoding { get; set; }

        public ResultCookieType ResultCookieType
        {
            get
            {
                return _ResultCookieType;
            }
            set
            {
                _ResultCookieType = value;
            }
        }

        public ICredentials ICredentials
        {
            get
            {
                return _ICredentials;
            }
            set
            {
                _ICredentials = value;
            }
        }

        public int MaximumAutomaticRedirections { get; set; }

        public DateTime? IfModifiedSince
        {
            get
            {
                return _IfModifiedSince;
            }
            set
            {
                _IfModifiedSince = value;
            }
        }

        public IPEndPoint IPEndPoint
        {
            get
            {
                return _IPEndPoint;
            }
            set
            {
                _IPEndPoint = value;
            }
        }
    }
}
