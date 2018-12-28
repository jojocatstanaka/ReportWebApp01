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
    public class FitbitAccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FitbitAccounts
        public ActionResult Index()
        {
            return View(db.FitbitAccounts.ToList());
        }

        // GET: FitbitAccounts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FitbitAccount fitbitAccount = db.FitbitAccounts.Find(id);
            if (fitbitAccount == null)
            {
                return HttpNotFound();
            }
            return View(fitbitAccount);
        }

        // GET: FitbitAccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FitbitAccounts/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "User_code,Corp_code,Nickname,DeviceList,Gift_flg,Last_sync_date,Access_token,Refresh_token,Create_date,Update_date")] FitbitAccount fitbitAccount)
        {
            if (ModelState.IsValid)
            {
                db.FitbitAccounts.Add(fitbitAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fitbitAccount);
        }

        // GET: FitbitAccounts/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FitbitAccount fitbitAccount = db.FitbitAccounts.Find(id);
            if (fitbitAccount == null)
            {
                return HttpNotFound();
            }
            return View(fitbitAccount);
        }

        // POST: FitbitAccounts/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "User_code,Corp_code,Nickname,DeviceList,Gift_flg,Last_sync_date,Access_token,Refresh_token,Create_date,Update_date")] FitbitAccount fitbitAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fitbitAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fitbitAccount);
        }

        // GET: FitbitAccounts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FitbitAccount fitbitAccount = db.FitbitAccounts.Find(id);
            if (fitbitAccount == null)
            {
                return HttpNotFound();
            }
            return View(fitbitAccount);
        }

        // POST: FitbitAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            FitbitAccount fitbitAccount = db.FitbitAccounts.Find(id);
            db.FitbitAccounts.Remove(fitbitAccount);
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
