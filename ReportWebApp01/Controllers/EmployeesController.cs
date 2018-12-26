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
    public class EmployeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Employees
        public ActionResult Index()
        {
            var employees = db.Employees.Include(e => e.Department).OrderBy(e => e.Id);
            return View(employees.ToList());
        }
        
        // GET: Employees
        public ActionResult Paging(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 0;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.currentFilter = searchString;

            int pageSize = 5;
            int pageNumber = (page ?? 0);
            ViewBag.currentPage = (page ?? 0);

            // ページがマイナスを指定されたらエラー。
            if (pageNumber < 0)
            {
                return View("Error");
            }

            var employees = db.Employees.Include(e => e.Department);

            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.Nickname.Contains(searchString));
            }

            // 最大ページ番号を取得
            int maxPage = employees.Count() / pageSize;
            // 最終ページにレコードがあるかどうかチェック。無ければ最大ページを－１。
            // ただし、maxPageが0なら、最大ページ番号は0のままでOK。
            if ( (maxPage != 0) && (employees.OrderBy(e => e.Id).Skip(maxPage * pageSize).Count()) == 0 )
            {
                maxPage--;
            }
            
            // ページが最大ページ番号より大きい場合はエラー
            if (pageNumber > maxPage)
            {
                return View("Error");
            }

            ViewBag.maxPage = maxPage;

            employees = employees.OrderBy(e => e.Id).Skip(pageNumber * pageSize).Take(pageSize);
                        
            return View(employees);
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        
        public ActionResult RadioList(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            ViewBag.Names = from e in db.Employees
                            select new SelectListItem { Value = e.Id.ToString(), Text = e.Nickname };

            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Department_name");
            return View();
        }

        // POST: Employees/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nickname,Birthday,DepartmentId,Remarks")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Department_name", employee.DepartmentId);
            return View(employee);
        }

        /********* カスタマイズCreate Start *********/
        // GET: Employees/Create
        public ActionResult Create2()
        {
            ViewData["departments"] = db.Departments.ToList();

            return View();
        }

        // POST: Employees/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2([Bind(Include = "Id,Nickname,Birthday,DepartmentId,Remarks")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewData["departments"] = db.Departments.ToList();

            return View(employee);
        }
        /********* カスタマイズCreate End *********/

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Department_name", employee.DepartmentId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nickname,Birthday,DepartmentId,Remarks")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Department_name", employee.DepartmentId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /****** Partial View Test ******/
        public ActionResult TestView(string SearchString)
        {
            ViewBag.SearchString = SearchString;

            return View();
        }
        
        [ChildActionOnly]
        public ActionResult EmployeesList(string SearchString)
        {
            var employees = db.Employees.Include(e => e.Department);

            if (!String.IsNullOrEmpty(SearchString))
            {
                employees = employees.Where(e => e.Nickname.Contains(SearchString));
            }

            employees = employees.OrderBy(e => e.Id);

            return PartialView("_EmployeesList", employees);
        }
        /**/

        /* Ajax Test */
        public ActionResult AjaxTest()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Department_name");
            return View();
        }

        /*
        public ActionResult AjaxSearch(int? id)
        {

            if ( Request.IsAjaxRequest())
            {
                var employees = db.Employees.Include(e => e.Department).Where(e => e.DepartmentId == id);
                return PartialView("_AjaxSearch", employees);
            }

            return Content("Ajax以外の通信はできません。");
        }
        */

        public ActionResult AjaxSearch(string id)
        {

            if (Request.IsAjaxRequest())
            {
                var employees = db.Employees.Include(e => e.Department);

                if (!String.IsNullOrEmpty(id))
                {
                    employees = employees.Where(e => e.Nickname.Contains(id) || e.Remarks.Contains(id));
                }
                
                return PartialView("_AjaxSearch", employees.OrderBy(e => e.Id));
            }

            return Content("Ajax以外の通信はできません。");
        }

        /* */
        [HttpGet]
        public ActionResult ListEmp()
        {
                        
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ListEmp(int[] Id)
        {
            if (Id != null)
            {
                List<Employee> SelectedEmps = new List<Employee>();

                foreach(int eid in Id)
                {
                    Employee emp = db.Employees.Find(eid);
                    if(emp != null)
                    {
                        SelectedEmps.Add(emp);
                    }
                }

                ViewBag.SelectedEmps = SelectedEmps;
            }
            else
            {
                ModelState.AddModelError("", "ユーザを選択してください。");
            }
            

            return View();
        }

        /********* カスタマイズCreate Start *********/

        public ActionResult CreateAjax()
        {
            return View();
        }

        /********* カスタマイズCreate End *********/

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
