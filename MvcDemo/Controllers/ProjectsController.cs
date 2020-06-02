using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MvcDemo.Models;
using MvcDemo.ViewModels;

namespace MvcDemo.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Projects
        public ActionResult Index(/*string whoId,*/ Guid? ProjectId )
        {
            string whoId = User.Identity.GetUserId();
            int numTasks = db.TaskHelpers.Count();

            ViewBag.Projects = db.Projects
           .Where(c => c.UserProjects.Any(d => d.ApplicationUser_Id == whoId))
           .OrderBy(i => i.Priority).ToList();

            var UserID = User.Identity.GetUserId();
            var userRoles = db.Roles.Include(r => r.Users).ToList();
            var userRoleNames = (from r in userRoles
                                 from u in r.Users
                                 where u.UserId == UserID
                                 select r.Name).ToList();
            //if project manager
            if (userRoleNames[0] == "Manager")
            {
                ViewBag.TaskView = from p in db.Projects
                                   join u in db.UserProjects on p.Id equals u.Project_Id
                                   join t in db.TaskHelpers on u.Project_Id equals t.ProjectTask_Id
                                   where u.ApplicationUser_Id == whoId  //project for only login user and tasks for all user under the project
                                   orderby p.Priority, t.Priority
                                   select new UPTNotification { Projects = p, TaskHelpers = t };
            }
            if (userRoleNames[0] == "Developer")
            {
                ViewBag.TaskView = from p in db.Projects
                                   join u in db.UserProjects on p.Id equals u.Project_Id
                                   join t in db.TaskHelpers on u.Project_Id equals t.ProjectTask_Id
                               //    where u.ApplicationUser_Id == whoId  //project for only login user and tasks for all user under the project
                                   orderby p.Priority, t.Priority
                                   select new UPTNotification { Projects = p, TaskHelpers = t };
            }

            ViewBag.ProjectId = ProjectId;
            ViewBag.NumTasks = numTasks;
            // ModelState.Clear();
            return View(/*viewModel*/);
        }

        public ActionResult ChangeIsFinished(Guid? ProjectId)
        {
            var isFinish = db.Projects.Find(ProjectId);
            if (isFinish.IsFinished == false)
            {
                isFinish.IsFinished = true;
                isFinish.FinishedTime = DateTime.Now;
            }
            else
            {
                isFinish.IsFinished = false;
                isFinish.FinishedTime = null ;
            }
            
            db.SaveChanges();

            return Redirect(Url.Action("Index", "Projects", new { ProjectId = ProjectId }));
        }


        public ActionResult ChangeTaskFinished(Guid? ProjectId)
        {
            var isFinish = db.Projects.Find(ProjectId);
            if (isFinish.IsFinished == false)
            {
                isFinish.IsFinished = true;
                isFinish.FinishedTime = DateTime.Now;
            }
            else
            {
                isFinish.IsFinished = false;
                isFinish.FinishedTime = null;
            }

            db.SaveChanges();

            return Redirect(Url.Action("Index", "Projects", new { ProjectId = ProjectId }));
        }


        // GET: Projects/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //   [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Budget,RealBudget,CreateTime,Deadline,FinishedTime,IsFinished,ProjectTitle,ProjectContent,Priority")] Project project)
        {
            project.CreateTime = DateTime.Now;
            if (ModelState.IsValid)
            {

                db.Projects.Add(project);
                //userProject table
                UserProject mTom = new UserProject();
                mTom.ApplicationUser_Id = User.Identity.GetUserId();
                mTom.Project_Id = project.Id;
                project.CreatorId = new Guid(mTom.ApplicationUser_Id);

                db.UserProjects.Add(mTom);

                //var UserID = User.Identity.GetUserId();
                // var ProjectID = project.Id;
                //db.UserProjects.Add(UserID);


                db.SaveChanges();
                return RedirectToAction("Index", "Projects");
            }

            return View();
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Budget,RealBudget,CreateTime,Deadline,FinishedTime,IsFinished,ProjectTitle,ProjectContent,Priority")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
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
