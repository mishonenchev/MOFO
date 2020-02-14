[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(MOFO.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(MOFO.App_Start.NinjectWebCommon), "Stop")]

namespace MOFO.App_Start
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using MOFO.Attributes;
    using MOFO.Controllers;
    using MOFO.Controllers.Contracts;
    using MOFO.Database;
    using MOFO.Database.Contracts;
    using MOFO.Database.Repositories;
    using MOFO.DataProcessing;
    using MOFO.DataProcessing.Contracts;
    using MOFO.Services;
    using MOFO.Services.Contracts;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using Ninject.Web.Mvc.FilterBindingSyntax;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel
               .Bind<IDatabase>()
               .To<Database>()
           .InRequestScope();
            kernel
                .Bind<IUserRepository>()
                .To<UserRepository>()
            .InRequestScope();
            kernel
                .Bind<IFileRepository>()
                .To<FileRepository>()
            .InRequestScope();
            kernel
                .Bind<ICardRepository>()
                .To<CardRepository>()
            .InRequestScope();
            kernel
                .Bind<IRoomRepository>()
                .To<RoomRepository>()
            .InRequestScope();
            kernel
               .Bind<ISessionRepository>()
               .To<SessionRepository>()
           .InRequestScope();
            kernel
              .Bind<ISessionHistoryRepository>()
              .To<SessionHistoryRepository>()
          .InRequestScope();
            kernel
             .Bind<ITeacherRepository>()
             .To<TeacherRepository>()
         .InRequestScope();
            kernel
             .Bind<IModeratorRepository>()
             .To<ModeratorRepository>()
         .InRequestScope();
            kernel
             .Bind<ISchoolRepository>()
             .To<SchoolRepository>()
         .InRequestScope();
            kernel
             .Bind<IStudentRepository>()
             .To<StudentRepository>()
         .InRequestScope();
            kernel
           .Bind<IMessageRepository>()
           .To<MessageRepository>()
         .InRequestScope();
            kernel
         .Bind<ICityRepository>()
         .To<CityRepository>()
       .InRequestScope();
            kernel
                .Bind<IUserService>()
                .To<UserService>()
           .InRequestScope();
            kernel
                .Bind<IRoomService>()
                .To<RoomService>()
           .InRequestScope();
            kernel
                .Bind<IMessageService>()
                .To<MessageService>()
           .InRequestScope();
            kernel
                .Bind<ISessionService>()
                .To<SessionService>()
           .InRequestScope();
            kernel
                 .Bind<ISchoolService>()
                 .To<SchoolService>()
            .InRequestScope();
            kernel
                .Bind<ICardService>()
                .To<CardService>()
           .InRequestScope();
            kernel
                .Bind<IModeratorService>()
                .To<ModeratorService>()
           .InRequestScope();
            kernel
               .Bind<ITeacherService>()
               .To<TeacherService>()
          .InRequestScope();
            kernel
               .Bind<IStudentService>()
               .To<StudentService>()
          .InRequestScope();
            kernel
              .Bind<IPdfBuilder>()
              .To<PdfBuilder>()
         .InRequestScope();
            kernel
             .Bind<IEmailController>()
             .To<EmailController>()
        .InRequestScope();
            kernel
         .BindFilter<VerificationRequiredAttribute>(FilterScope.Controller, 0);
        }        
    }
}