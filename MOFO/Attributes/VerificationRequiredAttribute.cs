using MOFO.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Routing;

namespace MOFO.Attributes
{
    public class VerificationRequiredAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        private readonly ITeacherService _teacherService;
        private readonly IModeratorService _moderatorService;
        public VerificationRequiredAttribute(ITeacherService teacherService, IModeratorService moderatorService)
        {
            _teacherService = teacherService;
            _moderatorService = moderatorService;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var urls = new List<string>() { "/error/verificationModerator", "/error/verificationTeacher", "/account/logoff" };

            var isModerator = (filterContext.RequestContext.HttpContext.User.Identity.GetUserId());
            if (!urls.Any(x => x.ToLower() == filterContext.HttpContext.Request.Url.AbsolutePath.ToLower())&& (filterContext.RequestContext.HttpContext.User.IsInRole("Moderator")|| filterContext.RequestContext.HttpContext.User.IsInRole("Teacher")))
            {
                if (filterContext.RequestContext.HttpContext.User.IsInRole("Moderator"))
                {
                    if (!_moderatorService.IsVerifiedByUserId(filterContext.RequestContext.HttpContext.User.Identity.GetUserId()))
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                        {
                            controller = "Error",
                            action = "verificationModerator"
                        }));
                    }
                }
               

                base.OnActionExecuting(filterContext);
            }
        }
    }
}