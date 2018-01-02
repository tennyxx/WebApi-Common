using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Web.DynamicData;

namespace WebApi.Common.Util
{
    /// <summary>
    /// DataTable DataReader转实体集合 
    /// </summary>
    public sealed class DataHelper
    {
        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(DataTable dt) where T : new()
        {
            using (dt)
            {
                if (Equals(null, dt))
                    return null;

                var prList = new List<PropertyInfo>();

                Array.ForEach(typeof(T).GetProperties(), p =>
                {
                    if (dt.Columns.IndexOf(p.Name) > -1)
                    {
                        prList.Add(p);
                    }
                });

                var list = new List<T>();
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var t = new T();
                    prList.ForEach(
                        p => p.SetValue(t, Equals(DBNull.Value, dt.Rows[i][p.Name]) ? null : dt.Rows[i][p.Name], null));
                    list.Add(t);
                }

                return list;
            }
        }

        /// <summary>
        /// DataRow转实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        public static T ToEntity<T>(DataRow row) where T : new()
        {
            var t = new T();
            var prList = new List<PropertyInfo>();

            Array.ForEach(typeof(T).GetProperties(), p =>
            {
                if (row.Table.Columns.IndexOf(p.Name) > -1)
                {
                    prList.Add(p);

                }
            });

            prList.ForEach(
                        p => p.SetValue(t, Equals(DBNull.Value, row[p.Name]) ? null : row[p.Name], null));

            return t;
        }
        /// 获得单个实体  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static T Entity<T>(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return default(T);
            }
            T entity = default(T);
            foreach (DataRow dr in dt.Rows)
            {
                entity = Activator.CreateInstance<T>();
                PropertyInfo[] pis = entity.GetType().GetProperties();
                foreach (PropertyInfo pi in pis)
                {
                    if (dt.Columns.Contains(pi.Name))
                    {
                        if (!pi.CanWrite)
                        {
                            continue;
                        }
                        if (dr[pi.Name] != DBNull.Value)
                        {
                            Type t = pi.PropertyType;

                            pi.SetValue(entity, dr[pi.Name], null);

                        }
                    }
                }
            }
            return entity;
        }
        /// <summary>
        /// DataReader转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(IDataReader reader) where T : new()
        {
            using (reader)
            {
                if (Equals(null, reader))
                    return null;

                var list = new List<T>();
                var columsStr = new StringBuilder(",");
                var prList = new List<PropertyInfo>();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    columsStr.AppendFormat("{0},", reader.GetName(i));
                }
                Array.ForEach(typeof(T).GetProperties(), p =>
                {
                    if (columsStr.ToString().IndexOf(string.Format(",{0},", p.Name),
                        StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        prList.Add(p);
                    }
                });
                while (reader.Read())
                {
                    var t = new T();
                    foreach (var property in prList)
                    {
                        property.SetValue(t, Equals(DBNull.Value, reader[property.Name]) ? null : reader[property.Name],
                            null);
                    }

                    list.Add(t);
                }

                if (!reader.IsClosed)
                    reader.Close();

                return list;
            }
        }

        /// <summary>
        /// DataReader转 dynamic List
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<DynamicEntity> ToList(IDataReader reader)
        {
            using (reader)
            {
                if (Equals(null, reader))
                    return null;

                var list = new List<DynamicEntity>();
                while (reader.Read())
                {
                    var entity = new DynamicEntity();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var key = reader.GetName(i);
                        if (entity.ContainsKey(key))
                            continue;

                        var val = reader.GetValue(i);
                        entity[key] = val;
                    }

                    list.Add(entity);
                }

                if (!reader.IsClosed)
                    reader.Close();

                return list;
            }
        }

        /// <summary>  
        /// 反射得到实体类的字段名称和值  
        /// var dict = GetProperties(model);  
        /// </summary>  
        /// <typeparam name="T">实体类</typeparam>  
        /// <param name="t">实例化</param>  
        /// <returns></returns>  
        public static Dictionary<object, object> GetProperties<T>(T t)
        {
            var ret = new Dictionary<object, object>();
            if (t == null) { return null; }
            PropertyInfo[] properties = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length <= 0) { return null; }
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(t, null);
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    ret.Add(name, value);
                }
            }
            return ret;
        }


        /// <summary>
        /// list转datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static DataTable ListToDt<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }


    }
}