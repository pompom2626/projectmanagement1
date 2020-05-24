using Microsoft.AspNet.Identity;
using MvcDemo.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcDemo.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            // What is current user role? Manager or Developer  
            var UserID = User.Identity.GetUserId();
            var userRoles = db.Roles.Include(r => r.Users).ToList();

            var userRoleNames = (from r in userRoles
                                 from u in r.Users
                                 where u.UserId == UserID
                                 select r.Name).ToList();

            //to get project and task lists

           

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}