using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebApi.Common.Security
{
    public class SecurityConfiguration
    {
        private bool _CheckSignature = true;
        /// <summary>
        /// 是否校验签名
        /// </summary>
        public bool CheckSignature
        {
            get
            {
                return _CheckSignature;
            }
            set
            {
                _CheckSignature = value;
            }
        }

        private List<string> _AllowedIPAddress = new List<string>();
        /// <summary>
        /// 允许访问的IP地址
        /// </summary>
        public List<string> AllowedIPAddress
        {
            get
            {
                return _AllowedIPAddress;
            }
            set
            {
                _AllowedIPAddress = value;
            }
        }

        /// <summary>
        /// 是否校验IP地址
        /// </summary>
        public bool CheckIPAddress
        {
            get
            {
                return !(_AllowedIPAddress == null || _AllowedIPAddress.Count == 0);
            }
        }

        public SecurityConfiguration()
        {
        }

        public SecurityConfiguration(string appkey)
        {
            if (string.IsNullOrEmpty(appkey))
                return;
            try
            {
                SecurityConfiguration config = new SecurityConfiguration();
                //config = JsonConvert.DeserializeObject<SecurityConfiguration>(configString);
                this.CheckSignature = config.CheckSignature;
                this.AllowedIPAddress = config.AllowedIPAddress;
            }
            catch
            {
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(new
            {
                CheckSignature = _CheckSignature,
                AllowedIPAddress = _AllowedIPAddress
            });
        }
    }
}