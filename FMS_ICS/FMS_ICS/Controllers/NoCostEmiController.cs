using FMS_ICS.Filters;
using FMS_ICS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMS_ICS.Controllers
{
    [RoleAuthorize(AllowedRoles = "User")]
    public class NoCostEmiController : Controller
    {
        private readonly fms_db_icsEntities entities = new fms_db_icsEntities();
        // GET: NoCostEMI/Index
        public ActionResult Index()
        {
            var products = entities.Products.Where(p => (bool)p.IsActive).ToList();
            return View(products);
        }
        // GET: NoCostEMI/Buy/{id}
        public ActionResult Buy(int id)
        {
            var product = entities.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            // EMI Tenure Options
            ViewBag.EMIPlans = new List<int> { 3, 6, 9, 12 };
            // Fetch UserID from Session
            if (Session["UserID"] == null)
            {
                TempData["Error"] = "You must be logged in to purchase.";
                return RedirectToAction("Login", "Account"); // Redirect to login if UserID is null
            }

            int userId = (int)Session["UserID"];
            var userCard = entities.UserCards.FirstOrDefault(c => c.UserID == userId && c.Status == "Active");
            
                if (userCard != null)
                {
                    ViewBag.CardID = userCard.CardID;
                    ViewBag.CardTypeID = userCard.CardTypeID;
                    ViewBag.CardNumber = userCard.CardNumber;
                    ViewBag.RemainingLimit = userCard.RemainingLimit;
                    ViewBag.Validity = userCard.Validity;
                    ViewBag.Status = userCard.Status;
                }
            
            return View(product);
        }
        // POST: NoCostEMI/Buy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Buy(int productId, int tenureMonths)
        {
            var product = entities.Products.Find(productId);
            if (product == null)
            {
                return HttpNotFound();
            }
            if (Session["UserID"] == null)
            {
                TempData["Error"] = "You must be logged in to purchase.";
                return RedirectToAction("Login", "Account"); // Redirect to login if UserID is null
            }

            int userId = (int)Session["UserID"];
            
            // Validate user's active card and fetch its details
            var userCard = entities.UserCards.FirstOrDefault(c => c.UserID == userId && c.Status == "Active");
            if (userCard == null || userCard.RemainingLimit < product.Price)
            {
                TempData["Error"] = "Insufficient card limit or no active card.";
                return RedirectToAction("Index");
            }
            // Deduct from card limit
            userCard.RemainingLimit -= product.Price;
            // Calculate Monthly EMI and save purchase
            var monthlyEMI = product.Price / tenureMonths;
            // Creating a new purchase object
            var purchase = new Purchase
            {
                UserID = userId,
                ProductID = product.ProductID,
                TotalAmount = product.Price,
                TenureMonths = tenureMonths,
                MonthlyEMI = monthlyEMI,
                RemainingEMI = product.Price,
                PurchaseDate = DateTime.Now,
            };
            // Creating a new UserCard object
            var card = new UserCard
            {
                CardID = userCard.CardID,
                CardTypeID = userCard.CardTypeID,
                CardNumber = userCard.CardNumber,
                RemainingLimit = userCard.RemainingLimit,
                Validity = userCard.Validity,
                Status = userCard.Status
            };
            // Add purchase and save changes
            entities.Purchases.Add(purchase);
            entities.SaveChanges();
            // Update UserCard after deduction
            entities.Entry(userCard).State = System.Data.Entity.EntityState.Modified;
            entities.SaveChanges();
            TempData["Success"] = $"Product purchased successfully with No-Cost EMI over {tenureMonths} months!";
            return RedirectToAction("Index","NoCostEmi");
        }
    }
}