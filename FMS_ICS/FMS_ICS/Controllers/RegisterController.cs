﻿using FMS_ICS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMS_ICS.Controllers
{

    public class RegisterController : Controller
    {
        public string GenerateCardNumber()
        {
            // Use a random generator to ensure randomness
            Random random = new Random();

            // Generate a 16-digit card number
            string cardNumber = string.Concat(Enumerable.Range(0, 16).Select(_ => random.Next(0, 10).ToString()));

            // Check if the card number is unique in the database
            while (db.UserCards.Any(c => c.CardNumber == cardNumber))
            {
                cardNumber = string.Concat(Enumerable.Range(0, 16).Select(_ => random.Next(0, 10).ToString()));
            }

            return cardNumber;
        }
        fms_db_icsEntities db = new fms_db_icsEntities();

        // GET: Register
        public ActionResult Index()
        {

            var cardTypes = db.CardTypes.ToList();

            ViewBag.CardTypes = new SelectList(cardTypes, "CardTypeID", "CardType1");

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user, int selectedCardType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check eligibility for the selected card type
                    var cardType = db.CardTypes.FirstOrDefault(c => c.CardTypeID == selectedCardType);
                    if (cardType == null)
                    {
                        ModelState.AddModelError("selectedCardType", "Invalid card type selected.");
                        throw new Exception("Invalid card type.");
                    }

                    // Save user details in the database
                    user.RegistrationDate = DateTime.Now;
                    user.Status = "Pending";
                    db.Users.Add(user);
                    db.SaveChanges();
                    if (user != null)
                    {
                        //registration success //open session
                        Session["UserID"] = user.UserID;
                        Session["Username"] = user.Username;
                        Session["role"] = "User";
                    }

                    // Create user card entry
                    var userCard = new UserCard
                    {
                        UserID = user.UserID,
                        CardTypeID = selectedCardType,
                        CardNumber = GenerateCardNumber(),
                        RemainingLimit = cardType.LimitAmount,
                        Validity = DateTime.Now.AddYears(5), // Set validity period
                        Status = "Inactive"
                    };
                    db.UserCards.Add(userCard);
                    db.SaveChanges();

                    // Redirect to success page
                    return RedirectToAction("UserPost", "DocumentVerification");
                }
            }
            catch (Exception ex)
            {
                // Log exception for debugging
                System.Diagnostics.Debug.WriteLine("Error in registration: " + ex.Message);
            }

            // Repopulate card types dropdown in case of an error
            ViewBag.CardTypes = new SelectList(db.CardTypes, "CardTypeID", "CardType1");
            return View(user);
        }
    }
}