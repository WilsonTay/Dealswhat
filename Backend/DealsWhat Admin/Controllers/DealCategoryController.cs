using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DealsWhat_Admin.Models;

namespace DealsWhat_Admin.Controllers
{
    public class DealCategoryController : Controller
    {
        private DealsContext db = new DealsContext();

        //
        // GET: /DealCategory/

        public ActionResult Index()
        {
            return View(db.DealCategories.ToList());
        }

        //
        // GET: /DealCategory/Details/5

        public ActionResult Details(string id = null)
        {
            DealCategory dealcategory = db.DealCategories.Find(id);
            if (dealcategory == null)
            {
                return HttpNotFound();
            }
            return View(dealcategory);
        }

        //
        // GET: /DealCategory/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /DealCategory/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DealCategory dealcategory)
        {
            if (ModelState.IsValid)
            {
                dealcategory.Id = Guid.NewGuid();
                db.DealCategories.Add(dealcategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dealcategory);
        }

        //
        // GET: /DealCategory/Edit/5

        public ActionResult Edit(string id = null)
        {
            var guid = Guid.Parse(id);
            DealCategory dealcategory = db.DealCategories.Find(guid);
            if (dealcategory == null)
            {
                return HttpNotFound();
            }
            return View(dealcategory);
        }

        //
        // POST: /DealCategory/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DealCategory dealcategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dealcategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dealcategory);
        }

        //
        // GET: /DealCategory/Delete/5

        public ActionResult Delete(string id = null)
        {
            var guid = Guid.Parse(id);
            DealCategory dealcategory = db.DealCategories.Find(guid);
            if (dealcategory == null)
            {
                return HttpNotFound();
            }
            return View(dealcategory);
        }

        //
        // POST: /DealCategory/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            DealCategory dealcategory = db.DealCategories.Find(id);
            db.DealCategories.Remove(dealcategory);
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