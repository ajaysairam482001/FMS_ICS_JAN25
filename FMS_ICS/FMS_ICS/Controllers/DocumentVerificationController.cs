using FMS_ICS.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FMS_ICS.Controllers
{
    public class DocumentVerificationController : Controller
    {
        // GET: DocumentVerification/UserPost
        [HttpGet]
        public ActionResult UserPost()
        {
            var userId = Session["UserID"] as int?;

            if (userId == null)
            {
                ViewBag.ErrorMessage = "UserId session not created or appended";
            }
            return View();
        }
        // GET: DocumentVerification/UserPost
        [HttpPost]
        public async Task<ActionResult> UserPost(DocumentVerification model)
        {
            //use when functional retrive userId from session
            //// Get UserID from Session
            var userId = Session["UserID"] as int?;

            if (userId == null)
            {
                ViewBag.ErrorMessage = "UserId session not created or appended"; 
                return View(model);
            }

            // Assign the UserID to the model
            model.UserID = userId.Value;

            model.Remarks = "Sent for Verification";
            model.DocumentStatus = "Pending";
            if (!Uri.IsWellFormedUriString(model.DocumentLink, UriKind.Absolute))
            {
                ViewBag.ErrorMessage = "Invalid URL format";
                return View(model); // Return to the same view if the URL is invalid
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44392/api/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //serializr it
                var jsonContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                // Make POST request to API
                HttpResponseMessage response = await client.PostAsync("documentverification/user", jsonContent);
                Console.WriteLine(response.Content);
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Document submitted successfully!";
                    return View();
                    //return RedirectToAction("SignUp", "Register");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error: Unable to submit the document drive Link.");
                }
            }

            return View(model);
        }
        
    }
}