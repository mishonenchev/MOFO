using MOFO.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.IO.Compression;
using Google.Cloud.Storage.V1;
using Google.Apis.Storage.v1.Data;
using Microsoft.AspNet.Identity;

namespace MOFO.Controllers
{
    public class FileController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;
        public FileController(IMessageService messageService, IUserService userService, ISessionService sessionService)
        {
            _messageService = messageService;
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
                    var messages = _messageService.GetMessagesByUserSession(user.Session);
                    user.DateTimeLastActive = DateTime.Now;
                    user.IsActive = true;
                    _userService.Update();
                    
                    return Json(new { status = "OK", files = messages.Select(x => new { type = x.Type, downloadCode = x.File?.DownloadCode, fileName = x.File?.FileName, message = x.Text, username = x.User.Name, dateTimeUploaded = DateTimeUploaded(x.DateTimeUploaded) }) }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { status = "NO SESSION" }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { status = "WRONG AUTH" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UploadFile(int type, string message, string auth)
        {
            var downloadCode = _messageService.NewDownloadCode();
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
                            
                            _sessionService.AddMessage(type, fileName, downloadCode, message, user, DateTime.Now, GetSizeString(file.ContentLength));
                            return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
                        }
                        else return Json(new { status = "ERR" }, JsonRequestBehavior.AllowGet);
                    }
                    else return Json(new { status = "NO SESSION" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (user.Session != null)
                    {
                        if (!string.IsNullOrWhiteSpace(message))
                        {
                            _sessionService.AddMessage(0, "", "", message, user, DateTime.Now, "0");
                            return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { status = "ERR" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { status = "NO SESSION" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else return Json(new { status = "WRONG AUTH" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher, Student")]
        public JsonResult FileUpload(string message)
        {
            var user = _userService.GetUserByUserId(User.Identity.GetUserId());
            if (user != null)
            {
                if (Request.Files.Count > 0)
                {
                    if (user.Session != null)
                    {
                        var file = Request.Files[0];
                        if (file != null && file.ContentLength > 0)
                        {
                            var downloadCode = _messageService.NewDownloadCode();
                            var fileName = downloadCode + Path.GetExtension(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/Files"), fileName);
                            file.SaveAs(path);

                            _sessionService.AddMessage(0, fileName, downloadCode, message, user, DateTime.Now, GetSizeString(file.ContentLength));
                            return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
                        }
                        else return Json(new { status = "ERR" }, JsonRequestBehavior.AllowGet);
                    }
                    else return Json(new { status = "NO SESSION" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        _sessionService.AddMessage(0, "", "", message, user, DateTime.Now, "0");
                        return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { status = "ERR" });
        }
        [HttpGet]
        public ActionResult DownloadFile(string auth, string downloadCode)
        {
            var user = _userService.GetUserByAuth(auth);
            if (user != null)
            {
                var message = _messageService.GetMessagesByUserSession(user.Session).Where(x=>x.File?.DownloadCode==downloadCode).FirstOrDefault();
                if (message != null)
                {
                    var fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/Files/") + message.File.FileName);
                    var extension = Path.GetExtension(message.File.FileName);
                    return File(fileBytes, MimeMapping.GetMimeMapping(message.File.FileName), " " + extension);

                    //Response.ContentType = MimeMapping.GetMimeMapping(message.File.FileName);
                    //Response.AppendHeader("Content-Disposition", "attachment; filename=" + message.File.FileName);
                    //Response.TransmitFile(Server.MapPath("~/Content/Files/" + message.File.FileName));
                    //Response.End();
                    //return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { status = "NO FILE" }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { status = "WRONG AUTH" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetArchive(int sessionId)
        {
            var sessionHistory = _sessionService.GetSessionHistoryById(sessionId);
            if (sessionHistory != null)
            {
                var messages = sessionHistory.Messages.ToList();
                string projectId = "techip";
                string bucketName = projectId + "-sessionid-" + sessionHistory.Id;
                var filepath = Server.MapPath("~/Content");
                var credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromFile(Path.Combine(filepath, @"..\App_Data\google-cloud.json"));
                // Instantiates a client.
                using (StorageClient storageClient = StorageClient.Create(credential))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                        {
                            var users = messages.Select(x => x.User).Distinct().ToList();
                            foreach (var user in users)
                            {
                                var userMessages = messages.Where(x => x.User.Id == user.Id).ToList();
                                foreach (var message in userMessages)
                                {
                                    if (message.File != null)
                                    {
                                        var localFile = archive.CreateEntry(user.Name + "/" + message.File.FileName);

                                        using (var entryStream = localFile.Open())
                                        {
                                            storageClient.DownloadObject(bucketName, message.File.FileName, entryStream);
                                        }


                                    }
                                }
                            }
                        }
                        var bytes = memoryStream.ToArray();
                        return File(bytes, "application/zip", String.Format( "techip_download_{0}_{1}.zip", sessionHistory.StartDateTime.ToString("ddMMyyyyHHmm"), sessionHistory.FinishDateTime.ToString("ddMMyyyyHHmm")));
                    }
                }
            }
            return Json(new { status = "ERR" }, JsonRequestBehavior.AllowGet);
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
        private string GetSizeString(int size )
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = size;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            string result = String.Format("{0:0.##} {1}", len, sizes[order]);
            return result;
        }
    }
}