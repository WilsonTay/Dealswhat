using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace DealsWhat.Helpers
{
    public sealed class PathHelper
    {
        public static string GetDefaultDealImagePath()
        {
            var configDirectory = GetDefaultDealImagePathInternal();
            var originalDirectory = new DirectoryInfo(string.Format("{0}{1}", HttpContext.Current.Server.MapPath(@"~"), configDirectory));

            return originalDirectory.ToString();
        }

        private static string GetDefaultDealImagePathInternal()
        {
            var configDirectory = ConfigurationManager.AppSettings["DefaultDealImagesFolder"];
            return configDirectory;
        }

        public static string ConvertRelativeToAbsoluteDealImagePath(string relativePath)
        {
            var path = Path.Combine(GetDefaultDealImagePathInternal(), relativePath);
            var originalDirectory = VirtualPathUtility.ToAppRelative("~" + path);

            return originalDirectory.ToString();
        }
    }
}