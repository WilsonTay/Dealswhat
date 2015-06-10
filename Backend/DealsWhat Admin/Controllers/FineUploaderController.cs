using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DealsWhat_Admin.Helpers;
using DealsWhat_Admin.Models;
using ImageResizer;
using Newtonsoft.Json.Linq;

namespace DealsWhat_Admin.Controllers
{
    public class FineUploaderController : Controller
    {
        [System.Web.Http.HttpGet]
        public ActionResult Index(string dealId)
        {
            using (var context = new DealsContext())
            {
                Guid dealGuid = Guid.Parse(dealId);
                var images = context.DealImages.Where(d => d.Deal_Id.Value == dealGuid)
                    .ToList()
                    .Select(image => new
                    {
                        uuid = image.Id,
                        name = image.RelativeUrl,
                        thumbnailUrl = VirtualPathUtility.ToAbsolute(PathHelper.ConvertRelativeToAbsoluteDealImagePath(image.RelativeUrl))
                    });

                var result = new JsonResult();
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                result.Data = images;

                return result;
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult UploadFile(FineUpload upload)
        {
            // asp.net mvc will set extraParam1 and extraParam2 from the params object passed by Fine-Uploader
            string fileName = Guid.NewGuid().ToString() + upload.Filename;
            string thumbFileName = Path.GetFileNameWithoutExtension(fileName) + "_thumb" + Path.GetExtension(fileName);
            string filePath = PathHelper.GetUploadedImagePath(fileName);
            string thumbPath = PathHelper.GetUploadedImagePath(thumbFileName);

            try
            {
                upload.SaveAs(filePath);

                ImageBuilder.Current.Build(filePath, thumbPath,
                    new ResizeSettings("maxwidth=400&maxheight=400&format=jpg"));

                ImageBuilder.Current.Build(filePath, filePath,
                  new ResizeSettings("maxwidth=1200&maxheight=1200&format=jpg"));

                using (var context = new DealsContext())
                {
                    DealImage dealImage = new DealImage
                    {
                        Deal_Id = Guid.Parse(upload.DealId),
                        Id = Guid.Parse(upload.Uuid),
                        Order = 1,
                        RelativeUrl = fileName
                    };

                    context.DealImages.Add(dealImage);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return new FineUploaderResult(false, error: ex.Message);
            }

            // the anonymous object in the result below will be convert to json and set back to the browser
            return new FineUploaderResult(true, new { extraInformation = 12345, newFileName = fileName });
        }

        [System.Web.Http.HttpPost]
        public ActionResult SetMainImage(string id)
        {
            using (var context = new DealsContext())
            {
                var imageUuid = Guid.Parse(id);
                var dealId = context.DealImages.First(f => f.Id == imageUuid).Deal_Id.Value;

                var otherImages = context.DealImages.Where(f => f.Deal_Id.Value == dealId);

                foreach (var image in otherImages)
                {
                    if (image.Id == imageUuid)
                    {
                        image.Order = 0;
                    }
                    else
                    {
                        image.Order = 1;
                    }
                }

                context.SaveChanges();

                return Content("Success");
            }
        }

        [System.Web.Http.HttpDelete]
        public ActionResult DeleteFile(string id)
        {
            using (var context = new DealsContext())
            {
                var guid = Guid.Parse(id);
                var image = context.DealImages.First(d => d.Id == guid);

                context.DealImages.Remove(image);
                context.SaveChanges();
            }
            return null;
        }
    }


    [ModelBinder(typeof(ModelBinder))]
    public class FineUpload
    {
        public string Filename { get; set; }
        public Stream InputStream { get; set; }
        public string Uuid { get; set; }
        public string DealId { get; set; }

        public void SaveAs(string destination, bool overwrite = false, bool autoCreateDirectory = true)
        {
            if (autoCreateDirectory)
            {
                var directory = new FileInfo(destination).Directory;
                if (directory != null) directory.Create();
            }

            using (var file = new FileStream(destination, overwrite ? FileMode.Create : FileMode.CreateNew))
                InputStream.CopyTo(file);
        }

        public class ModelBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                var request = controllerContext.RequestContext.HttpContext.Request;
                var formUpload = request.Files.Count > 0;

                // find filename
                var xFileName = request.Headers["X-File-Name"];
                var qqFile = request["qqfile"];
                var qqUuid = request["qquuid"];
                var dealId = request["dealId"];
                var formFilename = formUpload ? request.Files[0].FileName : null;

                var upload = new FineUpload
                {
                    Filename = xFileName ?? qqFile ?? formFilename,
                    InputStream = formUpload ? request.Files[0].InputStream : request.InputStream,
                    Uuid = qqUuid,
                    DealId = dealId
                };

                return upload;
            }
        }

    }

    /// <remarks>
    /// Docs at https://github.com/valums/file-uploader/blob/master/server/readme.md
    /// </remarks>
    public class FineUploaderResult : ActionResult
    {
        public const string ResponseContentType = "text/plain";

        private readonly bool _success;
        private readonly string _error;
        private readonly bool? _preventRetry;
        private readonly JObject _otherData;

        public FineUploaderResult(bool success, object otherData = null, string error = null, bool? preventRetry = null)
        {
            _success = success;
            _error = error;
            _preventRetry = preventRetry;

            if (otherData != null)
                _otherData = JObject.FromObject(otherData);
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = ResponseContentType;

            response.Write(BuildResponse());
        }

        public string BuildResponse()
        {
            var response = _otherData ?? new JObject();
            response["success"] = _success;

            if (!string.IsNullOrWhiteSpace(_error))
                response["error"] = _error;

            if (_preventRetry.HasValue)
                response["preventRetry"] = _preventRetry.Value;

            return response.ToString();
        }
    }
}
