using FMS_ICS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMS_ICS.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private fms_db_icsEntities db = new fms_db_icsEntities();

        // GET: ForgotPassword
        public ActionResult Index()
        {
            return View();
        }


        // POST: ForgotPassword
        [HttpPost]
        public ActionResult ResetPassword(string username, string phoneNumber, string newPassword, string confirmPassword)
        {
            // Check if new password matches the confirmation password
            if (newPassword != confirmPassword)
            {
                ViewBag.ErrorMessage = "New Password and Confirm Password do not match.";
                return View("Index");
            }

            // Validate password length (8 to 20 characters)
            if (newPassword.Length < 8 || newPassword.Length > 20)
            {
                ViewBag.ErrorMessage = "Password must be between 8 and 20 characters.";
                return View("Index");
            }

            // Check if the user exists and phone number matches
            var user = db.Users.FirstOrDefault(u => u.Username == username && u.PhoneNumber == phoneNumber);
            if (user == null)
            {
                ViewBag.ErrorMessage = "User not found or phone number does not match.";
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

                ViewBag.SuccessMessage = "Password has been reset successfully.";
                return RedirectToAction("Index", "Login");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error occurred while resetting the password. Please try again.";
            }

            return View("Index");
        }
    }
}