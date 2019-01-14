using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using ReportWebApp01.Models;

namespace ReportWebApp01.Controllers
{
    public class EmployeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int pageSize = 5;

        // GET: Employees
        public ActionResult Index()
        {
            var employees = db.Employees.Include(e => e.Department).OrderBy(e => e.Id);
            return View(employees.ToList());
        }
        
        public void ExportToExcel()
        {
            var employees = db.Employees.Include(e => e.Department).OrderBy(e => e.Id).ToList();

            ExcelPackage pck = new ExcelPackage();

            ExcelWorksheet wsEmp = pck.Workbook.Worksheets.Add("Employees");

            wsEmp.Cells["A1"].Value = "ReportName";
            wsEmp.Cells["B1"].Value = "Employees List";

            wsEmp.Cells["A2"].Value = "Date";
            wsEmp.Cells["B2"].Value = String.Format("{0:yyyy-MM-dd}", DateTime.Now);

            wsEmp.Cells["A4"].Value = "Employees";

            wsEmp.Cells["A5"].Value = "Id";
            wsEmp.Cells["B5"].Value = "Nickname";
            wsEmp.Cells["C5"].Value = "Birthday";
            wsEmp.Cells["D5"].Value = "Department";
            wsEmp.Cells["E5"].Value = "Remarks";

            int rowStart = 6;
            foreach(var item in employees)
            {
                wsEmp.Cells[String.Format("A{0}", rowStart)].Value = item.Id;
                wsEmp.Cells[String.Format("B{0}", rowStart)].Value = item.Nickname;
                wsEmp.Cells[String.Format("C{0}", rowStart)].Value = item.Birthday.ToString("yyyy-MM-dd");
                wsEmp.Cells[String.Format("D{0}", rowStart)].Value = item.Department.Department_name;
                wsEmp.Cells[String.Format("E{0}", rowStart)].Value = item.Remarks;

                rowStart++;
            }

            var departments = db.Departments.OrderBy(d => d.Id).ToList();

            ExcelWorksheet wsDep = pck.Workbook.Worksheets.Add("Departments");

            wsDep.Cells["A1"].Value = "ReportName";
            wsDep.Cells["B1"].Value = "Departments List";

            wsDep.Cells["A2"].Value = "Date";
            wsDep.Cells["B2"].Value = String.Format("{0:yyyy-MM-dd}", DateTime.Now);

            wsDep.Cells["A4"].Value = "Departments";

            wsDep.Cells["A5"].Value = "Id";
            wsDep.Cells["B5"].Value = "DepartmentName";
            wsDep.Cells["C5"].Value = "Description";

            rowStart = 6;
            foreach(var item in departments)
            {
                wsDep.Cells[String.Format("A{0}", rowStart)].Value = item.Id;
                wsDep.Cells[String.Format("B{0}", rowStart)].Value = item.Department_name;
                wsDep.Cells[String.Format("C{0}", rowStart)].Value = item.Description;

                rowStart++;
            }
            
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment;filename=FatabitExport-" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
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

        [HttpGet]
        public ActionResult AjaxTest2()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Department_name");
            ViewBag.Emps = db.Employees.OrderBy(e => e.Id).Take(pageSize);



            return View();
        }

        [HttpGet]
        public ActionResult AjaxTest2Result(string search, int page, string act)
        {
            Thread.Sleep(500);

            if (page < 0)
            {
                return Content("minpage");
            }

            if(act == "next")
            {
                page = page + 1;
            }
            else if(act == "prev")
            {
                page = page - 1;
            }
            else
            {

            }

            var emps = from e in db.Employees select e;
            
            if (!String.IsNullOrEmpty(search))
            {
                emps = emps.Where(e => e.Nickname.Contains(search));
            }

            emps = emps.OrderBy(e => e.Id).Skip(page * pageSize).Take(pageSize);

            if (emps.Count() == 0)
            {
                if (act == "next")
                {
                    return Content("maxpage");
                }                   
            }

            string jsonString = JsonConvert.SerializeObject(emps.ToList());

            return Content(jsonString);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxTest2([Bind(Include = "Id,Nickname,Birthday,DepartmentId,Remarks")] Employee employee)
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Department_name");
            ViewBag.Emps = db.Employees;

            return View();
        }

        [HttpGet]
        public ActionResult AjaxTest3Result1(string search)
        {
            Thread.Sleep(1000);

            var emps = from e in db.Employees select e;

            if (!String.IsNullOrEmpty(search))
            {
                emps = emps.Where(e => e.Nickname.Contains(search));
            }

            string jsonString = JsonConvert.SerializeObject(emps.ToList());

            return Content(jsonString);
        }

        [HttpGet]
        public ActionResult AjaxTest3Result2(string search)
        {
            Thread.Sleep(1000);

            var emps = from e in db.Employees select e;

            if (!String.IsNullOrEmpty(search))
            {
                emps = emps.Where(e => e.Nickname.Contains(search));
            }

            string jsonString = JsonConvert.SerializeObject(emps.ToList());

            return Content(jsonString);
        }

        [HttpPost]
        public ActionResult AjaxTest3Result3(string search)
        {
            Thread.Sleep(1000);

            var emps = from e in db.Employees select e;

            if (!String.IsNullOrEmpty(search))
            {
                emps = emps.Where(e => e.Nickname.Contains(search));
            }

            string jsonString = JsonConvert.SerializeObject(emps.ToList());

            return Content(jsonString);
        }

        [HttpGet]
        public ActionResult AjaxTest3()
        {
            ViewBag.Emps = db.Employees.OrderBy(e => e.Id).ToList();

            return View();
        }

        [HttpPost, ActionName("AjaxTest3")]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxTest3Post()
        {
            ViewBag.Emps = db.Employees.OrderBy(e => e.Id).ToList();

            return View();
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
