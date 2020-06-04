using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using MvcDemo.Models;
using MvcDemo.ViewModels;
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
            ViewBag.DeadlineProjects = db.Projects.Where(p => db.Notifications.Any(n => n.DeadlineProjectId == p.Id));
            ViewBag.DeadlineTasks = db.TaskHelpers.Where(t => db.Notifications.Any(n => n.DeadlineTaskHelperId == t.Id));


            //can i send view to project/index?
            // return View("~/Views/Prjoects/Index.cshtml");
            return View();
        }

        public ActionResult CheckBudget()
        {
            //refresh DB
            db.Budgets.RemoveRange(db.Budgets.ToList());
            db.SaveChanges();
            //create new version

            int i = 1;
            //1 project manager
            foreach (var pro in db.Projects.ToList())
            {
                Budget m = new Budget();
                TimeSpan expect = (pro.Deadline - pro.CreateTime).Duration();
                m.days = expect.TotalDays;
                m.ProjectId = pro.Id;
                m.CreatorId = pro.CreatorId.ToString();
                //   m.Id = new Guid();
                m.Id = i.ToString();
                if (pro.IsFinished == true)
                {
                    var fin = (DateTime)pro.FinishedTime;
                    TimeSpan actual = (fin - pro.CreateTime).Duration();
                    m.FinishDays = actual.TotalDays;
                }
                db.Budgets.Add(m);
                db.SaveChanges();
                i++;
            }

            //many developer
            //remove duplicate developer
            var pureTask = db.TaskHelpers.DistinctBy(d => d.CreatorId);
            foreach (var pro in db.Projects.ToList())
            {
                foreach (var task in pureTask.ToList())
                {
                    Budget d = new Budget();
                    if (pro.Id == task.ProjectTask_Id && pro.CreatorId != task.CreatorId)
                    {
                        TimeSpan expect = (pro.Deadline - pro.CreateTime).Duration();
                        d.days = expect.TotalDays;
                        d.ProjectId = pro.Id;
                        d.CreatorId = task.CreatorId.ToString();
                        d.Id = i.ToString();
                        if (task.IsFinished == true)
                        {
                            var fin = (DateTime)task.FinishTime;
                            TimeSpan actual = (fin - task.CreateTime).Duration();
                            d.FinishDays = actual.TotalDays;
                        }
                        db.Budgets.Add(d);
                        db.SaveChanges();
                        i++;
                    }
                }
            };

            //if db.Users.Salary   not null => SUM salary*days
            //if salary null project manager daily salary 500$ developer 200$
            var groups = db.Budgets
            .GroupBy(n => n.ProjectId)
            .Select(n => new
            {
                ProjectId = n.Key,
                ProjectCount = n.Count(),
            }
            )
            /*.OrderBy(n => n.Project)*/.ToList();

            var days = db.Budgets.DistinctBy(n => n.ProjectId).Select(n => new { n.ProjectId, n.days }).ToList();

            //        IEnumerable<object> all1 = ((IEnumerable<object>)groups).Concat(days.Cast<object>());
            double companyTotal = 0;
            var expectBudget = new List<Tuple<Guid,double>>();
  
            foreach (var g in groups)
            {
                foreach (var d in days)
                {
                    if (g.ProjectId == d.ProjectId)
                    {
                        var projectBudget = 1 * d.days * 500 + (g.ProjectCount - 1) * d.days * 200;
                        expectBudget.Add(new Tuple<Guid,double>(d.ProjectId, projectBudget));
                        companyTotal = companyTotal + projectBudget;
                        
                    }
                }
            }
            ViewBag.ExpectBudget = expectBudget;
            ViewBag.ProjectLists = db.Projects;

            //real budget
            var finishDays = db.Budgets.DistinctBy(n => n.ProjectId).Select(n => new { n.ProjectId, n.FinishDays, n.days }).ToList();
            var realBudget = new List<Tuple<Guid, double?, double>>();
            double? companyTotal2 = 0;
            foreach (var g in groups)
            {
                foreach (var d in finishDays)
                {
                    if (g.ProjectId == d.ProjectId)
                    {
                        var projectBudget2 = 1 * d.FinishDays * 500 + (g.ProjectCount - 1) * d.FinishDays * 200;
                        var projectBudget3 = 1 * d.days * 500 + (g.ProjectCount - 1) * d.days * 200;
                        if (projectBudget2 > projectBudget3)
                        {
                            realBudget.Add(new Tuple<Guid, double?, double>(d.ProjectId, projectBudget2, projectBudget3));
                            companyTotal2 = companyTotal2 + projectBudget2;
                        }
                    }
                }
            }
            ViewBag.RealBudget = realBudget;

            // var expectCost= groups.Where(n=>days.Any(m=>m.))

            //for real budget about finish over deadline


            //1st : how many staff ? join table => hard to manage

            //  List<TaskHelper> staffs = new List<TaskHelper>();
            //var result1 = from p in db.Projects
            //              join u in db.UserProjects on p.Id equals u.Project_Id
            //              join t in db.TaskHelpers on u.Project_Id equals t.ProjectTask_Id
            //              select new UPTNotification { Projects = p, TaskHelpers = t,UserProjects =u };
            //foreach (var many in result1)
            //{
            //    if(many.Projects.CreatorId == many.UserProjects.ApplicationUser_Id
            //};

            //dictionary ,, key is same and hard to manage
            //Dictionary<Guid, Guid> staffs = new Dictionary<Guid, Guid>();
            //foreach (var pro in db.Projects)
            //{
            //    foreach (var task in db.TaskHelpers)
            //    {
            //        if (pro.Id == task.ProjectTask_Id && pro.CreatorId != task.CreatorId)
            //        {
            //            staffs.Add(pro.Id, task.CreatorId);
            //        }
            //    }
            //    staffs.Add(pro.Id, pro.CreatorId);
            //}
            ////count staff number
            //Dictionary<Guid, int> staffNum = new Dictionary<Guid, int>();
            //var categoryGroups = staffs.GroupBy(g=>g.Key);
            //foreach(var g in categoryGroups)
            //{
            //    staffNum.Add(g.Key, g.Count());
            //}

            ////2nd : how many days?
            //Dictionary<Guid, double> expectDays = new Dictionary<Guid, double>();
            //Dictionary<Guid, double> actualDays = new Dictionary<Guid, double>();
            //foreach (var day in db.Projects)
            //{
            //    TimeSpan expect = (day.Deadline - day.CreateTime);
            //    //FinishedTime =FinishedTime.has
            //    //TimeSpan actual = (day.FinishedTime - day.CreateTime);
            //    expectDays.Add(day.Id, expect.TotalDays);
            //    if (day.IsFinished == true && day.FinishedTime != null)
            //    {
            //        //int? l = lc.HasValue ? (int)lc.Value : (int?)null;
            //        //actual = actual.HasValue ? (TimeSpan)actual.Value : (TimeSpan?)null;
            //        //actual = (TimeSpan)actual.Value;
            //        var fin = (DateTime)day.FinishedTime;
            //        TimeSpan actual = (fin - day.CreateTime);
            //        actualDays.Add(day.Id, actual.TotalDays);
            //    }
            //}
            //3rd : calculate budget
            //3-1 : catogorize project groups

            //3-2 : costs = salary*days*project staffs

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