﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWebApp01.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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
        
        public ActionResult JsTest()
        {
            

            return View();
        }

        public ActionResult JsTest2()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult CurrentTime()
        {
            return PartialView("_CurrentTimePartial", DateTime.Now);
        }

        public ActionResult Helper()
        {
            return View();
        }
    }
}