using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReportWebApp01.Models;

namespace ReportWebApp01.Controllers
{
    public class StepsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Steps
        public ActionResult Index()
        {
            var steps = db.Steps.Include(s => s.Employee);
            return View(steps.ToList());
        }

        // GET: Steps/Details/5
        public ActionResult Details(int? EmployeeID, DateTime? ActDate)
        {
            if (EmployeeID == null || ActDate == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Steps steps = db.Steps.Find(EmployeeID, ActDate);
            if (steps == null)
            {
                return HttpNotFound();
            }
            return View(steps);
        }

        // GET: Steps/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeID = new SelectList(db.Employees, "Id", "Nickname");
            return View();
        }

        // POST: Steps/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,ActDate,Value")] Steps steps)
        {
            if (ModelState.IsValid)
            {
                db.Steps.Add(steps);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = new SelectList(db.Employees, "Id", "Nickname", steps.EmployeeID);
            return View(steps);
        }

        /* カスタムCreate */
        // GET: Steps/Create
        public ActionResult Create2()
        {
            ViewBag.autoOpen = "false";

            ViewBag.Employees = db.Employees.Include(e => e.Department).ToList();

            return View();
        }

        // POST: Steps/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2([Bind(Include = "EmployeeID,ActDate,Value")] Steps steps, string autoOpen, string SearchString, string currentFilter, int? page)
        {
            if (ModelState.IsValid && String.IsNullOrEmpty(autoOpen))
            {
                db.Steps.Add(steps);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            var employees = db.Employees.Include(e => e.Department);

            // 検索フィルター
            if (!String.IsNullOrEmpty(SearchString))
            {
                employees = employees.Where(e => e.Nickname.Contains(SearchString));
            }
            
            // Dialogが開いた状態で来た場合、モデルのエラーは削除
            if (!String.IsNullOrEmpty(autoOpen))
            {                
                foreach (var key in ModelState.Keys) ModelState[key].Errors.Clear();
                ViewBag.autoOpen = "true";                
            }
            else
            {
                ViewBag.autoOpen = "false";
            }



            ViewBag.Employees = employees.ToList();
            steps.Employee = db.Employees.Find(steps.EmployeeID);

            return View(steps);
        }
        /* カスタムCreate */

        /****** カスタムCreate ******/
        public ActionResult Create3()
        {
            ViewBag.EmployeeID = new SelectList(db.Employees, "Id", "Nickname");
            return View();            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create3([Bind(Include = "EmployeeID,ActDate,Value")] Steps steps)
        {
            if (ModelState.IsValid)
            {
                db.Steps.Add(steps);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = new SelectList(db.Employees, "Id", "Nickname", steps.EmployeeID);
            return View(steps);
        }
        /****** カスタムCreate ******/

        // GET: Steps/Edit/5
        public ActionResult Edit(int? EmployeeID, DateTime? ActDate)
        {
            if (EmployeeID == null || ActDate == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Steps steps = db.Steps.Find(EmployeeID, ActDate);
            if (steps == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "Id", "Nickname", steps.EmployeeID);
            return View(steps);
        }

        // POST: Steps/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,ActDate,Value")] Steps steps)
        {
            if (ModelState.IsValid)
            {
                db.Entry(steps).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "Id", "Nickname", steps.EmployeeID);
            return View(steps);
        }

        // GET: Steps/Delete/5
        public ActionResult Delete(int? EmployeeID, DateTime? ActDate)
        {
            if (EmployeeID == null || ActDate == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Steps steps = db.Steps.Find(EmployeeID, ActDate);
            if (steps == null)
            {
                return HttpNotFound();
            }
            return View(steps);
        }

        // POST: Steps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? EmployeeID, DateTime? ActDate)
        {
            Steps steps = db.Steps.Find(EmployeeID, ActDate);
            db.Steps.Remove(steps);
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
