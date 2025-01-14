using FMS_ICS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using FMS_ICS.Filters;

namespace FMS_ICS.Controllers
{
    [RoleAuthorize(AllowedRoles = "Admin")]
    public class AdminDashboardController : Controller
    {
        fms_db_icsEntities db = new fms_db_icsEntities();

        // GET: AdminDashboard/Index
        public async Task<ActionResult> Index()
        {
            var pendingUsers = db.Users.Where(u => u.Status == "Pending").ToList();


            var pendingUserIDs = pendingUsers.Select(u => u.UserID).ToList();


            var userCards = db.UserCards.Where(c => pendingUserIDs.Contains(c.UserID)).ToList();
            //var documentVerifications = db.DocumentVerifications.Where(d => pendingUserIDs.Contains(d.UserID)).ToList();
            
            using (var client = new HttpClient())
            {
                // Set the base address for the API
                client.BaseAddress = new Uri("https://localhost:44392/api/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Make a GET request to the API
                HttpResponseMessage response = await client.GetAsync("documentverification/admin");

                // Check if the response is successful
                if (response.IsSuccessStatusCode)
                {
                    // Read and deserialize the JSON response
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var document = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DocumentVerification>>(jsonResponse);
                    //return View(document);
                    // Pass the data to the view
                    ViewBag.PendingUsers = pendingUsers;
                    ViewBag.UserCards = userCards;
                    ViewBag.DocumentVerifications = document;

                    return View();
                }
                else
                {
                    // Log the error or handle the failure case
                    Console.WriteLine($"Error: Unable to fetch document. Status code: {response.StatusCode}");
                }
            }
            return View();

        }

        // GET: AdminDashboard/Verify/{id}
        public ActionResult Verify(int id)
        {
            // Fetching the details of the user to be verified
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            // Fetching the user card and document verification related to the user
            var userCard = db.UserCards.FirstOrDefault(c => c.UserID == user.UserID);
            var documentVerification = db.DocumentVerifications.FirstOrDefault(d => d.UserID == user.UserID);

            // Pass the user, userCard, and documentVerification directly to the view
            ViewBag.User = user;
            ViewBag.UserCard = userCard;
            ViewBag.DocumentVerification = documentVerification;

            return View();
        }

        // POST: AdminDashboard/ActivateUser/{id}
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ActivateUser(int id)
        {
            var user = db.Users.Find(id);
            if (user != null)
            {
                // Update user status to "Active"
                user.Status = "Active";
                db.SaveChanges();

                // Optionally, log the admin action
                var adminAction = new AdminAction
                {
                    AdminID = 1, // Assume admin ID is 1 for now
                    ActionType = "Activate User",
                    ActionDetails = $"User {user.FullName} has been activated.",
                    ActionDate = DateTime.Now
                };
                db.AdminActions.Add(adminAction);
                db.SaveChanges();

                // Now, activate the user's card after the user is active
                var userCard = db.UserCards.FirstOrDefault(c => c.UserID == user.UserID);
                if (userCard != null && userCard.Status == "Inactive")
                {
                    userCard.Status = "Active";
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }

        // POST: AdminDashboard/ActivateCard/{id}
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ActivateCard(int id)
        {
            var userCard = db.UserCards.Find(id);
            //var pendingUser = db.Users.Find(id);
            //var pendingUsers = db.Users.Where(u => u.Status == "Pending").ToList();


            //var pendingUserIDs = pendingUsers.Select(u => u.UserID).ToList();


            //var userCards = db.UserCards.Where(c => pendingUserIDs.Contains(c.UserID)).ToList();
            //var documentVerifications = db.DocumentVerifications.Where(d => pendingUserIDs.Contains(d.UserID)).ToList();

            //// Pass the data to the view
            //ViewBag.PendingUser = pendingUser;
            //ViewBag.PendingUsers = pendingUsers;
            //ViewBag.UserCard = userCard;
            //ViewBag.DocumentVerifications = documentVerifications;
            if (userCard != null)
            {
                // Update card status to "Active"
                userCard.Status = "Active";
                db.SaveChanges();

                // Optionally, log the admin action
                var adminAction = new AdminAction
                {
                    AdminID = 1, // Assume admin ID is 1 for now
                    ActionType = "Activate Card",
                    ActionDetails = $"Card for UserID {userCard.CardTypeID} has been activated.",
                    ActionDate = DateTime.Now
                };
                db.AdminActions.Add(adminAction);
                db.SaveChanges();
                //return View();
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }


        // POST: AdminDashboard/DeactivateCard/{id}
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DeactivateCard(int id)
        {
            var userCard = db.UserCards.Find(id);
            if (userCard != null)
            {
                // Update card status to "Active"
                userCard.Status = "Inactive";
                db.SaveChanges();

                // Optionally, log the admin action
                var adminAction = new AdminAction
                {
                    AdminID = 1, // Assume admin ID is 1 for now
                    ActionType = "Deactivate Card",
                    ActionDetails = $"Card for UserID {userCard.CardTypeID} has been Deactivated.",
                    ActionDate = DateTime.Now
                };
                db.AdminActions.Add(adminAction);
                db.SaveChanges();

                //return View();


                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }


        // POST: AdminDashboard/VerifyDocument/{id}
        [HttpPost]
        //[ValidateAntiForgeryToken]
        // GET: AdminDashboard/VerifyDocument/{id}
        public ActionResult VerifyDocument(int id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var documentVerification = db.DocumentVerifications.FirstOrDefault(d => d.UserID == id);

            ViewBag.User = user;
            ViewBag.DocumentStatus = documentVerification.DocumentStatus;
            ViewBag.DocumentLink = documentVerification.DocumentLink;
            ViewBag.Remarks = documentVerification.Remarks;
            //if (user != null)
            //{
            //    // Update card status to "Active"
            //    documentVerification.DocumentStatus = "Verified";
            //    db.SaveChanges();
               
            //}

            return View(documentVerification);
        }

        [HttpPost]
        public ActionResult ConfirmVerification(int id)
        {
            var documentVerification = db.DocumentVerifications.FirstOrDefault(d => d.UserID == id);
            if (documentVerification == null)
            {
                return HttpNotFound("Document verification details not found.");
            }

            // Update the document status to "Verified"
            documentVerification.DocumentStatus = "Verified";
            db.SaveChanges();

            // Redirect back to the Admin Dashboard
            return RedirectToAction("Index", "AdminDashboard");
        }
    }
}