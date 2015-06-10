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
    public class MerchantController : Controller
    {
        private DealsContext db = new DealsContext();

        //
        // GET: /Merchant/

        public ActionResult Index()
        {
            return View(db.Merchants.ToList());
        }

        //
        // GET: /Merchant/Details/5

        public ActionResult Details(string id = null)
        {
            var guid = Guid.Parse(id);
            Merchant merchant = db.Merchants.Find(guid);
            if (merchant == null)
            {
                return HttpNotFound();
            }
            return View(merchant);
        }

        //
        // GET: /Merchant/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Merchant/Create

        [HttpPost]
        public ActionResult Create(Merchant merchant)
        {
            if (ModelState.IsValid)
            {
                merchant.Id = Guid.NewGuid();
                db.Merchants.Add(merchant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(merchant);
        }

        //
        // GET: /Merchant/Edit/5

        public ActionResult Edit(string id = null)
        {
            var guid = Guid.Parse(id);
            Merchant merchant = db.Merchants.Find(guid);
            if (merchant == null)
            {
                return HttpNotFound();
            }
            return View(merchant);
        }

        //
        // POST: /Merchant/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Merchant merchant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(merchant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(merchant);
        }

        //
        // GET: /Merchant/Delete/5

        public ActionResult Delete(string id = null)
        {
            var guid = Guid.Parse(id);
            Merchant merchant = db.Merchants.Find(guid);
            if (merchant == null)
            {
                return HttpNotFound();
            }
            return View(merchant);
        }

        //
        // POST: /Merchant/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Merchant merchant = db.Merchants.Find(id);
            db.Merchants.Remove(merchant);
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