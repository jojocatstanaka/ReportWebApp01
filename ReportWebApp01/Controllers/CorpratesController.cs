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
    public class CorpratesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Corprates
        public ActionResult Index()
        {
            return View(db.Corprates.ToList());
        }

        // GET: Corprates/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Corprate corprate = db.Corprates.Find(id);
            if (corprate == null)
            {
                return HttpNotFound();
            }
            return View(corprate);
        }

        // GET: Corprates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Corprates/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Corp_code,Region_code,Corp_name,Corp_secure_code")] Corprate corprate)
        {
            if (ModelState.IsValid)
            {
                db.Corprates.Add(corprate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(corprate);
        }

        // GET: Corprates/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Corprate corprate = db.Corprates.Find(id);
            if (corprate == null)
            {
                return HttpNotFound();
            }
            return View(corprate);
        }

        // POST: Corprates/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Corp_code,Region_code,Corp_name,Corp_secure_code")] Corprate corprate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(corprate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(corprate);
        }

        // GET: Corprates/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Corprate corprate = db.Corprates.Find(id);
            if (corprate == null)
            {
                return HttpNotFound();
            }
            return View(corprate);
        }

        // POST: Corprates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Corprate corprate = db.Corprates.Find(id);
            db.Corprates.Remove(corprate);
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
