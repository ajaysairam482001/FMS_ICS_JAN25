using FMS_ICS.Filters;
using FMS_ICS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMS_ICS.Controllers
{
    [RoleAuthorize(AllowedRoles = "Admin")]
    public class ProductController : Controller
    {
        private readonly fms_db_icsEntities entities = new fms_db_icsEntities();

        // GET: Product
        public ActionResult Index()
        {
            var products = entities.Products.Where(p => (bool)p.IsActive).ToList();
            return View(products);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FMS_ICS.Models.Product product)
        {
            if (ModelState.IsValid)
            {
                product.IsActive = true;
                entities.Products.Add(product);
                entities.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Product/Edit
        public ActionResult Edit(int id)
        {
            var product = entities.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    entities.Entry(product).State = System.Data.Entity.EntityState.Modified;
                    product.IsActive = true;
                    if(product.ImageURL == null)
                    {
                        product.ImageURL = "Image here";
                    }
                    entities.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(product);
        }

        // GET: Product/Delete
        public ActionResult Delete(int id)
        {
            var product = entities.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = entities.Products.Find(id);
            if (product != null)
            {
                // Delete related records in Purchases

                product.IsActive = false;
                entities.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}