using System;
using System.Text;
using System.IO;
using System.Web;
using System.Net;

namespace WebApi.Common.Util
{
    /// <summary>
    /// 文件操作辅助类
    /// </summary>
    public class FileUtility
    {
        public enum FileType
        {
            Image = 1,
            Video = 2,
            Audio = 3,
            RAR = 4,
            Other = 5,
            Flash = 6
        }

        #region 文件
        /// <summary>
        /// 写入指定的内容到指定的文件中
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="content">文件内容</param>
        /// <param name="encoder">编码:[gb2312][utf-8]</param>
        /// <param name="isAppend">是否为追加方式</param>
        public static void WriteFile(string filePath, string content, string encoder, bool isAppend)
        {
            try
            {
                Encoding encoding = Encoding.GetEncoding(encoder);

                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }
                using (StreamWriter sw = new StreamWriter(filePath, isAppend, encoding))
                {
                    sw.Write(content);
                }
            }
            catch (Exception ee)
            {
                throw new ApplicationException(ee.Message.ToString());
            }
        }

        //记录错误日志
        public static void WriteErrorLog(string strErrorInfo, string strErrorType)
        {
            string strCurrentPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\errorLog\";
            if (!string.IsNullOrEmpty(strErrorType.Trim()))
            {
                strCurrentPath += strErrorType.Trim() + "\\";
            }

            if (!Directory.Exists(strCurrentPath))
            {
                Directory.CreateDirectory(strCurrentPath);
            }
            string strLogFileName = strCurrentPath + "ErrLog.txt";
            using (StreamWriter sw = new StreamWriter(strLogFileName, true))
            {
                sw.WriteLine(strErrorInfo);
            }

        }

        public static string ReadFile(string filePath, string encoder)
        {
            try
            {
                Encoding encoding = Encoding.GetEncoding(encoder);


                using (StreamReader sw = new StreamReader(filePath, encoding))
                {
                    return sw.ReadToEnd();
                }
            }
            catch (Exception ee)
            {
                throw new ApplicationException(ee.Message.ToString());
            }
        }



        #endregion

        #region 上传文件

        public static string UpLoad(System.Web.UI.WebControls.FileUpload fup, string savePath, string saveFileName)
        {
            string reMsg = "Error: 未知错误！";

            if (fup.HasFile)
            {
                reMsg = CreateDirectory(savePath);
                if (reMsg != "") return reMsg;

                try
                {
                    string fullPath = Path.Combine(savePath, saveFileName);
                    fup.SaveAs(fullPath);
                    reMsg = saveFileName;
                }
                catch (Exception ex)
                {
                    reMsg = "Error: " + ex.Message;
                }
            }
            return reMsg;
        }

        public static string UpLoadJPEG(HttpPostedFile fup, string savePath, string saveFileName, int width, int height)
        {
            string reMsg = "Error: 未知错误！";

            reMsg = CreateDirectory(savePath);
            if (reMsg != "") return reMsg;
            string fullPath = Path.Combine(savePath, saveFileName);
            try
            {
                System.Drawing.Image im = System.Drawing.Image.FromStream(fup.InputStream);
                if (width == -1 && height == -1)
                {

                    width = im.Width;
                    height = im.Height;
                }
                PictureUtility.ComPressJPEG(System.Drawing.Image.FromStream(fup.InputStream), width, height).Save(fullPath);
                reMsg = saveFileName;
            }
            catch (Exception ex)
            {
                reMsg = "Error: " + ex.Message;
            }

            return reMsg;
        }

        public static string UpLoadJPEG(System.Web.UI.WebControls.FileUpload fup, string savePath, string saveFileName, int width, int height)
        {
            string reMsg = "Error: 未知错误！";
            if (fup.HasFile)
            {
                reMsg = CreateDirectory(savePath);
                if (reMsg != "") return reMsg;
                string fullPath = Path.Combine(savePath, saveFileName);
                try
                {
                    System.Drawing.Image im = System.Drawing.Image.FromStream(fup.PostedFile.InputStream);
                    if (width == -1 && height == -1)
                    {

                        width = im.Width;
                        height = im.Height;
                    }
                    PictureUtility.ComPressJPEG(System.Drawing.Image.FromStream(fup.PostedFile.InputStream), width, height).Save(fullPath);
                    reMsg = saveFileName;
                }
                catch (Exception ex)
                {
                    reMsg = "Error: " + ex.Message;
                }
            }
            return reMsg;
        }

