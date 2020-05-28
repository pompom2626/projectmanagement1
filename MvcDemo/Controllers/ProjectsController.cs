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
        public ActionResult Index(/*string whoId,*/ Guid? ProjectId)
        {
            string whoId = User.Identity.GetUserId();
            int numTasks = db.TaskHelpers.Count();

           // var viewModel = new UPTNotification();
          
             ViewBag.Projects = db.Projects
            .Where(c => c.UserProjects.Any(d => d.ApplicationUser_Id == whoId))
            .OrderBy(i => i.Priority).ToList();
            
            ////if (ProjectId != null)
            ////{

            //    viewModel.TaskHelpers =  db.Projects
            //.Where(c => c.TaskHelpers.Any(d => d.ProjectTask_Id == ProjectId)).FirstOrDefault().TaskHelpers
            //.OrderBy(i => i.Priority);

            ////}

            //if (ProjectId == null)
            //{
            //    var viewModel = from p in db.Projects
            //                     join u in db.UserProjects on p.Id equals u.Project_Id
            //                     where u.ApplicationUser_Id == whoId  //project for only login user and tasks for all user under the project
            //                     orderby p.CustomerKey, k.StateProvinceName
            //                     select new UPTNotification { Projects = p };
            //    ViewBag.ProjectId = ProjectId;
            //    ViewBag.NumTasks = numTasks;
            //    ModelState.Clear();
            //    return View(viewModel);
            //}
      //if (ProjectId != null && numTasks == 0)
      //      {
      //          var viewModel = from p in db.Projects
      //                          join u in db.UserProjects on p.Id equals u.Project_Id
      //                          where u.ApplicationUser_Id == whoId  //project for only login user and tasks for all user under the project
      //                         // orderby p.CustomerKey, k.StateProvinceName
      //                          select new UPTNotification { Projects = p };
      //          ViewBag.ProjectId = ProjectId;
      //          ViewBag.NumTasks = numTasks;
      //          ModelState.Clear();
      //          return View(viewModel);
      //      }
      //      else
      //      {
                var viewModel = from p in db.Projects
                                join u in db.UserProjects on p.Id equals u.Project_Id
                                join t in db.TaskHelpers on u.Project_Id equals t.ProjectTask_Id
                                where u.ApplicationUser_Id == whoId  //project for only login user and tasks for all user under the project
                                orderby p.Deadline, t.Priority
                                select new UPTNotification { Projects = p, TaskHelpers = t };
                ViewBag.ProjectId = ProjectId;
                ViewBag.NumTasks = numTasks;

               // ModelState.Clear();
                return View(viewModel);
            //}


            //2list linq =>
            //var UserID = User.Identity.GetUserId();
            //var joinList = db.UserProjects.Where(u => u.ApplicationUser_Id == whoId).ToList();
            //List<Project> projectList = new List<Project>();
            //foreach(var pro in joinList)
            //{

            //}

            //List<Project> projectList = new List<Project>();
            //foreach (var pro1 in db.Projects)
            //{
            //   if(pro1.Id ==joinList.proj)
            //}


            //viewmodel version1
            // List<ProjectTaskUserView> viewModel = new List<ProjectTaskUserView>();

            // var result1 = from a in db.Projects
            //              join b in db.UserProjects on a.Id equals b.Project_Id
            //              select new
            //              {
            //                  CreateTime = a.CreateTime,
            //                  Deadline = a.Deadline,
            //                  IsFinished = a.IsFinished,
            //                  ProjectTitle = a.ProjectTitle,
            //                  ProjectContent = a.ProjectContent,
            //                  Priority = a.Priority,
            //                  ApplicationUser_Id = b.ApplicationUser_Id
            //              };
            // var result2 = result1.Where(r => r.ApplicationUser_Id == UserID).ToList();
            // foreach (var a in result2) //retrieve each item and assign to model
            // {
            //     viewModel.Add(new ProjectTaskUserView()
            //     {
            //         CreateTime = a.CreateTime,
            //         Deadline = a.Deadline,
            //         IsFinished = a.IsFinished,
            //         ProjectTitle = a.ProjectTitle,
            //         ProjectContent = a.ProjectContent,
            //         Priority = a.Priority, 
            //         ApplicationUser_Id = a.ApplicationUser_Id
            //     });
            // }

            //ViewBag.Tasks = new SelectList(db.Projects, "Id", "ProjectTitle");

            //   get related tasks for the selected project
            //var result3 = from a in db.Projects
            //              join b in db.TaskHelpers on a.Id equals b.Project.Id
            //              select new
            //              {
            //                 ProId = a.Id,
            //                 CreateTime = b.CreateTime,
            //                 Deadline = b.Deadline,
            //                 IsFinished = b.IsFinished,
            //                 Title = b.Title,
            //                 Priority = b.Priority,
            //                 CreatorId = b.CreatorId,

            //             };
            //var result4= result3.Where(r => r.ProId == ).ToList();

        //    return View(viewModel);
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
