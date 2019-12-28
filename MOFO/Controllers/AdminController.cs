using MOFO.Models;
using MOFO.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MOFO.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ISchoolService _schoolService;
        private readonly IModeratorService _moderatorService;
        private readonly IRoomService _roomService;
        public AdminController(ISchoolService schoolService, IModeratorService moderatorService, IRoomService roomService)
        {
            _schoolService = schoolService;
            _moderatorService = moderatorService;
            _roomService = roomService;
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
        public ActionResult School(int id)
        {
            var school = _schoolService.GetSchoolById(id);
            if (school != null)
            {
                var moderator = _moderatorService.GetModeratorBySchool(school.Id);
                var roomCount = _roomService.GetRoomsBySchool(id).Count;
                if (moderator != null)
                {
                    ViewBag.SchoolName = school.Name;
                    ViewBag.Status = school.IsVerified ? "ОДОБРЕН" : "НЕОДОБРЕН";
                    ViewBag.City = school.City.Name;
                    ViewBag.Telephone = school.Telephone;
                    ViewBag.UserNames = moderator.Name;
                    ViewBag.IsVerified = school.IsVerified;
                    ViewBag.SchoolId = school.Id;
                    ViewBag.HasRooms = roomCount > 0;
                    return View();
                }

            }
            return RedirectToAction("Schools", "Admin");

        }
        public ActionResult SearchSchools(string schoolName = null, int status = 0, int cityId = 0)
        {
            var schools = _schoolService.SearchSchool(schoolName, cityId, status);
            return Json(new { status = "OK", schools = schools.Select(x => new { schoolName = x.Name, status = x.IsVerified ? "Одобрен" : "Неодобрен", city = x.City.Name, schoolUrl = "/admin/school?id=" + x.Id }) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSchoolRooms(int schoolId)
        {
            var rooms = _roomService.GetRoomsBySchool(schoolId);
            return Json(new { status = "OK", rooms = rooms.Select(x => new { id = x.Id, name = x.Name, description = x.Description, cardCount = x.Cards.Count }) }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSchoolInfo(int schoolId)
        {
            var school = _schoolService.GetSchoolById(schoolId);
            if (school != null)
            {
                var moderator = _moderatorService.GetModeratorBySchool(schoolId);
                if (moderator != null)
                {
                    var results = new List<object>();
                    results.Add(new { fieldName = "Име на училището", fieldValue = school.Name });

                    results.Add(new { fieldName = "Населено място", fieldValue = school.City.Name });
                    results.Add(new { fieldName = "Адрес", fieldValue = school.Address });
                    results.Add(new { fieldName = "Телефон на училището", fieldValue = school.Telephone });
                    results.Add(new { fieldName = "Статус", fieldValue = school.IsVerified ? "Одобрен" : "Неодобрен" });
                    results.Add(new { fieldName = "Име на лице за контакт", fieldValue = moderator.Name });
                    results.Add(new { fieldName = "Телефон на лице за контакт", fieldValue = moderator.Telephone });
                    results.Add(new { fieldName = "Имейл на лице за контакт", fieldValue = moderator.Email });

                    return Json(new { status = "OK", items = results }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "ERR" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRoom(int schoolId, string name, string description)
        {
            var school = _schoolService.GetSchoolById(schoolId);
            if (school != null)
            {
                name = name.Trim();
                description = description.Trim();
                var room = new Room()
                {
                    Description = description,
                    Name = name,
                    School = school
                };
                _roomService.AddRoom(room);
                return Json(new { status = "OK" });
            }
            return Json(new { status = "ERR" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRoom(int roomId, string name, string description)
        {
            var room = _roomService.GetRoomById(roomId);
            if (room != null)
            {
                room.Name = name;
                room.Description = description;
                _roomService.SaveChanges();
                return Json(new { status = "OK" });

            }
            return Json(new { status = "ERR" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmSchool(int schoolId)
        {
            var school = _schoolService.GetSchoolById(schoolId);
            if (school != null)
            {
                school.IsVerified = true;
                _roomService.SaveChanges();
                return Json(new { status = "OK" });
            }
            return Json(new { status = "ERR" });

        }
        public ActionResult SearchCities(string cityName = null, int status = 0)
        {
            var cities = _schoolService.SearchCity(cityName, status);
            return Json(new { status = "OK", cities = cities.Select(x => new { cityName = x.Name, status = x.IsVerified ? "Одобрен" : "Неодобрен", cityUrl = "/admin/city?id=" + x.Id }) }, JsonRequestBehavior.AllowGet);
        }
    }
}