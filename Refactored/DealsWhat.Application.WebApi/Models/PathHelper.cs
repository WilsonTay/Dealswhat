using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace DealsWhat.Application.WebApi.Models
{
    public class PathHelper
    {

        public static string ConvertRelativeToAbsoluteDealImagePath(string relativePath)
        {
            var configDirectory = ConfigurationManager.AppSettings["DealImageBaseUrl"];
            var path = Path.Combine(configDirectory, relativePath);

            return path.ToString();
        }
    }
}