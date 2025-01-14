using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMS_ICS.Filters
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        public string AllowedRoles { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var role = httpContext.Session["role"]?.ToString();
            if (!string.IsNullOrEmpty(role) && AllowedRoles.Split(',').Contains(role))
            {
                return true;
            }
            return false;
        }
    
    //protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    //    {
    //        // Redirect unauthorized users to the login page
    //        filterContext.Result = new RedirectToRouteResult(
    //            new System.Web.Routing.RouteValueDictionary
    //            {
    //            { "controller", "Login" },
    //            { "action", "Index" }
    //            });
    //    }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new System.Web.Routing.RouteValueDictionary
                {
                { "controller", "Error" },
                { "action", "AccessDenied" }
                });
        }
    }
    

    }