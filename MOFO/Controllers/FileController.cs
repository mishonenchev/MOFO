using MOFO.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace MOFO.Controllers
{
    public class FileController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;
        public FileController(IFileService fileService, IUserService userService, ISessionService sessionService)
        {
            _fileService = fileService;
            _userService = userService;
            _sessionService = sessionService;
        }
        [HttpGet]
        public JsonResult FileSync(string auth)
        {
            var user = _userService.GetUserByAuth(auth);
            if (user != null)
            {
                if (user.Session != null)
                {
                    var files = _fileService.GetFilesByUserSession(user.Session);
                    user.DateTimeLastActive = DateTime.Now;
                    user.IsActive = true;
                    _userService.Update();
                    return Json(new { status = "OK", files = files.Select(x => new { type = x.Type, downloadCode = x.DownloadCode, fileName = x.FileName, message = x.Message, username = x.User.Name, dateTimeUploaded = DateTimeUploaded(x.DateTimeUploaded) }) }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { status = "NO SESSION" }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { status = "WRONG AUTH" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UploadFile(int type, string message, string auth)
        {
            var downloadCode = _fileService.NewDownloadCode();
            var user = _userService.GetUserByAuth(auth);
            if (user != null)
            {
                if (Request.Files.Count > 0)
                {
                    if (user.Session != null)
                    {
                        var file = Request.Files[0];
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = downloadCode + Path.GetExtension(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/Files"), fileName);
                            file.SaveAs(path);
                            _sessionService.AddFile(type, fileName, downloadCode, message, user, DateTime.Now);
                            return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
                        }
                        else return Json(new { status = "ERR" }, JsonRequestBehavior.AllowGet);
                    }
                    else return Json(new { status = "NO SESSION" }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { status = "NO FILES" }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { status = "WRONG AUTH" }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult DownloadFile(string auth, string downloadCode)
        {
            var user = _userService.GetUserByAuth(auth);
            if (user != null)
            {
                var file = _fileService.GetFilesByUserSession(user.Session).Where(x=>x.DownloadCode==downloadCode).FirstOrDefault();
                if (file != null)
                {
                    //var fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/Files/") + file.FileName);
                    //var extension = Path.GetExtension(file.FileName);
                    //return File(fileBytes, MimeMapping.GetMimeMapping(file.FileName), " " + extension);

                    Response.ContentType = MimeMapping.GetMimeMapping(file.FileName);
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + file.FileName);
                    Response.TransmitFile(Server.MapPath("~/Content/Files/" + file.FileName));
                    Response.End();
                    return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { status = "NO FILE" }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { status = "WRONG AUTH" }, JsonRequestBehavior.AllowGet);
        }
            
        private string DateTimeUploaded(DateTime time)
        {
            var timeDiff = DateTime.Now - time;
            var days = timeDiff.Days;
            var hours = timeDiff.Hours;
            var minutes = timeDiff.Minutes;
            if (days!=0)
            {
                return String.Format("{0} days ago.", days);
            }
            else if (hours!=0)
            {
                return String.Format("{0} hours and {1} minutes ago.",hours , minutes);
            }
            else if (minutes!=0)
            {
                return String.Format("{0} minutes ago.", minutes);
            }
            else
            {
                return String.Format("Few moments ago.");
            }
        }
    }
}