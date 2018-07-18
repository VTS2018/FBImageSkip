using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VTS.Web.Util;

namespace FBImageSkip
{
    /// <summary>
    /// FBImageView 的摘要说明
    /// </summary>
    public class FBImageView : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        #region ProcessRequest
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string template = context.Server.MapPath("/Template");
            string imageDataPath = context.Server.MapPath("~/imagedata.txt");
            VelocityHelper helper = new VelocityHelper(template);

            if (context.Session["password"] != null)
            {
                if (context.Session["password"].ToString() == ConfigHelper.Password)
                {
                    List<FBImageInfo> ls = FBCommon.LoadFBImage(imageDataPath);
                    helper.PutSet("imagelist", ls);
                    helper.Display("imageview.html");
                }
            }
            else
            {
                helper.Display("password.html");
            }
        } 
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