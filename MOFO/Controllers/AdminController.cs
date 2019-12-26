using MOFO.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MOFO.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        private readonly ISchoolService _schoolService;
        public AdminController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Schools()
        {
            return View();
        }
        public ActionResult SearchSchools(string schoolName = null, int status = 0, int cityId = 0)
        {
            var schools = _schoolService.SearchSchool(schoolName, cityId, status);
            return Json(new { status = "OK", schools = schools.Select(x => new { schoolName = x.Name, status = x.IsVerified? "Одобрен" : "Неодобрен", city = x.City.Name, schoolUrl = "/admin/school?id=" + x.Id})}, JsonRequestBehavior.AllowGet);
        }
    }
}