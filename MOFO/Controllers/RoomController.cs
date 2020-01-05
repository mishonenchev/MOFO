using MOFO.Models;
using MOFO.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MOFO.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;
        private readonly ICardService _cardService;
        public RoomController(IRoomService roomService, IUserService userService, ISessionService sessionService, ICardService cardService)
        {
            _roomService = roomService;
            _userService = userService;
            _sessionService = sessionService;
            _cardService = cardService;
        }
        [HttpPost]
        public JsonResult JoinRoom(string deskCode, string auth)
        {
            Room room = null;
            Card card = null;
            if (deskCode.Length == 20)
            {
                room = _roomService.GetRoomByDeskQRCode(deskCode);
                card = _cardService.GetCardByQRCode(deskCode);
            }
            else
            {
                room = _roomService.GetRoomByDeskCode(deskCode);
                card = _cardService.GetCardByCode(deskCode);
            }
            var user = _userService.GetUserByAuth(auth);
            if (user != null)
            {
                if (room != null&& card!=null)
                {
                    if (card.ValidBefore > DateTime.Now)
                    {
                        if (!_sessionService.HasActiveSessionByRoom(room))
                        {
                            _sessionService.AddSession(new Session()
                            {
                                Room = room,
                                DateTimeCreated = DateTime.Now,
                                DateTimeLastActive = DateTime.Now
                            });
                        }
                        var previousUser = card.User;
                        if (previousUser != null && previousUser.Session != null)
                        {
                            previousUser.Session = null;
                        }
                        user.Session = _sessionService.GetSessionByRoom(room);
                        room.Cards.Where(x => x.User != null && x.User.Id == user.Id).ToList().ForEach(y => y.User = null);
                        card.User = user;
                        _userService.Update();
                        return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
                    }else return Json(new { status = "NO DESK" }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { status = "NO DESK" }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { status = "WRONG AUTH" }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult AuthKeyCheck(string auth)
        {
            var user = _userService.GetUserByAuth(auth);
            if (user != null)
            {
                if (user.Session != null)
                {
                    return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { status = "NO SESSION" }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { status = "WRONG AUTH" }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetActiveUsers(string auth)
        {
            var user = _userService.GetUserByAuth(auth);
            if (user != null)
            {
                if (user.Session != null)
                {
                    var allUsers = _userService.GetAll().Where(x => x.Session != null && x.Session.Id == user.Session.Id).ToList();
                    foreach (var _user in allUsers)
                    {
                        if (_user.DateTimeLastActive < DateTime.Now.AddMinutes(-5))
                        {
                            _user.IsActive = false;
                        }
                    }
                    _userService.Update();
                    var activeUsers = allUsers.Where(x => x.IsActive == true);
                    return Json(new { status = "OK", users = activeUsers.Select(x => new { userName = x.Name, role = x.Role }) }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { status = "NO SESSION" }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { status = "WRONG AUTH" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Logout(string auth)
        {
            var user = _userService.GetUserByAuth(auth);
            if (user != null)
            {
                if (user.Session != null)
                {
                    var userSession = user.Session;
                    var activeDesks = userSession.Room.Cards.Where(x => x.User != null);
                    var usersCount = activeDesks.Where(x => x.User.Session.Id == userSession.Id).ToList().Count;
                    if (usersCount > 1)
                    {
                        user.Session = null;
                        _userService.Update();
                    }
                    else if (usersCount <= 1 || user.Role == 0)
                    {
                        user.Session = null;
                        _userService.Update();
                        _sessionService.RemoveSession(userSession);
                    }
                    return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { status = "NO SESSION" }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { status = "WRONG AUTH" }, JsonRequestBehavior.AllowGet);
        }
    }
}