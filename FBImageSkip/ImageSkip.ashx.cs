using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Octopus.Log;
using VTS.Common;

namespace FBImageSkip
{
    /// <summary>
    /// ImageSkip 的摘要说明
    /// </summary>
    public class ImageSkip : IHttpHandler
    {
        #region 功能设计
        /*
        * 设计
        * 
        * 1.图片ID
        * 3.图片名
        * 4.自定义图片的跳转虚拟路径 个性化配置
        * 
        * 2.图片相对路径
        * 3.要跳转的URL 为空不跳转
        * 4.是否被全局跳转覆盖【如果被全局覆盖，则自定义的目标URL将会失效】
        * 5.使用的跳转方式 301跳转 脚本跳转
        * 6.使用的跳转脚本相对路径【可以自定义】
        * 7.实现自定义的长宽响应
        * 
        * 功能1：上传图片
        * 功能2：标记图片的跳转脚本
        * 功能3：启用多域名
        * 功能4：启用路径自定义
        * 
        */
        #endregion

        #region 全局变量
        /// <summary>
        /// 日志路径
        /// </summary>
        public static string logPath = string.Empty;

        /// <summary>
        /// 图片路径
        /// </summary>
        public static string imageDataPath = string.Empty;

        /// <summary>
        /// 初始化
        /// </summary>
        static ImageSkip()
        {
            logPath = ConfigHelper.BaseDirectory + "log.txt";
            imageDataPath = ConfigHelper.BaseDirectory + "imagedata.txt";
        }
        #endregion

        #region 处理请求
        public void ProcessRequest(HttpContext Context)
        {
            #region 阻止缓存
            Context.Response.Clear();
            Context.Response.ClearHeaders();
            Context.Response.ClearContent();
            Context.Response.ContentType = "text/html";

            //页面后退刷新的关键
            Context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Context.Response.Cache.SetNoStore();
            #endregion

            #region 定义变量
            //请求文件 /imageslib/a.jpg
            string imagefile = Context.Request.Path;
            #endregion

            #region 跳转判断
            //来源判断
            Uri url = Context.Request.UrlReferrer;

            if (url != null)
            {
                string refurl = url.ToString().ToLower();

                if (!string.IsNullOrEmpty(refurl))
                {
                    LogOut.Info("UrlReferrer:" + refurl);

                    if (refurl.Contains("facebook.com"))
                    {
                        //获取图片
                        FBImageInfo image = FBCommon.GetFBImageInfo(imagefile, imageDataPath);

                        if (image != null)
                        {
                            if (image.ImageSkipUrl != "skipurl")
                            {
                                #region 记录日志
                                LogOut.Info(("Images:" + imagefile).PadRight(100) + "," + url.ToString());
                                #endregion

                                #region 301 跳转
                                //301跳转方式
                                Utils.SetURL301(Context, image.ImageSkipUrl);
                                #endregion

                                #region 脚本跳转
                                //响应脚本跳转
                                //Context.Response.ContentType = "application/x-javascript";
                                //Context.Response.ContentType = "text/html";
                                //Context.Response.Write(Common.ReadTextToendByUTF8(js));
                                //VTSCommon.TraceLog("图片:" + imagefile + "," + url.ToString(), logPath, true, null);
                                //Context.Response.End(); 
                                #endregion
                            }
                        }
                    }
                }
            }
            #endregion

            #region 展示图片
            string filePath = Context.Server.MapPath(imagefile);//Context.Request.PhysicalPath;

            if (File.Exists(filePath))
            {
                InitContentType(Context, filePath);
                int Length;
                Context.Response.OutputStream.Write(CacheImage(filePath, out Length), 0, Length);
            }
            else
            {
                Context.Response.ContentType = "text/html";
                Context.Response.Write("No found!");
            }
            #endregion
        }

        #region 初始类型
        public void InitContentType(HttpContext Context, string FilePath)
        {
            string fileExt = Path.GetExtension(FilePath).ToLower().Remove(0, 1);

            //控制MIME类型
            switch (fileExt)
            {
                case "jpe":
                case "jpeg":
                case "jpg":
                    Context.Response.ContentType = "image/jpeg";
                    break;
                case "bmp":
                    Context.Response.ContentType = "image/bmp";
                    break;
                case "gif":
                    Context.Response.ContentType = "image/gif";
                    break;
                case "png":
                    Context.Response.ContentType = "image/png";
                    break;
                default:
                    Context.Response.ContentType = "text/html";
                    break;
            }
        }
        #endregion

        #region 缓存图片
        public byte[] CacheImage(string imageFilePath, out int length)
        {
            byte[] cache = CacheHelper.Get<byte[]>(imageFilePath);
            if (cache == null)
            {
                CacheHelper.Insert(imageFilePath, VTSCommon.GetPictureData(imageFilePath));
                cache = CacheHelper.Get<byte[]>(imageFilePath);
            }
            length = cache.Length;
            return cache;
        }
        #endregion

        #endregion

        #region IsReusable
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        #endregion
    }
}