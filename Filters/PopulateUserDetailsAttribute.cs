using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EliteMenu.Models;

namespace EliteMenu.Filters
{
    public class PopulateUserDetailsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                int userId = Convert.ToInt32(filterContext.HttpContext.Session["UserId"]);
                using (var db = new DBContext())
                {
                    var user = db.Utenti.Find(userId);
                    if (user != null)
                    {
                        filterContext.Controller.ViewBag.UserAvatar = user.Avatar;
                        filterContext.Controller.ViewBag.Username = user.Nome;
                    }
                }
            }
        }
    }
}