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
        private readonly ICardService _cardService;
        public AdminController(ISchoolService schoolService, IModeratorService moderatorService, IRoomService roomService, ICardService cardService)
        {
            _schoolService = schoolService;
            _moderatorService = moderatorService;
            _roomService = roomService;
            _cardService = cardService;
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
        public ActionResult City(int id)
        {
            var city = _schoolService.GetCityById(id);
            if (city != null)
            {
                ViewBag.CityId = city.Id;
                ViewBag.CityName = city.Name;
                ViewBag.Status = city.IsVerified ? "ОДОБРЕНО" : "НЕОДОБРЕНО";
                ViewBag.IsVerified = city.IsVerified;
                return View();
            }
            return RedirectToAction("Cities", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCity(string name)
        {
            name = name.Trim();
            var city = new City()
            {
                Name = name,
                IsVerified = true
            };
            _schoolService.AddCity(city);
            return Json(new { status = "OK" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmCity(int cityId)
        {
            var city = _schoolService.GetCityById(cityId);
            if (city != null)
            {
                _schoolService.VerifyCityById(city.Id);
                return Json(new { status = "OK" });
            }
            return Json(new { status = "ERR" });
        }
        public ActionResult GetCities()
        {
            var cities = _schoolService.GetAllCities();
            return Json(new { status = "OK", cities = cities.Select(x => new { id = x.Id, name = x.Name, status = x.IsVerified?"Одобрено":"Неодобрено",cityUrl= "/admin/city?id=" + x.Id }) }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSchoolsInCity(int cityId)
        {
            var schools = _schoolService.GetSchoolsByCityId(cityId);
            return Json(new { status = "OK", schools = schools.Select(x => new { id = x.Id, schoolName = x.Name, city=x.City.Name, status=x.IsVerified?"Одобрено":"Неодобрено", schoolUrl = "/admin/school?id=" + x.Id }) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchSchools(string schoolName = null, int status = 0, int cityId = 0)
        {
            var schools = _schoolService.SearchSchool(schoolName, cityId, status);
            return Json(new { status = "OK", schools = schools.Select(x => new { schoolName = x.Name, status = x.IsVerified ? "Одобрено" : "Неодобрено", city = x.City.Name, schoolUrl = "/admin/school?id=" + x.Id }) }, JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MergeCity(int currentCityId, int mergeCityId)
        {
            var currentCity = _schoolService.GetCityById(currentCityId);
            var mergeCity = _schoolService.GetCityById(mergeCityId);
            if (currentCity != null && mergeCity != null)
            {
                var mergeCitySchools = _schoolService.GetSchoolsByCityId(mergeCityId);
                if (mergeCitySchools != null)
                {
                    foreach (var school in mergeCitySchools)
                    {
                        school.City = currentCity;
                        _roomService.SaveChanges();
                    }
                }
                _schoolService.RemoveCity(mergeCity);
                return Json(new { status = "OK" });
            }
            return Json(new { status = "ERR" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoom (int roomId)
        {
            var room = _roomService.GetRoomById(roomId);
            if (room != null)
            {
                room.Cards.Clear();
                _roomService.SaveChanges();
                _roomService.RemoveRoom(room);
                return Json(new { status = "OK" });
            }
            return Json(new { status = "ERR" });
        }
        public ActionResult SearchCitiesModal(string query, int currentCityId)
        {
            var result = new List<object>();
            var currentCity = _schoolService.GetCityById(currentCityId);
            if (!string.IsNullOrEmpty(query))
            {
                query = query.Trim();
                var newQuery = query.ToLower();
                var cities = _schoolService.SearchCity(newQuery, 2);
                cities.Remove(currentCity);
                foreach (var city in cities)
                {
                    result.Add(new { id = city.Id, text = city.Name });
                }
            }
            else
            {
                var cities = _schoolService.SearchCity("", 2);
                cities.Remove(currentCity);
                foreach (var city in cities)
                {
                    result.Add(new { id = city.Id, text = city.Name });
                }
            }
            return Json(new { results = result }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult SearchCities(string cityName = null, int status = 0)
        {
            var cities = _schoolService.SearchCity(cityName, status);
            return Json(new { status = "OK", cities = cities.Select(x => new { cityName = x.Name, status = x.IsVerified ? "Одобрен" : "Неодобрен", cityUrl = "/admin/city?id=" + x.Id }) }, JsonRequestBehavior.AllowGet);
        }
    }
}