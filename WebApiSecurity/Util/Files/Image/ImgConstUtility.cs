using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
namespace WebApi.Common.Util
{
    public class ImgConstUtility
    {

        #region 根据后缀名获取相应的图标
        public static string GetIcon(string str,int flg)
        {
            string strPath = "/fileimg/";
            string strIconName = "weizhi.png";
            string strBig = "";
            if (flg == 1)
            {
                strBig = "B";
            }
            switch (str)
            {
                case ".doc":
                case ".docx":
                    strIconName = "word.png";
                    break;
                case ".xls":
                    strIconName = "exl.png";
                    break;
                case ".txt":
                    strIconName = "txt.png";
                    break;
                case ".rar":
                    strIconName = "rar.png";
                    break;
                case ".pdf":
                    strIconName = "pdf.png";
                    break;
                case ".ppt":
                    strIconName = "ppt.png";
                    break;
            }
            return "<img  src=\""+strPath + strBig + strIconName+"\">";
        }
        public static string GetImageBig(string str)
        {
            string strBig = str.Replace("m_", "");

            return strBig;
        }
        #endregion
    }
}