        public static string UpLoadJPEGAndWaterMark(System.Web.UI.WebControls.FileUpload fup, string savePath, string saveFileName, string stampFullPath, int width, int height)
        {
            string reMsg = "Error: 未知错误！";
            if (fup.HasFile)
            {
                reMsg = CreateDirectory(savePath);
                if (reMsg != "") return reMsg;

                try
                {
                    System.Drawing.Image sourceImg = PictureUtility.ComPressJPEG(System.Drawing.Image.FromStream(fup.PostedFile.InputStream), width, height);

                    System.Drawing.Image stampImg = System.Drawing.Image.FromFile(stampFullPath);
                    sourceImg = PictureUtility.AddWaterMark(sourceImg, stampImg, 1);
                    string fullPath = Path.Combine(savePath, saveFileName);
                    sourceImg.Save(fullPath);
                    reMsg = saveFileName;
                }
                catch (Exception ex)
                {
                    reMsg = "Error: " + ex.Message;
                }
            }
            return reMsg;
        }

        public static string UpLoadJPEGAndWaterMark(System.Web.UI.WebControls.FileUpload fup, string savePath, string saveFileName, string stampFullPath)
        {
            string reMsg = "Error: 未知错误！";
            if (fup.HasFile)
            {
                reMsg = CreateDirectory(savePath);
                if (reMsg != "") return reMsg;

                try
                {
                    System.Drawing.Image sourceImg = System.Drawing.Image.FromStream(fup.PostedFile.InputStream);

                    System.Drawing.Image stampImg = System.Drawing.Image.FromFile(stampFullPath);
                    sourceImg = PictureUtility.AddWaterMark(sourceImg, stampImg, 1);
                    string fullPath = Path.Combine(savePath, saveFileName);
                    sourceImg.Save(fullPath);
                    reMsg = saveFileName;
                }
                catch (Exception ex)
                {
                    reMsg = "Error: " + ex.Message;
                }
            }
            return reMsg;
        }

        #endregion

        #region 通用方法
        public static string GetFileName(string expandFileName)
        {

            string fileName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
                + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString()
                + DateTime.Now.Millisecond.ToString() + new Random(100).Next().ToString() + expandFileName;
            return fileName;
        }
        public static string CheckFileType(FileType fileType, string fileName)
        {
            string reMsg = "";
            string exName = Path.GetExtension(fileName);
            if (fileType == FileType.Image)
            {
                if (!(exName.ToUpper() == ".GIF" || exName.ToUpper() == ".JPG" || exName.ToUpper() == ".PNG" || exName.ToUpper() == ".BMP"))
                {
                    reMsg = "Error: " + "图片类型只能为GIF,JPG,PNG,或BMP格式！";
                }
            }
            if (fileType == FileType.Flash)
            {
                if (!(exName.ToUpper() == ".SWF" || exName.ToUpper() == ".FLA"))
                {
                    reMsg = "Error: " + "请选择Flash文件！";
                }
            }
            return reMsg;
        }
        public static string CheckFileSize(System.Web.UI.WebControls.FileUpload fup, int KB)
        {
            string reMsg = "";
            if (fup.HasFile)
            {
                if (fup.PostedFile.ContentLength > KB * 1024)
                {
                    reMsg = "Error: " + "文件大小应该小于 " + KB.ToString() + "KB！";
                }
            }
            return reMsg;
        }
        public static string CreateDirectory(string savePath)
        {
            string reMsg = "";
            if (!Directory.Exists(savePath))
            {
                try
                {
                    Directory.CreateDirectory(savePath);
                }
                catch (Exception ex)
                {
                    reMsg = "Error: " + ex.Message;
                }
            }
            return reMsg;
        }

        public static bool DeleteFile(string file_path)
        {
            //删除相应的附件文件
            if (File.Exists(file_path))
            {
                try
                {
                    File.Delete(file_path);
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
        public static bool CheckFile(string file_path)
        {
            //删除相应的附件文件
            if (File.Exists(file_path))
            {
                return true;
            }
            return false;
        }
        public static string ServerPath(string strPath)
        {
            return HttpContext.Current.Server.MapPath(strPath);
        }
        #endregion

        /// <summary>  
        /// 判断远程文件是否存在  
        /// </summary>  
        /// <param name="fileUrl"></param>  
        /// <returns></returns>  
        public static bool RemoteFileExists(string fileUrl)
        {
            HttpWebRequest re = null;
            HttpWebResponse res = null;
            try
            {
                re = (HttpWebRequest)WebRequest.Create(fileUrl);
                res = (HttpWebResponse)re.GetResponse();
                if (res.ContentLength != 0)
                {
                    //MessageBox.Show("文件存在");  
                    return true;
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("无此文件");  
                return false;
            }
            finally
            {
                if (re != null)
                {
                    re.Abort();//销毁关闭连接  
                }
                if (res != null)
                {
                    res.Close();//销毁关闭响应  
                }
            }
            return false;
        }
    }



}
