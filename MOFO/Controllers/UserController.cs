using MOFO.Models;
using MOFO.Services;
using MOFO.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.IO;

namespace MOFO.Controllers
{
    [Authorize (Roles ="Teacher, Student")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        public UserController(IUserService userService, IMessageService messageService)
        {
            _userService = userService;
            _messageService = messageService;
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult RegisterUser(string name)
        {
            var auth = _userService.NewAuthString();
            _userService.AddUser(new User() { Name = name, Auth = auth, Role = UserRole.Student, IsActive = true, DateTimeLastActive = DateTime.Now, DateTimeRegistered = DateTime.Now });
            return Json(new { status = "OK", auth = auth }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]

        [AllowAnonymous]
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
        [AllowAnonymous]
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
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ActiveSession()
        {
            return View();
        }
        public ActionResult SessionHistories()
        {
            return View();
        }
        public ActionResult SessionHistory(int id)
        {
            return View();
        }
        public JsonResult GetIndexContent()
        {
            var user = _userService.GetUserByUserId(User.Identity.GetUserId());
            if (user != null)
            {

                Object session = null;
                if (user.Session != null)
                {
                    var userCount = _userService.GetUsersBySession(user.Session.Id).Count;
                    session = new { usersCount = userCount, messagesCount = user.Session.Messages.Count, name = user.Session.Room.Name, description = user.Session.Room.Description };
                }
                var lastSessions = _userService.GetLastSessionHistoriesByUserId(user.Id).Where(x => x.StartDateTime != x.FinishDateTime).Take(3).ToList();

                return Json(new { status = "OK", activeSession = session, sessions = lastSessions.Select(x => new { messagesCount = x.Messages.Count, usersCount = x.Users.Count, name = x.Room.Name, description = x.Room.Description, lastActive = DateTimeUploaded(x.FinishDateTime) }) }, JsonRequestBehavior.AllowGet); ;
            }
            return Json(new { status = "ERR" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ActiveSessionSync(int lastMessage = 0)
        {
            var user = _userService.GetUserByUserId(User.Identity.GetUserId());
            if (user != null)
            {
                if (user.Session != null)
                {
                    var messages = _messageService.GetMessagesByUserSession(user.Session).OrderBy(x => x.DateTimeUploaded).ToList();
                    if (lastMessage != 0)
                    {
                        messages = messages.Where(x => x.Id > lastMessage).ToList();
                    }
                    user.DateTimeLastActive = DateTime.Now;
                    user.IsActive = true;
                    _userService.Update();
                    var usersCount = _userService.GetUsersBySession(user.Session.Id).Count;

                    return Json(new { status = "OK", usersCount, messages = messages.Select(x => new { id = x.Id, hasFile = x.File == null ? false : true, downloadCode = x.File?.DownloadCode, fileSize = x.File?.Size, fileName = x.File?.FileName, message = x.Text, username = x.User.Name, dateTimeUploaded = DateTimeUploaded(x.DateTimeUploaded) }) }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { status = "NO SESSION" }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { status = "WRONG AUTH" }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult DownloadFile(string downloadCode)
        {
            var user = _userService.GetUserByUserId(User.Identity.GetUserId());
            if (user != null)
            {
                var message = _messageService.GetMessagesByUserSession(user.Session).Where(x => x.File?.DownloadCode == downloadCode).FirstOrDefault();
                if (message != null)
                {
                    var fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/Files/") + message.File.FileName);
                    var extension = Path.GetExtension(message.File.FileName);
                    return File(fileBytes, MimeMapping.GetMimeMapping(message.File.FileName), message.File.FileName);

                }
            }
            return new EmptyResult();
        }
        private string DateTimeUploaded(DateTime time)
        {
            var timeDiff = DateTime.Now - time;
            var days = timeDiff.Days;
            var hours = timeDiff.Hours;
            var minutes = timeDiff.Minutes;
            if (days != 0)
            {
                return String.Format("{0} дни", days);
            }
            else if (hours != 0)
            {
                return String.Format("{0} часа и {1} минути", hours, minutes);
            }
            else if (minutes != 0)
            {
                return String.Format("{0} минути", minutes);
            }
            else
            {
                return String.Format("няколко секунди");
            }
        }
    }
}