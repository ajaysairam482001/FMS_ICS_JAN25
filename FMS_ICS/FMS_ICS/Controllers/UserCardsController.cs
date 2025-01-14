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
    public class UserCardsController : Controller
    {
        //user page1
        private fms_db_icsEntities db = new fms_db_icsEntities();

        public ActionResult Index()
        {
            // Replace 1 with logic to fetch the logged-in user's ID (e.g., from session or authentication)
            //var userId = 4;
            int userId = (int)Session["UserID"];
            // Fetch the user details
            var user = db.Users.FirstOrDefault(u => u.UserID == userId);
            if (user == null)
                return HttpNotFound("User not found.");

            // Fetch the user's active card details
            var userCard = db.UserCards
                .FirstOrDefault(uc => uc.UserID == userId);

            // Fetch the products purchased by the user
            var productsPurchased = db.Purchases
                .Where(p => p.UserID == userId)
                .OrderByDescending(p => p.PurchaseDate) // Latest purchases first
                .Select(p => new
                {
                    p.PurchaseID,
                    ProductName = db.Products.FirstOrDefault(pr => pr.ProductID == p.ProductID).ProductName,
                    p.PurchaseDate,
                    p.TotalAmount,
                    p.TenureMonths,
                    p.MonthlyEMI,
                    p.RemainingEMI
                })
                .ToList()
                .Select(p => new Purchase
                {
                    Product = new Product { ProductName = p.ProductName },
                    PurchaseDate = p.PurchaseDate,
                    TotalAmount = p.TotalAmount,
                    TenureMonths = p.TenureMonths,
                    MonthlyEMI = p.MonthlyEMI,
                    RemainingEMI = p.RemainingEMI
                })
                .ToList();

            // Pass data to the View
            ViewBag.UserName = user.FullName;
            ViewBag.CardNumber = userCard?.CardNumber;
            ViewBag.Validity = userCard?.Validity;
            ViewBag.RemainingLimit = userCard?.RemainingLimit;
            ViewBag.ProductsPurchased = productsPurchased;
            ViewBag.Status = userCard.Status;

            return View();
        }

        [HttpPost]
        public ActionResult Logout()
        {
            // Clear the session
            Session.Clear();
            Session.Abandon();

            // Redirect to the Login page
            return RedirectToAction("Index", "Login");
        }
    }
}