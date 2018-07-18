using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBImageSkip
{
    /// <summary>
    /// Reboot 的摘要说明
    /// </summary>
    public class Reboot : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request.QueryString == null ? "" : context.Request.QueryString["action"].ToString();
            switch (action)
            {
                case "refreshcache":
                    //刷新缓存
                    string imageDataPath = ConfigHelper.BaseDirectory + "imagedata.txt";
                    FBCommon.RefreshCache(imageDataPath);
                    context.Response.Write("缓存刷新成功!");
                    break;
                case "reboot":
                    HttpRuntime.UnloadAppDomain();
                    context.Response.Write("退出成功!");
                    break;
                default:
                    context.Response.Write("Hello World!");
                    break;
            }
        }

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