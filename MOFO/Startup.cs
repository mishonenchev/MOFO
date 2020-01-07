using Microsoft.Owin;
using Hangfire;
using Hangfire.Dashboard;
using Ninject;
using Ninject.Web.Common;
using MOFO.Services.Contracts;
using MOFO.Services;
using MOFO.Database.Repositories;
using MOFO.Database.Contracts;
using MOFO.Database;

using Owin;

[assembly: OwinStartupAttribute(typeof(MOFO.Startup))]
namespace MOFO
{
    public partial class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            StandardKernel kernel = new StandardKernel();
            kernel
              .Bind<IDatabase>()
              .To<MOFO.Database.Database>()
          .InBackgroundJobScope();
            kernel
                .Bind<IUserRepository>()
                .To<UserRepository>()
            .InBackgroundJobScope();
            kernel
                .Bind<IFileRepository>()
                .To<FileRepository>()
            .InBackgroundJobScope();
            kernel
                .Bind<ICardRepository>()
                .To<CardRepository>()
            .InBackgroundJobScope();
            kernel
                .Bind<IRoomRepository>()
                .To<RoomRepository>()
            .InBackgroundJobScope();
            kernel
               .Bind<ISessionRepository>()
               .To<SessionRepository>()
           .InBackgroundJobScope();
            kernel
              .Bind<ISessionHistoryRepository>()
              .To<SessionHistoryRepository>()
          .InBackgroundJobScope();
            kernel
             .Bind<ITeacherRepository>()
             .To<TeacherRepository>()
         .InBackgroundJobScope();
            kernel
             .Bind<IModeratorRepository>()
             .To<ModeratorRepository>()
         .InBackgroundJobScope();
            kernel
             .Bind<ISchoolRepository>()
             .To<SchoolRepository>()
         .InBackgroundJobScope();
            kernel
             .Bind<IStudentRepository>()
             .To<StudentRepository>()
         .InBackgroundJobScope();
            kernel
           .Bind<IMessageRepository>()
           .To<MessageRepository>()
         .InBackgroundJobScope();
            kernel
         .Bind<ICityRepository>()
         .To<CityRepository>()
       .InBackgroundJobScope();
            kernel
                .Bind<IUserService>()
                .To<UserService>()
           .InBackgroundJobScope();
            kernel
                .Bind<IRoomService>()
                .To<RoomService>()
           .InBackgroundJobScope();
            kernel
                .Bind<IMessageService>()
                .To<MessageService>()
           .InBackgroundJobScope();
            kernel
                .Bind<ISessionService>()
                .To<SessionService>()
           .InBackgroundJobScope();
            kernel
                 .Bind<ISchoolService>()
                 .To<SchoolService>()
            .InBackgroundJobScope();
            kernel
                .Bind<ICardService>()
                .To<CardService>()
           .InBackgroundJobScope();
            kernel
                .Bind<IModeratorService>()
                .To<ModeratorService>()
           .InBackgroundJobScope();
            kernel
               .Bind<ITeacherService>()
               .To<TeacherService>()
          .InBackgroundJobScope();
            kernel
               .Bind<IStudentService>()
               .To<StudentService>()
          .InBackgroundJobScope();
            kernel
              .Bind<DataProcessing.Contracts.IPdfBuilder>()
              .To<DataProcessing.PdfBuilder>()
         .InBackgroundJobScope();

            GlobalConfiguration.Configuration.UseNinjectActivator(kernel);
            ConfigureAuth(app);
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("DefaultConnection");

            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new[] { new AdminAutorization() }
            }
            );
            app.UseHangfireServer();
        }
         class AdminAutorization : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                // In case you need an OWIN context, use the next line, `OwinContext` class
                // is the part of the `Microsoft.Owin` package.
                var owinContext = new OwinContext(context.GetOwinEnvironment());

                // Allow all authenticated users to see the Dashboard (potentially dangerous).
                return owinContext.Authentication.User.IsInRole("Admin");
            }
        }
    }
}
