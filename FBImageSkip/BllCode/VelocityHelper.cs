using System;
using System.IO;
using System.Web;
using System.Text;
using System.Collections.Generic;

using NVelocity;
using NVelocity.App;
using NVelocity.Context;
using NVelocity.App.Tools;
using NVelocity.Runtime;
using Commons.Collections;

namespace VTS.Web.Util
{
    public class VelocityHelper
    {
        #region 使用方法
        #region 使用方法
        /*
        VelocityHelper vh = new VelocityHelper();
        vh.Init(@"templates");//指定模板文件的相对路径
        vh.PutSet("title", "员工信息");
        vh.PutSet("comName","成都xxxx里公司");
        vh.PutSet("property”,"天营");
        ArrayList aems = new ArrayList();
        //使用tp1.htm模板显示
        vh.Display("tp1.htm");
        */
        #endregion

        #region 使用方法
        /*调用如下：
        VelocityHelper vh = new VelocityHelper();
        /// <summary>
        /// 显示页面
        /// </summary>
        public void ShowInfo()
        {
            vh.Init("~/template/default");//模板路径
            PublicTemplate.GetHead(ref vh);
            vh.Put("menu", 1);
            vh.Put("minDate",DateTime.Now.ToShortDateString());
            vh.Put("maxDate", DateTime.Now.AddMonths(3).ToShortDateString());
            if (CRequest.IsPost())//判断是什么请求
            {
                vh.Put("post", true);
                TJ();
            }
            else
            {
                vh.Put("post", false);
            }
            GetDCStore();
            vh.Display("dc.html");
        }*/
        #endregion
        #endregion

        #region Fields
        private VelocityEngine velocity = null;
        private IContext context = null;
        #endregion

        #region 构造
        public VelocityHelper() { }
        public VelocityHelper(string templatePath)
        {
            Init(templatePath);
        }
        #endregion

        #region Init
        public void Init(string templatDir)
        {
            velocity = new VelocityEngine();

            ExtendedProperties props = new ExtendedProperties();
            props.AddProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            //props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, HttpContext.Current.Server.MapPath(templatDir));
            props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, templatDir);
            props.AddProperty(RuntimeConstants.INPUT_ENCODING, "utf-8");//gb2312
            props.AddProperty(RuntimeConstants.OUTPUT_ENCODING, "utf-8");

            //props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_CACHE, true);              //是否缓存
            //props.AddProperty("file.resource.loader.modificationCheckInterval", (Int64)30);    //缓存时间(秒)
            velocity.Init(props);
            context = new VelocityContext();
            //context.Put("format", new VelocityFormatter(context));
        }
        #endregion

        #region PutSet
        /// <summary>
        /// 给模板变量赋值
        /// </summary>
        /// <param name="key">模板变量</param>
        /// <param name="value">模板变量值</param>
        public void PutSet(string key, object value)
        {
            if (context == null)
            {
                context = new VelocityContext();
            }
            context.Put(key, value);
        }
        #endregion

        #region Display
        /// <summary>
        /// 显示模板
        /// </summary>
        /// <param name="templatFileName">模板文件名</param>
        public void Display(string templatFileName)
        {
            //从文件中读取模板
            //Template template = velocity.GetTemplate(templatFileName);
            Template template = velocity.GetTemplate(templatFileName, "UTF-8");

            //合并模板
            StringWriter writer = new StringWriter();
            template.Merge(context, writer);

            //输出
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(writer.GetStringBuilder().ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        #endregion

        #region CreateHtml
        /// <summary>
        /// 根据模板生成静态页面
        /// </summary>
        /// <param name="templatFileName"></param>
        /// <param name="htmlpath"></param>
        public void CreateHtml(string templatFileName, string htmlpath)
        {
            //从文件中读取模板
            Template template = velocity.GetTemplate(templatFileName);

            //合并模板
            //StringWriter writer = new StringWriter();
            //template.Merge(context, writer);
            //using (StreamWriter write2 = new StreamWriter(HttpContext.Current.Server.MapPath(htmlpath), false, Encoding.UTF8, 200))
            //{
            //    write2.Write(writer);
            //    write2.Flush();
            //    write2.Close();
            //}

            using (StringWriter writer = new StringWriter())
            {
                template.Merge(context, writer);
                using (StreamWriter write2 = new StreamWriter(HttpContext.Current.Server.MapPath(htmlpath), false, Encoding.UTF8, 200))
                {
                    write2.Write(writer);
                }
            }
        }
        #endregion

        #region OutString
        /*
        /// <summary>
        /// 输出一个模板的结果
        /// </summary>
        /// <param name="templatFileName">模板名字</param>
        /// <param name="isZip">是否做压缩处理</param>
        /// <returns></returns>
        public string OutString(string templatFileName, bool isZip)
        {
            Template template = velocity.GetTemplate(templatFileName, "UTF-8");
            StringWriter writer = new StringWriter();
            template.Merge(context, writer);
            if (isZip)
            {
                return VTS.Common.ZipHtml.GZipHtml(writer.ToString());
            }
            return writer.ToString();
        }*/
        #endregion

        #region CreateJS
        /*/// <summary>
        /// 根据模板生成静态页面
        /// </summary>
        /// <param name="templatFileName"></param>
        /// <param name="htmlpath"></param>
        public void CreateJS(string templatFileName, string htmlpath)
        {
            ////从文件中读取模板
            //Template template = velocity.GetTemplate(templatFileName);
            ////合并模板
            //StringWriter writer = new StringWriter();
            //template.Merge(context, writer);
            //using (StreamWriter write2 = new StreamWriter(HttpContext.Current.Server.MapPath(htmlpath), false, Encoding.UTF8, 200))
            //{
            //    write2.Write(YZControl.Strings.Html2Js(YZControl.Strings.ZipHtml(writer.ToString())));
            //    write2.Flush();
            //    write2.Close();
            //}
        }*/
        #endregion
    }
}