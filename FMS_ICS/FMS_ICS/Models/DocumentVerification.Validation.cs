using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMS_ICS.Models
{
    [MetadataType(typeof(DocumentVerificationMetaData))]
    public partial class DocumentVerification
    {

    }

    public class DocumentVerificationMetaData
    {
        [Required(ErrorMessage = "Full Name is required")]
        [Url(ErrorMessage = "Invalid URL")]
        public string DocumentLink { get; set; }

    }
}