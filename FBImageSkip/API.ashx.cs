using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBImageSkip
{
    /// <summary>
    /// API 的摘要说明
    /// </summary>
    public class API : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";

            string action = "myAction";
            action = context.Request.Form["myaction"];
            string imageid = context.Request.Form["imageid"];

            switch (action)
            {
                case "login":
                    string password = context.Request.Form["mypassword"].ToString();
                    if (password == ConfigHelper.Password)
                    {
                        context.Session["password"] = ConfigHelper.Password;
                        context.Response.Write("<script>window.location = '/FBImageView.ashx';</script>");
                    }
                    break;

                case "delete":
                    if (context.Session["password"] != null)
                    {
                        if (context.Session["password"].ToString() == ConfigHelper.Password)
                        {
                            FBCommon.DeleteImage(imageid);
                        }
                    }
                    break;

                case "save":
                    if (context.Session["password"] != null)
                    {
                        if (context.Session["password"].ToString() == ConfigHelper.Password)
                        {
                            string skipurl = context.Request.Form["skipurl"].Replace("|", "");
                            string title = context.Request.Form["title"].Replace("|", "");
                            string alt = context.Request.Form["alt"].Replace("|", "");
                            FBCommon.SaveImage(imageid, skipurl, title, alt);
                        }
                    }
                    break;
                default:
                    context.Response.Write("Hello World");
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