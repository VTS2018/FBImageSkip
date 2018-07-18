using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Collections.Generic;

using VTS.Common;

namespace FBImageSkip
{
    /// <summary>
    /// 
    /// </summary>
    public static class FBCommon
    {
        #region WriteLine
        public static bool WriteLine(string path, string content)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.UTF8))
                {
                    sw.WriteLine(content);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region LoadFBImage
        public static List<FBImageInfo> LoadFBImage(string path)
        {
            List<FBImageInfo> list = new List<FBImageInfo>();
            List<string> ls = VTSCommon.LoadTextToList(path, true);

            //字段分割数组
            string[] arr;
            FBImageInfo info = null;

            foreach (string item in ls)
            {
                arr = item.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                //为了兼容以前的需要判断
                if (arr != null)
                {
                    if (arr.Length == 4)
                    {
                        info = new FBImageInfo()
                       {
                           ImageID = long.Parse(arr[0]),
                           ImageName = arr[1],
                           RelativePath = arr[2],
                           ImageSkipUrl = arr[3],
                           ImageTitle = "title",
                           ImageAlt = "alt"
                       };
                    }

                    if (arr.Length == 6)
                    {
                        info = new FBImageInfo()
                       {
                           ImageID = long.Parse(arr[0]),
                           ImageName = arr[1],
                           RelativePath = arr[2],
                           ImageSkipUrl = arr[3],

                           ImageTitle = arr[4],
                           ImageAlt = arr[5]
                       };
                    }
                }
                list.Add(info);
            }
            return list.OrderByDescending(x => x.ImageID).ToList();
        }
        #endregion

        #region 刷新缓存
        public static void RefreshCache(string imagedatapath)
        {
            List<FBImageInfo> cache = CacheHelper.Get<List<FBImageInfo>>("keyimagelist");
            if (cache == null)
            {
                CacheHelper.Insert("keyimagelist", FBCommon.LoadFBImage(imagedatapath));
                cache = CacheHelper.Get<List<FBImageInfo>>("keyimagelist");
            }
            else
            {
                cache.Clear();
                CacheHelper.Insert("keyimagelist", FBCommon.LoadFBImage(imagedatapath));
                cache = CacheHelper.Get<List<FBImageInfo>>("keyimagelist");
            }
        }
        #endregion

        #region Cache
        /// <summary>
        /// 从缓存中读取一个图片对象
        /// </summary>
        /// <param name="RelativePath">图片相对路径</param>
        /// <param name="ImageDataPath">图片Txt文本路径</param>
        /// <returns></returns>
        public static FBImageInfo GetFBImageInfo(string RelativePath, string ImageDataPath)
        {
            List<FBImageInfo> cache = CacheHelper.Get<List<FBImageInfo>>("keyimagelist");
            if (cache == null)
            {
                CacheHelper.Insert("keyimagelist", FBCommon.LoadFBImage(ImageDataPath));
                cache = CacheHelper.Get<List<FBImageInfo>>("keyimagelist");
            }
            return cache.Find(x => x.RelativePath == RelativePath);
        }
        #endregion

        #region 删除图片
        public static void DeleteImage(string imageid)
        {
            List<string> ls = VTSCommon.LoadTextToList(string.Concat(ConfigHelper.BaseDirectory, "imagedata.txt"), true);
            string str = ls.Find(x => x.StartsWith(imageid));
            string[] arr = str.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (arr != null)
            {
                if (arr.Length == 4 || arr.Length == 6)
                {
                    if (System.IO.File.Exists(ConfigHelper.BaseDirectory + arr[2]))
                    {
                        System.IO.File.Delete(ConfigHelper.BaseDirectory + arr[2]);
                    }
                }
            }
            ls.Remove(str);

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(string.Concat(ConfigHelper.BaseDirectory, "imagedata.txt"), false))
            {
                foreach (string item in ls)
                {
                    sw.WriteLine(item);
                }
            }
        }
        #endregion

        #region 保存图片
        public static void SaveImage(string imageid, string skipurl, string title = "title", string alt = "alt")
        {
            List<string> ls = VTSCommon.LoadTextToList(string.Concat(ConfigHelper.BaseDirectory, "imagedata.txt"), true);
            //获取信息
            string str = ls.Find(x => x.StartsWith(imageid));
            ls.Remove(str);

            //逐个修改字段的值
            if (!string.IsNullOrEmpty(str))
            {
                string[] arr = str.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (arr != null)
                {
                    if (arr.Length == 4)
                    {
                        arr[3] = skipurl;
                        str = string.Concat(arr[0], "|", arr[1], "|", arr[2], "|", arr[3], "|", title, "|", alt);
                        ls.Add(str);
                    }

                    if (arr.Length == 6)
                    {
                        arr[3] = skipurl;
                        arr[4] = title;
                        arr[5] = alt;
                        str = string.Concat(arr[0], "|", arr[1], "|", arr[2], "|", arr[3], "|", arr[4], "|", arr[5]);
                        ls.Add(str);
                    }
                }
            }

            //保存动作
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(string.Concat(ConfigHelper.BaseDirectory, "imagedata.txt"), false))
            {
                foreach (string item in ls)
                {
                    sw.WriteLine(item);
                }
            }
        }
        #endregion

        #region GetPassword
        public static string GetPassword()
        {
            using (System.IO.StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "App_Data\\password.db"))
            {
                return sr.ReadLine();
            }
        }
        #endregion
    }

    public class ConfigHelper
    {
        #region BaseDirectory
        static string _baseDirectory = string.Empty;
        /// <summary>
        /// 程序根目录【后缀自带“\\”】
        /// </summary>
        public static string BaseDirectory
        {
            get { return _baseDirectory; }
        }

        public static string _password = string.Empty;
        public static string Password
        {
            get { return _password; }
        }
        #endregion

        #region Static ConfigHelper
        static ConfigHelper()
        {
            _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _password = FBCommon.GetPassword();
        }
        #endregion

        #region SiteUrl
        /// <summary>
        /// 程序Url
        /// 未考虑https://;存在多个域名指向时，有BUG，因为静态变量已存在，如www.abc.com,abc.com
        /// 已解决
        /// </summary>
        public static string SiteUrl
        {
            get
            {
                //如果在线程池中调用的话 就会出现异常
                string _siteurl = string.Concat("http://", HttpContext.Current.Request.Url.Host);
                if (HttpContext.Current.Request.Url.Port != 80)
                {
                    _siteurl = string.Concat("http://", HttpContext.Current.Request.Url.Host, ":", HttpContext.Current.Request.Url.Port);
                }
                return _siteurl;
            }
        }
        #endregion
    }
}