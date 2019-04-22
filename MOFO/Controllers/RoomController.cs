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
        public RoomController(IRoomService roomService, IUserService userService)
        {
            _roomService = roomService;
            _userService = userService;
        }
        [HttpPost]
        public JsonResult JoinRoom(string deskCode, string auth)
        {
            var room = _roomService.GetRoomByDeskCode(deskCode);
            var user = _userService.GetUserByAuth(auth);
            if (user != null)
            {
                if (room != null)
                {
                    var deskInRoom = room.Desks.Where(x => x.Code == deskCode).First();
                    if (_roomService.HasActiveSessionByRoom(room.Id))
                    {
                        deskInRoom.User = user;
                        _userService.Update();
                    }
                    else
                    {
                        _roomService.AddSession(new Session()
                        {
                            Room = room,
                            DateTimeCreated = DateTime.Now,
                            DateTimeLastActive = DateTime.Now
                        });
                        deskInRoom.User = user;
                        _userService.Update();
                    }
                    return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { status = "NO DESK" }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { status = "WRONG AUTH KEY" }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetActiveUsers(string auth)
        {
            var user = _userService.GetUserByAuth(auth);
            var allUsers = _userService.GetAll();
            //TODO: Send only active users
            return Json(new { status = "OK", users = allUsers.Select(x => new { userName = x.Name, role = x.Role }) }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Logout(string auth)
        {
            var user = _userService.GetUserByAuth(auth);
            if (user != null)
            {
                user.Session = null;
                //TODO: Remove files from session
            }
            return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
        }
    }
}