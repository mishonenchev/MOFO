using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MOFO.Controllers
{
    [Authorize(Roles = "Moderator")]
    public class ModeratorController : Controller
    {
        // GET: Moderator
        public ActionResult Index()
        {
            return View();
        }
    }
}