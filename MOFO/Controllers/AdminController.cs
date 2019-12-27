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
        public ActionResult Cities()
        {
            return View();
        }
        public ActionResult SearchSchools(string schoolName = null, int status = 0, int cityId = 0)
        {
            var schools = _schoolService.SearchSchool(schoolName, cityId, status);
            return Json(new { status = "OK", schools = schools.Select(x => new { schoolName = x.Name, status = x.IsVerified? "Одобрен" : "Неодобрен", city = x.City.Name, schoolUrl = "/admin/school?id=" + x.Id})}, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchCities(string cityName = null, int status = 0)
        {
            var cities = _schoolService.SearchCity(cityName, status);
            return Json(new { status = "OK", cities = cities.Select(x => new { cityName = x.Name, status = x.IsVerified ? "Одобрен" : "Неодобрен", cityUrl = "/admin/city?id=" + x.Id }) }, JsonRequestBehavior.AllowGet);
        }
    }
}