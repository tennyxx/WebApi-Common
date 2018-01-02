using WebApi.Common.Security.Interface;
using System;
using System.Security.Cryptography;
using System.Text;

namespace WebApi.Common.Security
{
    public class ApiRSA : IApiEncrypt
    {
        private string _xmlPrivateKey;

        /// <summary>
        /// 私钥
        /// </summary>
        /// <param name="xmlPrivateKey"></param>
        public ApiRSA(string xmlPrivateKey)
        {
            _xmlPrivateKey = xmlPrivateKey;
        }

        public string GetSignature(string DecryptString)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(_xmlPrivateKey);
            byte[] rgb = Convert.FromBase64String(DecryptString);
            byte[] buffer = provider.Decrypt(rgb, false);
            return new UnicodeEncoding().GetString(buffer).ToUpper();
        }
    }
}