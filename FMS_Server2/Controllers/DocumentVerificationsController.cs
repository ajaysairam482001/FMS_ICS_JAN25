using FMS_Server2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;

namespace FMS_Server2.Controllers
{
    public class DocumentVerificationsController : ApiController
    {
        private fms_db_ics2Entities db = new fms_db_ics2Entities();

        // GET: api/DocumentVerification (Admin)
        [HttpGet]
        [Route("api/documentverification/admin")]
        public IHttpActionResult GetDocumentsForAdmin()
        {
            var pendingUsers = db.Users.Where(u => u.Status == "Pending").ToList();
            var pendingUserIDs = pendingUsers.Select(u => u.UserID).ToList();
            var documents = db.DocumentVerification.Where(d => pendingUserIDs.Contains(d.UserID)).ToList();
            return Ok(documents);
        }

        // POST: api/DocumentVerification (User)
        [HttpPost]
        [Route("api/documentverification/user")]
        public IHttpActionResult PostDocumentForReview([FromBody] DocumentVerification document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Add document to the database
            db.DocumentVerification.Add(document);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(document);

        }

        [HttpGet]
        [Route("api/documentverification/{id}")]
        public IHttpActionResult GetDocumentById(int id)
        {
            var document = db.DocumentVerification.Find(id);
            if (document == null)
            {
                return NotFound();
            }

            return Ok(document);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}