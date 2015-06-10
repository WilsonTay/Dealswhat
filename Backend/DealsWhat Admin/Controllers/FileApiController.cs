using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;

namespace DealsWhat_Admin.Controllers
{
    public class FileApiController : ApiController
    {
        // Test
        public HttpResponseMessage PostFile()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var files = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var fileName = Guid.NewGuid() + postedFile.FileName;
                    var filePath = HttpContext.Current.Server.MapPath("~/Images/" + fileName);
                    postedFile.SaveAs(filePath);

                    files.Add(fileName);
                }

                var response = Request.CreateResponse(HttpStatusCode.Created);
                response.Content = new StringContent(JsonConvert.SerializeObject(files));

                return response;
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return result;
        }
    }
}
