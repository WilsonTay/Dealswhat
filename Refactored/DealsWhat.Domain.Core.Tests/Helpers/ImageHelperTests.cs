using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Interfaces.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DealsWhat.Domain.Core.Tests.Helpers
{
    [TestClass]
    public class ImageHelperTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullParameter_ExceptionExpected()
        {
            ImageHelper.GenerateThumbnailPath(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EmptyParameter_ExceptionExpected()
        {
            ImageHelper.GenerateThumbnailPath(string.Empty);
        }
    }
}
