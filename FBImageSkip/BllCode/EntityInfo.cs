using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBImageSkip
{
    /// <summary>
    /// 跳转图片对象
    /// </summary>
    public class FBImageInfo
    {
        /// <summary>
        /// 图片ID
        /// </summary>
        public long ImageID { get; set; }

        /// <summary>
        /// 图片名字
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string ImageTitle { get; set; }

        /// <summary>
        /// Alt
        /// </summary>
        public string ImageAlt { get; set; }

        /// <summary>
        /// 图片的相对路径
        /// </summary>
        public string RelativePath { get; set; }

        /// <summary>
        /// 图片请求的虚拟路径
        /// </summary>
        public string VirtualPath { get; set; }

        /// <summary>
        /// 要跳转的目标站点
        /// </summary>
        public string ImageSkipUrl { get; set; }

        /// <summary>
        /// 是否被全局跳转URL所覆盖
        /// </summary>
        public int IsCovered { get; set; }

        /// <summary>
        /// 所使用的跳转方式
        /// </summary>
        public int SkipType { get; set; }

        /// <summary>
        /// 图片所使用的的跳转脚本
        /// </summary>
        public string SkipJs { get; set; }

        public string ImageSrc
        {
            get
            {
                if (!string.IsNullOrEmpty(RelativePath))
                {
                    return string.Concat(ConfigHelper.SiteUrl, this.RelativePath);
                }
                return string.Empty;
            }
        }
    }
}