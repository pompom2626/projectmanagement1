using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcDemo.Models;

namespace MvcDemo.Controllers
{
    public class TaskHelpersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TaskHelpers
        public ActionResult Index()
        {
            var taskHelpers = db.TaskHelpers.Include(t => t.Project);
            return View(taskHelpers.ToList());
        }

        // GET: TaskHelpers/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskHelper taskHelper = db.TaskHelpers.Find(id);
            if (taskHelper == null)
            {
                return HttpNotFound();
            }
            return View(taskHelper);
        }

        // GET: TaskHelpers/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectTitle");
            return View();
        }

        // POST: TaskHelpers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content,StaffId,ProjectId,Deadline,CreateTime,FinishTime,IsFinished,Priority,Status")] TaskHelper taskHelper)
        {
            if (ModelState.IsValid)
            {
                taskHelper.Id = Guid.NewGuid();
                db.TaskHelpers.Add(taskHelper);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectTitle", taskHelper.Project_Id);
            return View(taskHelper);
        }

        // GET: TaskHelpers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskHelper taskHelper = db.TaskHelpers.Find(id);
            if (taskHelper == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectTitle", taskHelper.Project_Id);
            return View(taskHelper);
        }

        // POST: TaskHelpers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,StaffId,ProjectId,Deadline,CreateTime,FinishTime,IsFinished,Priority,Status")] TaskHelper taskHelper)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taskHelper).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectTitle", taskHelper.Project_Id);
            return View(taskHelper);
        }

        // GET: TaskHelpers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskHelper taskHelper = db.TaskHelpers.Find(id);
            if (taskHelper == null)
            {
                return HttpNotFound();
            }
            return View(taskHelper);
        }

        // POST: TaskHelpers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TaskHelper taskHelper = db.TaskHelpers.Find(id);
            db.TaskHelpers.Remove(taskHelper);
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
