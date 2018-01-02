using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Net;
namespace WebApi.Common.Util
{
    public class PictureUtility
    {

        #region 通用方法

        #region 从字节数组中得到图片
        public static Image GetImgFromBytes(byte[] buffer)
        {
            try
            {
                MemoryStream ms = new MemoryStream(buffer);
                System.Drawing.Image img = Bitmap.FromStream(ms);
                return img;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 获取图形编码信息
        private static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }
        #endregion

        #region 缩略图
        /// <summary>
        /// 生成指定了最大宽度的缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="maxWidth">缩略图最大边长</param>
        public static void MakeThumbnail(System.Drawing.Image originalImage, Int32 maxWidth, string originalImagePath)
        {

            //String tempFile = originalImagePath + "Temp";

            ////原图文件名改为临时文件名
            ////System.IO.File.Move(originalImagePath, tempFile);

            //System.Drawing.Image originalImage = System.Drawing.Image.FromStream(originalImagePath);
            Int32 towidth = originalImage.Width;
            Int32 toheight = originalImage.Height;
            Int32 x = 0;
            Int32 y = 0;
            Int32 ow = originalImage.Width;
            Int32 oh = originalImage.Height;

            //指定宽，高按比例
            //只有当原图宽度大于指定最大宽度时，才进行缩放
            if (originalImage.Width > maxWidth)
            {
                towidth = maxWidth;
                toheight = originalImage.Height * maxWidth / originalImage.Width;
            }

            //Utility.LogInfo("Make Thunbnail", "mode = " + mode.ToString() + ", width = " + width + ", height = " + height + ", towidth = " + towidth + ", toheight = " + toheight + ", x = " + x + ", y = " + y + ", ow = " + ow + ", oh = " + oh);

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //Image im = AddWaterMark(bitmap, Bitmap.FromFile(SiteUtility.GetServerPath(SiteUtility.GetAppSettings("shuiyin"))), 1);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage,
                new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(originalImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                //this.SendFileUploadResponse(1, true, "", "", e.ToString());
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();

                //删除临时文件
                //System.IO.File.Delete(tempFile);
            }

        }
        #endregion

        #region 图片加水印
        /// <summary>
        /// 图片加水印
        /// </summary>
        /// <param name="Source">源图片</param>
        /// <param name="Stamp">水印图片</param>
        /// <param name="Position">
        /// 水印位置
        /// 1：右下角
        /// 2：左下角
        /// 3：右上角
        /// 4：左上角
        /// 5：中间
        /// </param>
        /// <returns></returns>
        public static Image AddWaterMark(Image Source, Image Stamp, int Position)
        {
            Bitmap bm = new Bitmap(Source.Width, Source.Height);

            Graphics g = Graphics.FromImage(bm);
            g.DrawImage(Source, new Point(0, 0));

            //设置图像的透明度
            float[][] ptsArray ={ 
                                    new float[] {1, 0, 0, 0, 0},
                                    new float[] {0, 1, 0, 0, 0},
                                    new float[] {0, 0, 1, 0, 0},
                                    new float[] {0, 0, 0, 0.4F, 0}, //注意：在此处设图象的透明度
                                    new float[] {0, 0, 0, 0, 1}};

            ColorMatrix clrMatrix = new ColorMatrix(ptsArray);
            ImageAttributes imgAttributes = new ImageAttributes();

            imgAttributes.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            //画图像
            Rectangle rect = new Rectangle();
            rect.Height = Stamp.Height;
            rect.Width = Stamp.Width;
            switch (Position)
            {
                default:
                case 1:
                    rect.X = Source.Width - Stamp.Width;
                    rect.Y = Source.Height - Stamp.Height;
                    break;
                case 2:
                    rect.X = 0;
                    rect.Y = Source.Height - Stamp.Height;
                    break;
                case 3:
                    rect.X = Source.Width - Stamp.Width;
                    rect.Y = 0;
                    break;
                case 4:
                    rect.X = 0;
                    rect.Y = 0;
                    break;
                case 5:
                    rect.X = (Source.Width - Stamp.Width) / 2;
                    rect.Y = (Source.Height - Stamp.Height) / 2;
                    break;
            }
            g.DrawImage(Stamp, rect, 0, 0, Stamp.Width, Stamp.Height, GraphicsUnit.Pixel, imgAttributes);

            g.Dispose();
            return bm;
        }
        #endregion

        #region 添加文字水印
        /// <summary>         
        /// 添加文字水印         
        /// </summary>         
        /// <param name="text">水印文字</param>         
        /// <param name="file">图片文件</param>         
        public static void AttachText(string text, string file, string path)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            if (!File.Exists(file))
            {
                return;
            }
            FileInfo oFile = new FileInfo(file);
            string strTempFile = Path.Combine(oFile.DirectoryName, Guid.NewGuid().ToString() + oFile.Extension);
            oFile.CopyTo(strTempFile);
            Image img = Image.FromFile(strTempFile);
            ImageFormat thisFormat = img.RawFormat;
            int nHeight = img.Height;
            int nWidth = img.Width;
            Bitmap outBmp = new Bitmap(nWidth, nHeight);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.White);
            // 设置画布的描绘质量        
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, new Rectangle(0, 0, nWidth, nHeight), 0, 0, nWidth, nHeight, GraphicsUnit.Pixel);
            int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };
            Font crFont = null;
            SizeF crSize = new SizeF();
            //通过循环这个数组，来选用不同的字体大小   
            //如果它的大小小于图像的宽度，就选用这个大小的字体     
            for (int i = 0; i < 7; i++)
            {
                //设置字体，这里是用arial，黑体       
                crFont = new Font("arial", sizes[i], FontStyle.Bold);
                //Measure the Copyright string in this Font             
                crSize = g.MeasureString(text, crFont);
                if ((ushort)crSize.Width < (ushort)nWidth)
                { break; }
            }
            //因为图片的高度可能不尽相同, 所以定义了       
            //从图片底部算起预留了5%的空间     
            int yPixlesFromBottom = (int)(nHeight * .08);
            //现在使用版权信息字符串的高度来确定要绘制的图像的字符串的y坐标    
            float yPosFromBottom = ((nHeight - yPixlesFromBottom) - (crSize.Height / 2));
            //计算x坐标          
            float xCenterOfImg = (nWidth / 2);
            //把文本布局设置为居中          
            StringFormat StrFormat = new StringFormat();

            StrFormat.Alignment = StringAlignment.Center;
            //通过Brush来设置黑色半透明      
            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));
            //绘制版权字符串           
            g.DrawString(text,             //版权字符串文本            
                crFont,                             //字体           
                semiTransBrush2,                           //Brush        
                new PointF(xCenterOfImg + 1, yPosFromBottom + 1),  //位置     
                StrFormat);              //设置成白色半透明          
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));
            //第二次绘制版权字符串来创建阴影效果           
            //记住移动文本的位置1像素           
            g.DrawString(text,                 //版权文本     
                crFont,                                   //字体 
                semiTransBrush,                           //Brush    
                new PointF(xCenterOfImg, yPosFromBottom),  //位置     
                StrFormat);
            g.Dispose();              // 以下代码为保存图片时，设置压缩质量         
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;              //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegICI = null;
            for (int x = 0; x < arrayICI.Length; x++)
            {
                if (arrayICI[x].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[x];//设置JPEG编码          
                    break;
                }
            } if (jpegICI != null)
            { outBmp.Save(file, jpegICI, encoderParams); }
            else { outBmp.Save(file, thisFormat); }
            img.Dispose();
            outBmp.Dispose();
            File.Delete(strTempFile);
        }
        #endregion

        #endregion

        #region GIF图片操作
        public static Image CompressGIF(Image ScaleImage, int ScaleWidth, int ScaleHeight)
        {
            //计算图片的缩放比例，保证图片的长宽比不变
            float Scale = Math.Min((float)ScaleWidth / ScaleImage.Width,
                (float)ScaleHeight / ScaleImage.Height);
            float newImgWidth = Scale * ScaleImage.Width;
            float newImgHeight = Scale * ScaleImage.Height;

            //当原图比需要的图小时以BackGroundColor填充
            Bitmap OutputBitmap = new Bitmap(ScaleWidth, ScaleHeight);

            Graphics g = Graphics.FromImage(OutputBitmap);

            g.Clear(System.Drawing.Color.Transparent);

            if (Scale > 1)
                g.DrawImage(ScaleImage,
                    (ScaleWidth - ScaleImage.Width) / 2,
                    (ScaleHeight - ScaleImage.Height) / 2,
                    ScaleImage.Width, ScaleImage.Height);
            else
                g.DrawImage(ScaleImage,
                    (ScaleWidth - newImgWidth) / 2,
                    (ScaleHeight - newImgHeight) / 2,
                    newImgWidth, newImgHeight);

            g.Dispose();

            GifPalette gifPalette = new GifPalette();
            OutputBitmap = gifPalette.Quantize(OutputBitmap);



            //压缩图片

            Encoder enc = Encoder.Quality;
            ImageCodecInfo ici = GetCodecInfo("image/gif");
            EncoderParameters eps = new EncoderParameters(1);
            EncoderParameter ep = new EncoderParameter(enc, 1000);

            eps.Param[0] = ep;

            MemoryStream OutputStream = new MemoryStream();



            OutputBitmap.Save(OutputStream, ici, eps);
            return Image.FromStream(OutputStream);


        }
        #endregion

        #region JPEG图片操作
        /// <summary>
        /// JPEG图片压缩
        /// </summary>
        /// <param name="ScaleImage">源图片</param>
        /// <param name="ScaleWidth">宽</param>
        /// <param name="ScaleHeight">高</param>
        /// <returns></returns>
        public static Image ComPressJPEG(Image ScaleImage, int ScaleWidth, int ScaleHeight)
        {
            //计算图片的缩放比例，保证图片的长宽比不变
            float Scale = Math.Min((float)ScaleWidth / ScaleImage.Width,
                (float)ScaleHeight / ScaleImage.Height);
            float newImgWidth = Scale * ScaleImage.Width;
            float newImgHeight = Scale * ScaleImage.Height;

            //当原图比需要的图小时以BackGroundColor填充
            Bitmap OutputBitmap = new Bitmap(ScaleWidth, ScaleHeight);
            
            Graphics g = Graphics.FromImage(OutputBitmap);

            //g.Clear(System.Drawing.Color.Transparent);
            g.Clear(System.Drawing.Color.White);

            if (Scale > 1)
                g.DrawImage(ScaleImage,
                    (ScaleWidth - ScaleImage.Width) / 2,
                    (ScaleHeight - ScaleImage.Height) / 2,
                    ScaleImage.Width, ScaleImage.Height);
            else
                g.DrawImage(ScaleImage,
                    (ScaleWidth - newImgWidth) / 2,
                    (ScaleHeight - newImgHeight) / 2,
                    newImgWidth, newImgHeight);

            g.Dispose();
            try
            {
                EncoderParameter p;
                EncoderParameters ps;

                ps = new EncoderParameters(1);
                long[] quality = new long[1];
                quality[0] = 5000;
                p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                ps.Param[0] = p;
                MemoryStream OutputStream = new MemoryStream();
                OutputBitmap.Save(OutputStream, GetCodecInfo("image/jpeg"), ps);
                return Image.FromStream(OutputStream);
            }
            catch (Exception ee)
            {
                throw new ApplicationException(ee.Message.ToString());
            }
        }


        #endregion

        #region MyRegion


        /// </summary>
        /// 
        /// <summary>
        /// 把一个图片加到另一个上
        /// </summary>
        /// <param name="BackGroundImage">背景图</param>
        /// <param name="img">新图</param>
        /// <param name="X">X</param>
        /// <param name="Y">Y</param>
        /// <param name="Width">宽度</param>
        /// <param name="Height">高度</param>
        public static Image DrawImage(Image BackGroundImage, Image img, int X, int Y, int Width, int Height)
        {
            Bitmap bm = new Bitmap(BackGroundImage);
            Graphics g = Graphics.FromImage(bm);

            //设置图像的透明度
            float[][] ptsArray ={ 
                                    new float[] {1, 0, 0, 0, 0},
                                    new float[] {0, 1, 0, 0, 0},
                                    new float[] {0, 0, 1, 0, 0},
                                    new float[] {0, 0, 0, 1, 0}, //注意：在此处设图象的透明度
                                    new float[] {0, 0, 0, 0, 1}};

            ColorMatrix clrMatrix = new ColorMatrix(ptsArray);
            ImageAttributes imgAttributes = new ImageAttributes();

            imgAttributes.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);


            g.DrawImage(img, new Rectangle(X, Y, Width, Height),
                0, 0, img.Width, img.Height,
                GraphicsUnit.Pixel, imgAttributes);
            g.Flush();
            g.Dispose();
            return bm;
        }

        /// <summary>
        /// 给图片加上印章
        /// </summary>
        ///<remarks>印章图案必须是带Aphla通道的Png图片</remarks>
        /// <param name="BackGroundImage">需要加印章的图</param>
        /// <param name="StampImage">印章图案</param>
        /// <param name="Location">盖印章的位置</param>
        /// <param name="StampScaleWidth">印章的宽度</param>
        /// <param name="StampScaleHeight">印章的高度</param>
        /// <param name="Transparencyint">印章的透明度值为0到1这间的float类型的数</param>
        public static void ImageStamp(ref System.Drawing.Image BackGroundImage, Image StampImage, Point Location, int StampScaleWidth, int StampScaleHeight, float Transparencyint)
        {
            Graphics g = Graphics.FromImage(BackGroundImage);

            //设置图像的透明度
            float[][] ptsArray ={ 
                                    new float[] {1, 0, 0, 0, 0},
                                    new float[] {0, 1, 0, 0, 0},
                                    new float[] {0, 0, 1, 0, 0},
                                    new float[] {0, 0, 0, Transparencyint, 0}, //注意：在此处设图象的透明度
                                    new float[] {0, 0, 0, 0, 1}};

            ColorMatrix clrMatrix = new ColorMatrix(ptsArray);
            ImageAttributes imgAttributes = new ImageAttributes();

            imgAttributes.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            //缩放印章的大小,并用透明色填充背景。
            Bitmap bm = (Bitmap)StampImage; //只有Bitmap才可以得到某个象素的颜色
            StampImage = CompressGIF(StampImage, StampScaleWidth, StampScaleHeight);

            //画图像
            g.DrawImage(StampImage, new Rectangle(Location.X, Location.Y, StampImage.Width, StampImage.Height),
                0, 0, StampImage.Width, StampImage.Height,
                GraphicsUnit.Pixel, imgAttributes);
        }
        #endregion

        #region 最新图片处理
        /// <summary>
        /// 获取网络上的图片，转换成2进制流
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static Bitmap GetUrlImage(string Url)
        {
            Bitmap img = null;
            HttpWebRequest req;
            HttpWebResponse res = null;
            try
            {
                System.Uri httpUrl = new System.Uri(Url);
                req = (HttpWebRequest)(WebRequest.Create(httpUrl));
                req.Timeout = 180000; //设置超时值10秒
                //req.UserAgent = "XXXXX";
                //req.Accept = "XXXXXX";
                req.Method = "GET";
                res = (HttpWebResponse)(req.GetResponse());
                img = new Bitmap(res.GetResponseStream());//获取图片流     
                //img.Save(@"E:/" + DateTime.Now.ToFileTime().ToString() + ".png");//随机名
            }

            catch (Exception ex)
            {
                string aa = ex.Message;
            }
            finally
            {
                res.Close();
            }
            return img;
        }

        #endregion

        #region Base64转Image
        public Image Base64ToImage(string base64String)
{
  // Convert Base64 String to byte[]
  byte[] imageBytes = Convert.FromBase64String(base64String);
  MemoryStream ms = new MemoryStream(imageBytes, 0, 
    imageBytes.Length);

  // Convert byte[] to Image
  ms.Write(imageBytes, 0, imageBytes.Length);
  Image image = Image.FromStream(ms, true);
  return image;
}
        #endregion

        #region Image 对象转Base64
          public string ImageToBase64(Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
        #endregion

        #region 在源图中间黏贴另一张图
          /// <summary>    
          /// 调用此函数后使此两种图片合并，类似相册，有个    
          /// 背景图，中间贴自己的目标图片    
          /// </summary>    
          /// <param name="imgBack">粘贴的源图片</param>    
          /// <param name="destImg">粘贴的目标图片</param>    
          public static Image CombinImage(Image imgBack, string destImg)
          {
              Image img = Image.FromFile(destImg);        //照片图片      
              if (img.Height != 65 || img.Width != 65)
              {
                  img = KiResizeImage(img, 65, 65, 0);
              }
              Graphics g = Graphics.FromImage(imgBack);

              g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);     //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);     

              //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框    

              g.DrawImage(img, imgBack.Width / 2 - img.Width / 2, imgBack.Width / 2 - img.Width / 2, img.Width, img.Height);
              GC.Collect();
              return imgBack;
          }
          public static Image CombinImage(Image imgBack, Image img,int Height=30,int Width=30)
          {  
              //if (img.Height != 30 || img.Width != 30)
              //{
                  img = KiResizeImage(img, Width, Height, 0);
              //}
              Graphics g = Graphics.FromImage(imgBack);

              g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);     //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);     

              //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框    

              g.DrawImage(img, imgBack.Width / 2 - img.Width / 2, imgBack.Width / 2 - img.Width / 2, img.Width, img.Height);
              GC.Collect();
              return imgBack;
          }
          /// <summary>    
          /// Resize图片    
          /// </summary>    
          /// <param name="bmp">原始Bitmap</param>    
          /// <param name="newW">新的宽度</param>    
          /// <param name="newH">新的高度</param>    
          /// <param name="Mode">保留着，暂时未用</param>    
          /// <returns>处理以后的图片</returns>    
          public static Image KiResizeImage(Image bmp, int newW, int newH, int Mode)
          {
              try
              {
                  Image b = new Bitmap(newW, newH);
                  Graphics g = Graphics.FromImage(b);
                  // 插值算法的质量    
                  g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                  g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                  g.Dispose();
                  return b;
              }
              catch
              {
                  return null;
              }
          }  
          #endregion
    }

    internal class GifPalette
    {
        private static ArrayList _cardPalette;
        private Color[] _colors;
        private Hashtable _colorMap;

        public GifPalette(ArrayList palette)
        {
            _colorMap = new Hashtable();
            _colors = new Color[palette.Count];
            palette.CopyTo(_colors);
        }

        public GifPalette()
        {
            ArrayList palette = SetPalette();
            _colorMap = new Hashtable();
            _colors = new Color[palette.Count];
            palette.CopyTo(_colors);
        }

        public Bitmap Quantize(Image source)
        {
            int height = source.Height;
            int width = source.Width;

            Rectangle bounds = new Rectangle(0, 0, width, height);

            Bitmap copy = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Bitmap output = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            using (Graphics g = Graphics.FromImage(copy))
            {
                g.PageUnit = GraphicsUnit.Pixel;

                g.DrawImageUnscaled(source, bounds);
            }

            BitmapData sourceData = null;

            try
            {
                sourceData = copy.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                output.Palette = this.GetPalette(output.Palette);
                SecondPass(sourceData, output, width, height, bounds);
            }
            finally
            {
                copy.UnlockBits(sourceData);
            }

            return output;
        }

        private ColorPalette GetPalette(ColorPalette palette)
        {
            for (int index = 0; index < _colors.Length; index++)
                palette.Entries[index] = _colors[index];
            return palette;
        }

        private unsafe void SecondPass(BitmapData sourceData, Bitmap output, int width, int height, Rectangle bounds)
        {
            BitmapData outputData = null;

            try
            {
                outputData = output.LockBits(bounds, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

                byte* pSourceRow = (byte*)sourceData.Scan0.ToPointer();
                Int32* pSourcePixel = (Int32*)pSourceRow;
                Int32* pPreviousPixel = pSourcePixel;
                byte* pDestinationRow = (byte*)outputData.Scan0.ToPointer();
                byte* pDestinationPixel = pDestinationRow;

                byte pixelValue = QuantizePixel((Color32*)pSourcePixel);

                *pDestinationPixel = pixelValue;

                for (int row = 0; row < height; row++)
                {
                    pSourcePixel = (Int32*)pSourceRow;
                    pDestinationPixel = pDestinationRow;
                    for (int col = 0; col < width; col++, pSourcePixel++, pDestinationPixel++)
                    {
                        if (*pPreviousPixel != *pSourcePixel)
                        {
                            pixelValue = QuantizePixel((Color32*)pSourcePixel);
                            pPreviousPixel = pSourcePixel;
                        }
                        *pDestinationPixel = pixelValue;
                    }
                    pSourceRow += sourceData.Stride;

                    pDestinationRow += outputData.Stride;
                }
            }
            finally
            {
                output.UnlockBits(outputData);
            }
        }

        private unsafe byte QuantizePixel(Color32* pixel)
        {
            byte colorIndex = 0;
            int colorHash = pixel->ARGB;

            if (_colorMap.ContainsKey(colorHash))
                colorIndex = (byte)_colorMap[colorHash];
            else
            {
                if (0 == pixel->Alpha)
                {
                    for (int index = 0; index < _colors.Length; index++)
                    {
                        if (0 == _colors[index].A)
                        {
                            colorIndex = (byte)index;
                            break;
                        }
                    }
                }
                else
                {
                    int leastDistance = int.MaxValue;
                    int red = pixel->Red;
                    int green = pixel->Green;
                    int blue = pixel->Blue;

                    for (int index = 0; index < _colors.Length; index++)
                    {
                        Color paletteColor = _colors[index];

                        int redDistance = paletteColor.R - red;
                        int greenDistance = paletteColor.G - green;
                        int blueDistance = paletteColor.B - blue;

                        int distance = (redDistance * redDistance) +
                            (greenDistance * greenDistance) +
                           (blueDistance * blueDistance);
                        if (distance < leastDistance)
                        {
                            colorIndex = (byte)index;
                            leastDistance = distance;

                            if (0 == distance)
                                break;
                        }
                    }
                }

                _colorMap.Add(colorHash, colorIndex);
            }

            return colorIndex;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct Color32
        {
            [FieldOffset(0)]
            public byte Blue;

            [FieldOffset(1)]
            public byte Green;

            [FieldOffset(2)]
            public byte Red;

            [FieldOffset(3)]
            public byte Alpha;

            [FieldOffset(0)]
            public int ARGB;

            public Color Color
            {
                get { return Color.FromArgb(Alpha, Red, Green, Blue); }
            }
        }

        public static ArrayList SetPalette()
        {
            if (null == _cardPalette)
            {
                _cardPalette = new ArrayList();

                //Insert the colors into the arraylist
                //Insert the colors into the arraylist;
                #region Insert the colors into the arraylist
                _cardPalette.Add(Color.FromArgb(255, 0, 0, 0));
                _cardPalette.Add(Color.FromArgb(255, 128, 0, 0));
                _cardPalette.Add(Color.FromArgb(255, 0, 128, 0));
                _cardPalette.Add(Color.FromArgb(255, 128, 128, 0));
                _cardPalette.Add(Color.FromArgb(255, 0, 0, 128));
                _cardPalette.Add(Color.FromArgb(255, 128, 0, 128));
                _cardPalette.Add(Color.FromArgb(255, 0, 128, 128));
                _cardPalette.Add(Color.FromArgb(255, 192, 192, 192));
                _cardPalette.Add(Color.FromArgb(255, 192, 220, 192));
                _cardPalette.Add(Color.FromArgb(255, 166, 202, 240));
                _cardPalette.Add(Color.FromArgb(255, 1, 25, 83));
                _cardPalette.Add(Color.FromArgb(255, 1, 37, 92));
                _cardPalette.Add(Color.FromArgb(255, 2, 51, 103));
                _cardPalette.Add(Color.FromArgb(255, 18, 66, 114));
                _cardPalette.Add(Color.FromArgb(255, 39, 78, 123));
                _cardPalette.Add(Color.FromArgb(255, 101, 63, 107));
                _cardPalette.Add(Color.FromArgb(255, 72, 92, 119));
                _cardPalette.Add(Color.FromArgb(255, 89, 74, 121));
                _cardPalette.Add(Color.FromArgb(255, 85, 101, 122));
                _cardPalette.Add(Color.FromArgb(255, 122, 89, 127));
                _cardPalette.Add(Color.FromArgb(255, 101, 108, 106));
                _cardPalette.Add(Color.FromArgb(255, 111, 116, 111));
                _cardPalette.Add(Color.FromArgb(255, 109, 118, 122));
                _cardPalette.Add(Color.FromArgb(255, 120, 119, 97));
                _cardPalette.Add(Color.FromArgb(255, 121, 124, 114));
                _cardPalette.Add(Color.FromArgb(255, 1, 52, 154));
                _cardPalette.Add(Color.FromArgb(255, 16, 61, 156));
                _cardPalette.Add(Color.FromArgb(255, 15, 63, 160));
                _cardPalette.Add(Color.FromArgb(255, 37, 55, 131));
                _cardPalette.Add(Color.FromArgb(255, 24, 69, 158));
                _cardPalette.Add(Color.FromArgb(255, 21, 68, 162));
                _cardPalette.Add(Color.FromArgb(255, 35, 71, 137));
                _cardPalette.Add(Color.FromArgb(255, 33, 71, 152));
                _cardPalette.Add(Color.FromArgb(255, 43, 93, 130));
                _cardPalette.Add(Color.FromArgb(255, 51, 68, 139));
                _cardPalette.Add(Color.FromArgb(255, 48, 79, 159));
                _cardPalette.Add(Color.FromArgb(255, 53, 85, 131));
                _cardPalette.Add(Color.FromArgb(255, 49, 81, 151));
                _cardPalette.Add(Color.FromArgb(255, 34, 78, 167));
                _cardPalette.Add(Color.FromArgb(255, 41, 84, 170));
                _cardPalette.Add(Color.FromArgb(255, 50, 91, 173));
                _cardPalette.Add(Color.FromArgb(255, 54, 94, 176));
                _cardPalette.Add(Color.FromArgb(255, 53, 101, 136));
                _cardPalette.Add(Color.FromArgb(255, 60, 97, 168));
                _cardPalette.Add(Color.FromArgb(255, 59, 99, 177));
                _cardPalette.Add(Color.FromArgb(255, 68, 93, 134));
                _cardPalette.Add(Color.FromArgb(255, 67, 91, 150));
                _cardPalette.Add(Color.FromArgb(255, 86, 93, 151));
                _cardPalette.Add(Color.FromArgb(255, 64, 102, 142));
                _cardPalette.Add(Color.FromArgb(255, 76, 105, 153));
                _cardPalette.Add(Color.FromArgb(255, 75, 116, 150));
                _cardPalette.Add(Color.FromArgb(255, 81, 103, 140));
                _cardPalette.Add(Color.FromArgb(255, 84, 106, 147));
                _cardPalette.Add(Color.FromArgb(255, 91, 113, 146));
                _cardPalette.Add(Color.FromArgb(255, 75, 107, 172));
                _cardPalette.Add(Color.FromArgb(255, 65, 103, 180));
                _cardPalette.Add(Color.FromArgb(255, 77, 113, 184));
                _cardPalette.Add(Color.FromArgb(255, 90, 104, 162));
                _cardPalette.Add(Color.FromArgb(255, 90, 115, 160));
                _cardPalette.Add(Color.FromArgb(255, 90, 123, 189));
                _cardPalette.Add(Color.FromArgb(255, 101, 87, 130));
                _cardPalette.Add(Color.FromArgb(255, 106, 108, 158));
                _cardPalette.Add(Color.FromArgb(255, 101, 115, 130));
                _cardPalette.Add(Color.FromArgb(255, 103, 121, 149));
                _cardPalette.Add(Color.FromArgb(255, 112, 99, 139));
                _cardPalette.Add(Color.FromArgb(255, 122, 110, 148));
                _cardPalette.Add(Color.FromArgb(255, 101, 122, 165));
                _cardPalette.Add(Color.FromArgb(255, 116, 124, 172));
                _cardPalette.Add(Color.FromArgb(255, 93, 126, 192));
                _cardPalette.Add(Color.FromArgb(255, 96, 127, 192));
                _cardPalette.Add(Color.FromArgb(0, 72, 254, 42));
                _cardPalette.Add(Color.FromArgb(255, 90, 128, 160));
                _cardPalette.Add(Color.FromArgb(255, 97, 130, 159));
                _cardPalette.Add(Color.FromArgb(255, 119, 129, 138));
                _cardPalette.Add(Color.FromArgb(255, 118, 133, 154));
                _cardPalette.Add(Color.FromArgb(255, 107, 131, 169));
                _cardPalette.Add(Color.FromArgb(255, 105, 132, 186));
                _cardPalette.Add(Color.FromArgb(255, 118, 138, 170));
                _cardPalette.Add(Color.FromArgb(255, 117, 137, 180));
                _cardPalette.Add(Color.FromArgb(255, 118, 145, 173));
                _cardPalette.Add(Color.FromArgb(255, 124, 152, 183));
                _cardPalette.Add(Color.FromArgb(255, 95, 128, 192));
                _cardPalette.Add(Color.FromArgb(255, 102, 133, 195));
                _cardPalette.Add(Color.FromArgb(255, 112, 141, 199));
                _cardPalette.Add(Color.FromArgb(255, 120, 147, 202));
                _cardPalette.Add(Color.FromArgb(255, 154, 53, 53));
                _cardPalette.Add(Color.FromArgb(255, 131, 72, 91));
                _cardPalette.Add(Color.FromArgb(255, 143, 87, 104));
                _cardPalette.Add(Color.FromArgb(255, 129, 123, 92));
                _cardPalette.Add(Color.FromArgb(255, 156, 124, 68));
                _cardPalette.Add(Color.FromArgb(255, 129, 126, 101));
                _cardPalette.Add(Color.FromArgb(255, 154, 105, 120));
                _cardPalette.Add(Color.FromArgb(255, 169, 83, 82));
                _cardPalette.Add(Color.FromArgb(255, 165, 125, 70));
                _cardPalette.Add(Color.FromArgb(255, 160, 125, 80));
                _cardPalette.Add(Color.FromArgb(255, 176, 109, 114));
                _cardPalette.Add(Color.FromArgb(255, 205, 53, 2));
                _cardPalette.Add(Color.FromArgb(255, 209, 67, 19));
                _cardPalette.Add(Color.FromArgb(255, 210, 85, 47));
                _cardPalette.Add(Color.FromArgb(255, 230, 67, 21));
                _cardPalette.Add(Color.FromArgb(255, 243, 88, 46));
                _cardPalette.Add(Color.FromArgb(255, 255, 97, 53));
                _cardPalette.Add(Color.FromArgb(255, 201, 92, 71));
                _cardPalette.Add(Color.FromArgb(255, 214, 107, 79));
                _cardPalette.Add(Color.FromArgb(255, 205, 117, 105));
                _cardPalette.Add(Color.FromArgb(255, 245, 112, 74));
                _cardPalette.Add(Color.FromArgb(255, 131, 101, 136));
                _cardPalette.Add(Color.FromArgb(255, 139, 110, 144));
                _cardPalette.Add(Color.FromArgb(255, 130, 120, 156));
                _cardPalette.Add(Color.FromArgb(255, 146, 124, 155));
                _cardPalette.Add(Color.FromArgb(255, 162, 117, 131));
                _cardPalette.Add(Color.FromArgb(255, 141, 133, 94));
                _cardPalette.Add(Color.FromArgb(255, 156, 131, 72));
                _cardPalette.Add(Color.FromArgb(255, 151, 133, 83));
                _cardPalette.Add(Color.FromArgb(255, 157, 144, 92));
                _cardPalette.Add(Color.FromArgb(255, 137, 132, 102));
                _cardPalette.Add(Color.FromArgb(255, 135, 136, 120));
                _cardPalette.Add(Color.FromArgb(255, 147, 139, 102));
                _cardPalette.Add(Color.FromArgb(255, 148, 143, 115));
                _cardPalette.Add(Color.FromArgb(255, 156, 146, 106));
                _cardPalette.Add(Color.FromArgb(255, 148, 145, 122));
                _cardPalette.Add(Color.FromArgb(255, 168, 136, 73));
                _cardPalette.Add(Color.FromArgb(255, 168, 138, 88));
                _cardPalette.Add(Color.FromArgb(255, 172, 147, 90));
                _cardPalette.Add(Color.FromArgb(255, 178, 138, 82));
                _cardPalette.Add(Color.FromArgb(255, 186, 153, 69));
                _cardPalette.Add(Color.FromArgb(255, 179, 150, 91));
                _cardPalette.Add(Color.FromArgb(255, 174, 139, 100));
                _cardPalette.Add(Color.FromArgb(255, 166, 154, 107));
                _cardPalette.Add(Color.FromArgb(255, 161, 151, 114));
                _cardPalette.Add(Color.FromArgb(255, 182, 154, 101));
                _cardPalette.Add(Color.FromArgb(255, 190, 162, 81));
                _cardPalette.Add(Color.FromArgb(255, 172, 160, 117));
                _cardPalette.Add(Color.FromArgb(255, 183, 161, 103));
                _cardPalette.Add(Color.FromArgb(255, 182, 163, 119));
                _cardPalette.Add(Color.FromArgb(255, 205, 168, 63));
                _cardPalette.Add(Color.FromArgb(255, 218, 174, 52));
                _cardPalette.Add(Color.FromArgb(255, 221, 177, 53));
                _cardPalette.Add(Color.FromArgb(255, 255, 154, 1));
                _cardPalette.Add(Color.FromArgb(255, 255, 161, 18));
                _cardPalette.Add(Color.FromArgb(255, 235, 184, 44));
                _cardPalette.Add(Color.FromArgb(255, 228, 182, 52));
                _cardPalette.Add(Color.FromArgb(255, 247, 190, 36));
                _cardPalette.Add(Color.FromArgb(255, 200, 155, 94));
                _cardPalette.Add(Color.FromArgb(255, 192, 153, 104));
                _cardPalette.Add(Color.FromArgb(255, 223, 130, 103));
                _cardPalette.Add(Color.FromArgb(255, 215, 132, 118));
                _cardPalette.Add(Color.FromArgb(255, 201, 167, 68));
                _cardPalette.Add(Color.FromArgb(255, 196, 167, 87));
                _cardPalette.Add(Color.FromArgb(255, 209, 173, 70));
                _cardPalette.Add(Color.FromArgb(255, 212, 169, 91));
                _cardPalette.Add(Color.FromArgb(255, 213, 177, 73));
                _cardPalette.Add(Color.FromArgb(255, 198, 166, 102));
                _cardPalette.Add(Color.FromArgb(255, 196, 168, 123));
                _cardPalette.Add(Color.FromArgb(255, 219, 172, 112));
                _cardPalette.Add(Color.FromArgb(255, 219, 183, 106));
                _cardPalette.Add(Color.FromArgb(255, 217, 185, 115));
                _cardPalette.Add(Color.FromArgb(255, 255, 131, 91));
                _cardPalette.Add(Color.FromArgb(255, 249, 143, 109));
                _cardPalette.Add(Color.FromArgb(255, 227, 186, 108));
                _cardPalette.Add(Color.FromArgb(255, 229, 186, 112));
                _cardPalette.Add(Color.FromArgb(255, 255, 164, 123));
                _cardPalette.Add(Color.FromArgb(255, 207, 195, 127));
                _cardPalette.Add(Color.FromArgb(255, 253, 204, 94));
                _cardPalette.Add(Color.FromArgb(255, 235, 194, 108));
                _cardPalette.Add(Color.FromArgb(255, 233, 197, 117));
                _cardPalette.Add(Color.FromArgb(255, 252, 204, 104));
                _cardPalette.Add(Color.FromArgb(255, 249, 204, 115));
                _cardPalette.Add(Color.FromArgb(255, 251, 208, 106));
                _cardPalette.Add(Color.FromArgb(255, 253, 209, 117));
                _cardPalette.Add(Color.FromArgb(255, 128, 137, 140));
                _cardPalette.Add(Color.FromArgb(255, 136, 144, 145));
                _cardPalette.Add(Color.FromArgb(255, 151, 150, 130));
                _cardPalette.Add(Color.FromArgb(255, 144, 149, 148));
                _cardPalette.Add(Color.FromArgb(255, 137, 131, 165));
                _cardPalette.Add(Color.FromArgb(255, 137, 140, 180));
                _cardPalette.Add(Color.FromArgb(255, 140, 154, 168));
                _cardPalette.Add(Color.FromArgb(255, 133, 151, 180));
                _cardPalette.Add(Color.FromArgb(255, 149, 137, 167));
                _cardPalette.Add(Color.FromArgb(255, 149, 152, 188));
                _cardPalette.Add(Color.FromArgb(255, 138, 168, 188));
                _cardPalette.Add(Color.FromArgb(255, 175, 135, 148));
                _cardPalette.Add(Color.FromArgb(255, 169, 154, 129));
                _cardPalette.Add(Color.FromArgb(255, 187, 135, 134));
                _cardPalette.Add(Color.FromArgb(255, 178, 138, 148));
                _cardPalette.Add(Color.FromArgb(255, 184, 146, 156));
                _cardPalette.Add(Color.FromArgb(255, 168, 149, 173));
                _cardPalette.Add(Color.FromArgb(255, 170, 163, 131));
                _cardPalette.Add(Color.FromArgb(255, 184, 165, 128));
                _cardPalette.Add(Color.FromArgb(255, 181, 166, 157));
                _cardPalette.Add(Color.FromArgb(255, 162, 175, 186));
                _cardPalette.Add(Color.FromArgb(255, 189, 185, 170));
                _cardPalette.Add(Color.FromArgb(255, 130, 155, 206));
                _cardPalette.Add(Color.FromArgb(255, 134, 158, 208));
                _cardPalette.Add(Color.FromArgb(255, 146, 155, 196));
                _cardPalette.Add(Color.FromArgb(255, 142, 169, 193));
                _cardPalette.Add(Color.FromArgb(255, 140, 163, 209));
                _cardPalette.Add(Color.FromArgb(255, 153, 168, 199));
                _cardPalette.Add(Color.FromArgb(255, 148, 170, 213));
                _cardPalette.Add(Color.FromArgb(255, 157, 177, 216));
                _cardPalette.Add(Color.FromArgb(255, 164, 166, 193));
                _cardPalette.Add(Color.FromArgb(255, 171, 179, 198));
                _cardPalette.Add(Color.FromArgb(255, 167, 185, 220));
                _cardPalette.Add(Color.FromArgb(255, 181, 179, 205));
                _cardPalette.Add(Color.FromArgb(255, 181, 188, 211));
                _cardPalette.Add(Color.FromArgb(255, 173, 190, 224));
                _cardPalette.Add(Color.FromArgb(255, 178, 196, 217));
                _cardPalette.Add(Color.FromArgb(255, 174, 192, 224));
                _cardPalette.Add(Color.FromArgb(255, 174, 212, 224));
                _cardPalette.Add(Color.FromArgb(255, 184, 198, 227));
                _cardPalette.Add(Color.FromArgb(255, 185, 218, 229));
                _cardPalette.Add(Color.FromArgb(255, 202, 145, 142));
                _cardPalette.Add(Color.FromArgb(255, 196, 172, 134));
                _cardPalette.Add(Color.FromArgb(255, 203, 161, 158));
                _cardPalette.Add(Color.FromArgb(255, 203, 177, 134));
                _cardPalette.Add(Color.FromArgb(255, 207, 185, 151));
                _cardPalette.Add(Color.FromArgb(255, 210, 186, 132));
                _cardPalette.Add(Color.FromArgb(255, 215, 189, 148));
                _cardPalette.Add(Color.FromArgb(255, 213, 168, 167));
                _cardPalette.Add(Color.FromArgb(255, 234, 155, 132));
                _cardPalette.Add(Color.FromArgb(255, 253, 172, 140));
                _cardPalette.Add(Color.FromArgb(255, 193, 191, 193));
                _cardPalette.Add(Color.FromArgb(255, 206, 194, 135));
                _cardPalette.Add(Color.FromArgb(255, 214, 198, 136));
                _cardPalette.Add(Color.FromArgb(255, 219, 204, 145));
                _cardPalette.Add(Color.FromArgb(255, 218, 209, 141));
                _cardPalette.Add(Color.FromArgb(255, 215, 209, 154));
                _cardPalette.Add(Color.FromArgb(255, 231, 196, 135));
                _cardPalette.Add(Color.FromArgb(255, 225, 213, 143));
                _cardPalette.Add(Color.FromArgb(255, 231, 217, 148));
                _cardPalette.Add(Color.FromArgb(255, 251, 212, 129));
                _cardPalette.Add(Color.FromArgb(255, 255, 199, 172));
                _cardPalette.Add(Color.FromArgb(255, 202, 205, 219));
                _cardPalette.Add(Color.FromArgb(255, 193, 205, 230));
                _cardPalette.Add(Color.FromArgb(255, 200, 211, 233));
                _cardPalette.Add(Color.FromArgb(255, 211, 220, 237));
                _cardPalette.Add(Color.FromArgb(255, 213, 222, 240));
                _cardPalette.Add(Color.FromArgb(255, 201, 226, 228));
                _cardPalette.Add(Color.FromArgb(255, 201, 231, 242));
                _cardPalette.Add(Color.FromArgb(255, 216, 227, 232));
                _cardPalette.Add(Color.FromArgb(255, 219, 227, 241));
                _cardPalette.Add(Color.FromArgb(255, 228, 233, 237));
                _cardPalette.Add(Color.FromArgb(255, 229, 234, 244));
                _cardPalette.Add(Color.FromArgb(255, 236, 241, 248));
                _cardPalette.Add(Color.FromArgb(255, 240, 239, 243));
                _cardPalette.Add(Color.FromArgb(255, 253, 253, 254));
                _cardPalette.Add(Color.FromArgb(255, 255, 251, 240));
                _cardPalette.Add(Color.FromArgb(255, 160, 160, 164));
                _cardPalette.Add(Color.FromArgb(255, 128, 128, 128));
                _cardPalette.Add(Color.FromArgb(255, 255, 0, 0));
                _cardPalette.Add(Color.FromArgb(255, 0, 255, 0));
                _cardPalette.Add(Color.FromArgb(255, 255, 255, 0));
                _cardPalette.Add(Color.FromArgb(255, 0, 0, 255));
                _cardPalette.Add(Color.FromArgb(255, 255, 0, 255));
                _cardPalette.Add(Color.FromArgb(255, 0, 255, 255));
                _cardPalette.Add(Color.FromArgb(255, 255, 255, 255));
                #endregion
            }
            return _cardPalette;
        }

    }
}
