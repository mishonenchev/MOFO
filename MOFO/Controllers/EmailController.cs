using MOFO.Controllers.Contracts;
using MOFO.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MOFO.Controllers
{
    public class EmailController : Controller, IEmailController
    {
        public void ForgotPasswordEmail(ApplicationUser user, MOFO.Models.Emails.ForgotPasswordViewModel viewModel)
        {
                var emailService = new EmailService();
                var email = RenderViewToString("Email", "ForgotPassword", viewModel);
                emailService.SendAsync(new Microsoft.AspNet.Identity.IdentityMessage()
                {
                    Body = email,
                    Destination = user.Email,
                    Subject = "Възстановяване на парола в Techip.bg"
                }, "techip@akvarel.net");
            }
        public string RenderViewToString(string controllerName, string viewName, object viewData)
        {
            using (var writer = new StringWriter())
            {
                var routeData = new RouteData();
                routeData.Values.Add("controller", controllerName);
                var fakeControllerContext = new ControllerContext(new HttpContextWrapper(new HttpContext(new HttpRequest(null, "http://google.com", null), new HttpResponse(null))), routeData, new EmailController());
                var razorViewEngine = new RazorViewEngine();
                var razorViewResult = razorViewEngine.FindView(fakeControllerContext, viewName, "", false);

                var viewContext = new ViewContext(fakeControllerContext, razorViewResult.View, new ViewDataDictionary(viewData), new TempDataDictionary(), writer);
                razorViewResult.View.Render(viewContext, writer);
                return writer.ToString();

            }
        }
    }
}