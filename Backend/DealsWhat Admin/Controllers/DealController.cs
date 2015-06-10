using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DealsWhat_Admin.Helpers;
using DealsWhat_Admin.Models;
using log4net;
using log4net.Core;

namespace DealsWhat_Admin.Controllers
{
    [DealsWhatAuthorize]
    public class DealController : Controller
    {
        private DealsContext db = new DealsContext();
        private static readonly IList<Tuple<Guid, string>> DealImagesCache = new List<Tuple<Guid, string>>();
        private ILog logger = LogManager.GetLogger(typeof(DealController));
        //
        // GET: /Deal/ab

        public ActionResult Index()
        {
            var deals = db.Deals
                .Where(d => d.Status != (int)DealStatus.Deleted)
                .Include(d => d.DealCategory)
                .Include(d => d.Merchant)
                .Include(d => d.DealImages)
                .OrderByDescending(d => d.DateAdded);

            return View(deals.ToList());
        }

        //
        // GET: /Deal/Details/5

        public ActionResult Details(string id = null)
        {
            var guid = Guid.Parse(id);
            Deal deal = db.Deals.Find(guid);
            if (deal == null)
            {
                return HttpNotFound();
            }
            return View(deal);
        }

        //
        // GET: /Deal/Createba

        public ActionResult Create(string id = null)
        {
            ViewBag.Category_Id = new SelectList(db.DealCategories, "Id", "Name");
            ViewBag.Merchant_Id = new SelectList(db.Merchants, "Id", "Name");

            if (!string.IsNullOrEmpty(id))
            {
                var guid = Guid.Empty;

                if (Guid.TryParse(id, out guid))
                {
                    Deal persistedDeal = db.Deals.Find(guid);

                    if (persistedDeal == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.Method = "Edit";
                    ViewBag.ActionButtonText = "Edit";

                    return View(persistedDeal);
                }
            }

            ViewBag.Method = "Create";

            var sampleFineprint = string.Empty;
            var path = Server.MapPath("~/Assets/SampleFineprint.txt");
            using (var reader = new StreamReader(System.IO.File.OpenRead(path)))
            {
                sampleFineprint = reader.ReadToEnd();
            }
            var sampleHighlight = string.Empty;
            path = Server.MapPath("~/Assets/SampleHighlight.txt");
            using (var reader = new StreamReader(System.IO.File.OpenRead(path)))
            {
                sampleHighlight = reader.ReadToEnd();
            }
            var sampleDescription = string.Empty;
            path = Server.MapPath("~/Assets/SampleDescription.txt");
            using (var reader = new StreamReader(System.IO.File.OpenRead(path)))
            {
                sampleDescription = reader.ReadToEnd();
            }

            var deal = new Deal();
            deal.Id = Guid.NewGuid();
            deal.FinePrint = sampleFineprint;
            deal.Highlight = sampleHighlight;
            deal.ShortTitle = "Short Title";
            deal.LongTitle = "Long Title";
            deal.ShortDescription = "Short Description";
            deal.LongDescription = sampleDescription;
            deal.RegularPrice = 0.00;
            deal.SpecialPrice = 0.00;
            deal.StartTime = DateTime.Now;
            deal.EndTime = DateTime.Now.AddDays(7);
            deal.Status = (int)DealStatus.Published;

            ViewBag.ActionButtonText = "Create";

            return View(deal);
        }

        public ActionResult GetUploadedImages(string dealId)
        {
            var guid = Guid.Empty;

            if (!Guid.TryParse(dealId, out guid))
            {
                return Json(new { Message = "Deal id is not valid." });
            }

            var savedImages = DealImagesCache.Where(cache => cache.Item1 == guid)
                .Select(s => s.Item2);

            return Json(savedImages, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveUploadedFile()
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            string savedFileName = "";

            try
            {
                Guid guid = Guid.Empty;
                if (!Guid.TryParse(Request.Headers["dealId"], out guid))
                {
                    return Json(new { Message = "Error in saving file" });
                }

                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
                    if (file.ContentLength > 0)
                    {
                        string pathString = PathHelper.GetDefaultDealImagePath();

                        logger.InfoFormat("Attempting to handle file upload {0} in path {1}", fName, pathString);

                        bool isExists = Directory.Exists(pathString);

                        if (!isExists)
                        {
                            logger.InfoFormat("Path {0} does not exist. Creating path..", pathString);
                            Directory.CreateDirectory(pathString);
                        }

                        var localFileName = string.Format("{0}_{1}", guid, file.FileName);
                        savedFileName = localFileName;
                        var path = string.Format("{0}\\{1}", pathString, localFileName);

                        logger.InfoFormat("Attempting to save file {0} in local path {1}", fName, path);
                        file.SaveAs(path);

                        DealImagesCache.Add(new Tuple<Guid, string>(guid, localFileName));

                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error("Error in saving file.", ex);
                isSavedSuccessfully = false;
            }


            if (isSavedSuccessfully)
            {
                return Json(new {Message = savedFileName});
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }

        //
        // POST: /Deal/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Deal deal)
        {
            if (ModelState.IsValid)
            {
                deal.DateAdded = DateTime.Now;
                deal.CanonicalUrl = string.Concat(HttpUtility.UrlPathEncode(deal.ShortTitle.Replace(" ", "-")), Guid.NewGuid());
                deal.SKU = string.Concat(HttpUtility.UrlPathEncode(deal.ShortTitle), Guid.NewGuid());

                var persistedDeal = db.Deals.Find(deal.Id);

                if (persistedDeal == null)
                {
                    var savedImages = DealImagesCache.Where(cache => cache.Item1 == deal.Id);

                    int order = 0;
                    foreach (var image in savedImages)
                    {
                        DealImage dealImage = new DealImage
                        {
                            Deal_Id = deal.Id,
                            Id = Guid.NewGuid(),
                            Order = order,
                            RelativeUrl = image.Item2
                        };

                        deal.DealImages.Add(dealImage);

                        order++;
                    }

                    //if (!deal.DealImages.Any())
                    //{
                    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No images added.");
                    //}

                    db.Deals.Add(deal);
                }
                else
                {
                    db.Entry(persistedDeal).CurrentValues.SetValues(deal);
                }

                foreach (var option in deal.DealOptions)
                {
                    option.Id = Guid.NewGuid();
                    foreach (var attribute in option.DealAttributes)
                    {
                        attribute.Id = Guid.NewGuid();
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Category_Id = new SelectList(db.DealCategories, "Id", "Name", deal.Category_Id);
            ViewBag.Merchant_Id = new SelectList(db.Merchants, "Id", "EmailAddress", deal.Merchant_Id);
            return View(deal);
        }

        //
        // GET: /Deal/Edit/5

        public ActionResult Edit(string id = null)
        {
            var guid = Guid.Parse(id);
            Deal deal = db.Deals.Find(guid);
            if (deal == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_Id = new SelectList(db.DealCategories, "Id", "Name", deal.Category_Id);
            ViewBag.Merchant_Id = new SelectList(db.Merchants, "Id", "EmailAddress", deal.Merchant_Id);
            return View(deal);
        }

        //
        // POST: /Deal/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Deal deal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category_Id = new SelectList(db.DealCategories, "Id", "Name", deal.Category_Id);
            ViewBag.Merchant_Id = new SelectList(db.Merchants, "Id", "EmailAddress", deal.Merchant_Id);
            return View(deal);
        }

        //
        // GET: /Deal/Delete/5

        public ActionResult Delete(string id = null)
        {
            var guid = Guid.Parse(id);
            Deal deal = db.Deals.Find(guid);
            if (deal == null)
            {
                return HttpNotFound();
            }
            return View(deal);
        }

        //
        // POST: /Deal/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Deal deal = db.Deals.Find(id);

            deal.Status = (int)DealStatus.Deleted;
            //db.Deals.Remove(deal);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}