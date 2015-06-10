using System;
using System.Web;
using System.Web.Mvc;
using DealsWhat_Admin.Helpers;

namespace DealsWhat_Admin.Controllers
{
    public class CKEditorController : Controller
    {
        public ActionResult UploadImage()
        {
            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];

                if (file.ContentLength > 0)
                {
                    string funcNum = Request.QueryString["CKEditorFuncNum"];
                    string guid = Guid.NewGuid().ToString();
                    var localFileName = string.Format("{0}_{1}", guid, file.FileName);
                    string pathString = PathHelper.GetUploadedImagePath(localFileName);

                    file.SaveAs(pathString);

                    string absolutePath = VirtualPathUtility.ToAbsolute(PathHelper.ConvertRelativeToAbsoluteDealImagePath(localFileName));

                    return Content("<script type=\"text/javascript\">window.parent.CKEDITOR.tools.callFunction(" + funcNum + ", '" + absolutePath + "', '');</script>");
                }
            }


            return Content("Error");
        }

    }
}
