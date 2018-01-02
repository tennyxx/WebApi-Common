using WebApi.Common.Security.Interface;
using System.Security.Cryptography;
using System.Text;

namespace WebApi.Common.Security
{
    public class ApiMD5 : IApiEncrypt
    {
        //public string GetSignature(string DecryptString)
        //{
        //    MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        //    byte[] str1 = Encoding.UTF8.GetBytes(DecryptString);
        //    byte[] str2 = md5.ComputeHash(str1, 0, str1.Length);
        //    md5.Clear();
        //    (md5 as IDisposable).Dispose();
        //    return Convert.ToBase64String(str2);
        //}
        public string GetSignature(string DecryptString)
        {
            byte[] sor = Encoding.UTF8.GetBytes(DecryptString);
            MD5 md5 = MD5.Create();
            byte[] result = md5.ComputeHash(sor);
            StringBuilder strbul = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strbul.Append(result[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位

            }
            return strbul.ToString();
        }

    }
}