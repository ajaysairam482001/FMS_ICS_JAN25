
using FMS_ICS.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace FMS_ICS.Controllers
{
    public class IndexProductController : Controller
    {
        private readonly fms_db_icsEntities db = new fms_db_icsEntities();
        // GET: IndexProduct
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var totalProducts = db.Products.Where(p => (bool)p.IsActive).Count(); // Total number of products
            var products = db.Products.Where(p => (bool)p.IsActive)
            .OrderBy(p => p.ProductID) // Order by ProductID
            .Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();

            // ViewBag for Pagination
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.HasPrevious = page > 1;
            ViewBag.HasNext = page < ViewBag.TotalPages;

            return View(products);
        }
        public ActionResult BuyNow()
        {
            return RedirectToAction("Login","Login");
        }
        public ActionResult Eligibility()
        {
            var eligibility = db.EligibilityCriterias.ToList();
            return View(eligibility);
        }
    }
}