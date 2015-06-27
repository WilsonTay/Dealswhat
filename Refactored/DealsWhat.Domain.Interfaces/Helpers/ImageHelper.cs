using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Interfaces.Helpers
{
    public static class ImageHelper
    {
        public static string GenerateThumbnailPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(path);
            }

            var extension = Path.GetExtension(path);
            var thumbPostfix = "_thumb" + extension;

            return path.Replace(extension, thumbPostfix);
        }
    }
}
