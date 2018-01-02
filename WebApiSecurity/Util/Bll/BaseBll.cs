using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace WebApi.Common.Util
{
    public abstract class BaseBll<T> where T : class, new()
    {
        private static T instance = default(T);
        private static object _lockObj = new object();

        /// <summary>
        /// 以单例方式获取对象实例(非线程安全的)
        /// </summary>
        /// <returns></returns>
        public static T GetInstance()
        {
            if (instance == null)
            {
                instance = new T();
            }

            return instance;
        }

        /// <summary>
        /// 以单例方式获取对象实例(线程安全的)
        /// </summary>
        /// <returns></returns>
        public static T GetThreadSafeIntance()
        {
            if (instance == null)
            {
                lock (_lockObj)
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                }
            }

            return instance;
        }

    }
}
