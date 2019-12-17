using MOFO.Models;
using MOFO.Services;
using MOFO.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MOFO.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public JsonResult RegisterUser(string name)
        {
            var auth = _userService.NewAuthString();
            _userService.AddUser(new User() { Name = name, Auth = auth, Role = UserRole.Student, IsActive = true, DateTimeLastActive = DateTime.Now });
            return Json(new { status = "OK", auth = auth }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAccountInfo(string auth)
        {
            var user = _userService.GetUserByAuth(auth);
            if (user != null)
            {
                return Json(new { status = "OK", userName = user.Name }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { status = "ERR" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateAccountInfo(string auth, string name)
        {
            var user = _userService.GetUserByAuth(auth);
            if (user != null)
            {
                user.Name = name;
                _userService.Update();
                return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { status = "ERR" }, JsonRequestBehavior.AllowGet);
        }
    }
}