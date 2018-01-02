using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Common.Security.Interface
{
    /// <summary>
    /// 加密接口
    /// </summary>
    public interface IApiEncrypt
    {
        /// <summary>
        /// 获取加密后数据
        /// </summary>
        /// <param name="DecryptString">加密前数据</param>
        /// <returns></returns>
        string GetSignature(string DecryptString);
    }
}
