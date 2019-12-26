using System.Web;
using System.Web.Optimization;

namespace MOFO
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js", "~/Scripts/knockout.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));
            bundles.Add(new ScriptBundle("~/scripts/admin").Include("~/Scripts/bootstrap.min.js", "~/Scripts/popper.min.js",
                     "~/Scripts/coreui.min.js"));
            bundles.Add(new ScriptBundle("~/js/select2").Include(
                    "~/Scripts/select2.min.js"));
            bundles.Add(new ScriptBundle("~/main/registerTeacher").Include(
                     "~/Scripts/app/main/registerTeacherViewModel.js"));
            bundles.Add(new ScriptBundle("~/main/registerStudent").Include(
                     "~/Scripts/app/main/registerStudentViewModel.js"));
            bundles.Add(new ScriptBundle("~/main/registerSchool").Include(
                     "~/Scripts/app/main/registerSchoolViewModel.js"));
            bundles.Add(new ScriptBundle("~/admins/schools").Include(
                 "~/Scripts/app/admin/schoolsViewModel-{version}.js"));
            bundles.Add(new ScriptBundle("~/app/inputValidation").Include(
                     "~/Scripts/inputValidation.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/admin").Include(
                     "~/Content/Admin.min.css"));
            bundles.Add(new StyleBundle("~/css/select2").Include(
                     "~/Content/select2.min.css"));


        }
    }
}
