using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using MvcDemo.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcDemo.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            // What is current user role? Manager or Developer  
            var UserID = User.Identity.GetUserId();
            ViewBag.UserId = UserID;
            var userRoles = db.Roles.Include(r => r.Users).ToList();
            var userRoleNames = (from r in userRoles
                                 from u in r.Users
                                 where u.UserId == UserID
                                 select r.Name).ToList();

            string currentUserName = System.Web.HttpContext.Current.User.Identity.Name;
            //   var currentRole = System.Web.Security.Roles.GetRolesForUser().Single();
            //    var RolesForUser = await UserManager.GetRolesAsync(UserID);

            ViewBag.Projects = new SelectList(db.Projects, "Id", "ProjectTitle");
            //   MembershipUser user = Membership.GetUser(currentUserName);
            // to get project and task lists

            //   var projects = db.Projects.Include(p => p.ApplicationUser_Id == UserID).ToList();

            //if project link is clicked, to view tasks list under the project.


            return View();
        }

        public ActionResult CheckDeadline()
        {
            var UserID = User.Identity.GetUserId();
            var SelectManager = db.Projects
           .Where(c => c.UserProjects.Any(d => d.ApplicationUser_Id == UserID))
           .OrderBy(i => i.Priority).ToList();

            //duplication check
            //HashSet<string> proNoti = new HashSet<string>(db.Notifications.Select(s => s.Content));
            //var result1 = db.Projects
            //    .Where(m => !proNoti.Contains(m.ProjectContent)).ToList();
            var result1 = db.Projects.Where(x => !db.Notifications.Any(y => y.DeadlineProjectId == x.Id)).ToList();

            foreach (var project in result1)
            {
                //only notification for project manager
                TimeSpan t = (DateTime.Now - project.Deadline);
                if (t.TotalDays > 0)
                {
                    var n = new Notification();
                    n.Title = $"Deadline Project Notification";
                    n.Content = $"DeadLine Project : {project.ProjectTitle} ..input more resources";
                    n.IsChecked = false;
                    n.IsBeyondDeadline = true;
                    n.ApplicationUserId = project.CreatorId.ToString();
                    n.DeadlineProjectId = project.Id;
                    db.Notifications.Add(n);
                }
            }
            //duplication check
            //HashSet<string> taskNoti = new HashSet<string>(db.Notifications.Select(s => s.Content));
            //var result2 = db.TaskHelpers.Where(m => !taskNoti.Contains(m.Content));
            var result2 = db.TaskHelpers.Where(x => !db.Notifications.Any(y => y.DeadlineTaskHelperId == x.Id)).ToList();
            foreach (var task in result2)
            {
                //only notification for project manager
                TimeSpan t = (DateTime.Now - task.Deadline);
                if (t.TotalDays > 0)
                {
                    var n = new Notification();
                    n.Title = $"Deadline Task Notification";
                    n.Content = $"DeadLine Task : {task.Title} ..input more resources";
                    n.IsChecked = false;
                    n.IsBeyondDeadline = true;
                    n.ApplicationUserId = task.CreatorId.ToString();
                    n.DeadlineTaskHelperId = task.Id;
                    db.Notifications.Add(n);
                }
            }
            db.SaveChanges();
            ViewBag.DeadlineProjects =db.Projects.Where(p=>db.Notifications.Any(n => n.DeadlineProjectId == p.Id));
            ViewBag.DeadlineTasks = db.TaskHelpers.Where(t=>db.Notifications.Any(n => n.DeadlineTaskHelperId ==t.Id ));
            //foreach (var task in db.TaskHelpers)
            //    {
            //        //for porject manager and developer (a developer can review peer developer's deadline and cowork with each other
            //        TimeSpan tt = (DateTime.Now - task.Deadline);
            //        if (tt.TotalDays > 0)
            //        {
            //         //   var notificationList = db.Notifications.Where(nt => nt.ApplicationUser_Id == UserID);
            //         //   if (!notificationList.Any(n => n.Task == task.Id))
            //         //   {
            //                var n = new Notification();
            //                n.Title = $"Deadline Task Notification";
            //                n.Content = $"Deadline Task :{task.Title} ....input more resources";
            //                n.ApplicationUserId = task.CreatorId.ToString();
            //                n.IsChecked = false;
            //                n.IsBeyondDeadline = true;
            //                db.Notifications.Add(n);
            //                db.SaveChanges();
            //          //  }
            //        }
            //    }


            // return View("~/Views/Prjoects/Index.cshtml");
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