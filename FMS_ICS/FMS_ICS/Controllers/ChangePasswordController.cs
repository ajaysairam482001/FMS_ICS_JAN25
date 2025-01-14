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
    public class ChangePasswordController : Controller
    {
        private fms_db_icsEntities db = new fms_db_icsEntities();

        // GET: ChangePassword
        public ActionResult Index()
        {
            return View();
        }

        // POST: ChangePassword
        [HttpPost]
        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {

            

            // Check if new password matches the confirmation password
            if (newPassword != confirmPassword)
            {
                ViewBag.ErrorMessage = "Passwords do not match.";
                return View("Index");
            }

            // Check if the new password is at least 8 characters long
            if (newPassword.Length < 8)
            {
                ViewBag.ErrorMessage = "New password must be at least 8 characters long.";
                return View("Index");
            }
            var user1 = db.Users.Find(Session["UserID"]);
            // Retrieve the user by username and current password
            var user = db.Users.FirstOrDefault(u => u.Username == user1.Username && u.Password == currentPassword);
            if (user == null)
            {
                ViewBag.ErrorMessage = "User not found or incorrect current password.";
                return View("Index");
            }

            // Check if the new password is the same as the old password
            if (user.Password == newPassword)
            {
                ViewBag.ErrorMessage = "New password cannot be the same as the old password.";
                return View("Index");
            }

            try
            {
                // Update the password
                user.Password = newPassword;
                db.SaveChanges();

                ViewBag.SuccessMessage = "Password changed successfully.";
                return RedirectToAction("Index", "UserCards");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error occurred while changing the password. Please try again.";
            }

            return View("Index");
        }
    }
}