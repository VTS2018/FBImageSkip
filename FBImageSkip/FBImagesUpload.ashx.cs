using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBImageSkip
{
    /// <summary>
    /// FBImagesUpload 的摘要说明
    /// </summary>
    public class FBImagesUpload : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";

            //图片数据库
            string imageDataPath = context.Server.MapPath("~/imagedata.txt");

            //上传文件集合
            HttpFileCollection fileColl = context.Request.Files;

            if (fileColl != null && fileColl.Count > 0)
            {
                HttpPostedFile file = fileColl[0];

                string fileName = file.FileName;
                string fileExt = Path.GetExtension(fileName);
                fileName = VTS.Common.VTSCommon.GetOrderNumber() + fileExt;

                string filePath = context.Server.MapPath("/imageslib/" + fileName);

                if (System.IO.File.Exists(filePath))
                {
                    context.Response.Write("<a href='javascript:window.history.go(-1)'>返回</a><br />");
                    context.Response.Write("已存在同名的图片,请重命名之后再上传！<br />");
                }
                else
                {
                    file.SaveAs(filePath);

                    //存储参数：图片ID,图片名字,图片相对路径,跳转URL,图片title,图片alt
                    FBCommon.WriteLine(imageDataPath, string.Concat(DateTime.Now.Ticks, "|", fileName, "|", "/imageslib/" + fileName, "|", "skipurl", "|", "title", "|", "alt"));

                    context.Response.Write("上传成功！<br />");
                    context.Response.Write("查看图片-><a href='/FBImageView.ashx'>查看图片</a>");
                }

                /*
                long ticks = DateTime.Now.Ticks;
                DateTime now = new DateTime(ticks);
                context.Response.Write(now.ToString("yyyy-MM-dd hh:mm:ss"));
                */
            }
            else
            {
                context.Response.Write("上传失败！");
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