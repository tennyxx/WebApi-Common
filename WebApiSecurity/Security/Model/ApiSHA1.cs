using WebApi.Common.Security.Interface;
using System;
using System.Security.Cryptography;
using System.Text;

namespace WebApi.Common.Security
{
    public class ApiSHA1 : IApiEncrypt
    {

        public string GetSignature(string DecryptString)
        {

            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] str1 = Encoding.UTF8.GetBytes(DecryptString);
            byte[] str2 = sha1.ComputeHash(str1);
            sha1.Clear();
            (sha1 as IDisposable).Dispose();

            return Convert.ToBase64String(str2);
        }
    }
}