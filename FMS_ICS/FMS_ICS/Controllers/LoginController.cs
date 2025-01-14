using FMS_ICS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMS_ICS.Controllers
{
    public class LoginController : Controller
    {
        fms_db_icsEntities db = new fms_db_icsEntities();

        
        [HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult Index()
        {
            return View(); 
        }

        // POST: User Login
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Index(string username, string password)
        {
            try
            {
                if (AuthenticateUser(username, password))
                {
                    return RedirectToAction("Index", "UserCards");
                }
 
                ViewBag.ErrorMessage = "Incorrect username or password";
                
                ModelState.AddModelError("", "Invalid login attempt");
            }
            catch
            {
                // Log error and handle exception
            }

            return View("Index");
        }
        public bool AuthenticateUser(string username, string password)
        {
            var user = db.Users.FirstOrDefault(u =>
                    u.Username == username &&
                    u.Password == password
                );

            if (user != null)
            {
                // Implement session management
                Session["UserID"] = user.UserID;
                Session["Username"] = user.Username;
                Session["role"] = "User";
                return true;
            }
            return false;
        }

        // POST: Admin Login
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AdminLogin(string adminUsername, string adminPassword)
        {
            try
            {
                var admin = db.Admins.FirstOrDefault(a =>
                    a.Username == adminUsername &&
                    a.Password == adminPassword
                );

                if (admin != null)
                {
                    // Implement session management
                    Session["AdminID"] = admin.AdminID;
                    Session["role"] = "Admin";
                    return RedirectToAction("Index", "AdminDashboard");
                    
                }

                ModelState.AddModelError("", "Invalid login attempt");
            }
            catch
            {
                
            }

            return View("Index");
        }

        // Logout
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index");
        }

    }

    //public class LoginController : Controller
    //{
    //    fms_db_icsEntities entities = new fms_db_icsEntities();

    //    //GET
    //    [HttpGet]
    //    public ActionResult Login()
    //    {
    //        return View();
    //    }

    //    [HttpPost]
    //    public ActionResult Login(string username, string password)
    //    {
    //        //check if the fields are empty
    //        if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
    //        {
    //            ViewBag.ErrorMessage = "Username and Password is Required";
    //            return View();
    //        }

    //        var user = entities.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
    //        if(user != null)
    //        {
    //            //login success
    //            Session["UserId"] = user.UserID;
    //            Session["Username"] = user.Username;
    //            return RedirectToAction("Dashboard", "Home"); //actionmethod followed by controller in which its present
    //        }
    //        return RedirectToAction("SignUp", "Register");
    //    }

}