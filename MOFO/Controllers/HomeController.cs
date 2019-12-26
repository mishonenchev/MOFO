using MOFO.Models;
using MOFO.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MOFO.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        public HomeController(IUserService userService)
        {
            _userService = userService;
        }
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }else if (User.IsInRole("Moderator"))
                {
                    return RedirectToAction("Index", "Moderator");
                }else if (User.IsInRole("Teacher"))
                {
                    return RedirectToAction("Index", "Teacher");
                }
                else
                {
                    return RedirectToAction("Index", "Student");
                }
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}