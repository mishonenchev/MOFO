using MOFO.Database.Contracts;
using MOFO.Models;
using MOFO.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository; 
        private readonly IFileService _fileService;
        public SessionService(ISessionRepository sessionRepository, IFileService fileService, IUserRepository userRepository )
        {
            _sessionRepository = sessionRepository;
            _fileService = fileService;
            _userRepository = userRepository;
        }
        public bool HasActiveSessionByRoom(Room room)
        {
            return _sessionRepository.Where(x => x.Room.Id == room.Id).FirstOrDefault() != null;
        }
        public Session GetSessionByRoom(Room room)
        {
            return _sessionRepository.Where(x => x.Room.Id == room.Id, x=>x.Room).FirstOrDefault();
        }
        public void AddFile(int type, string fileName, string downloadCode, string message, User user, DateTime dateTimeUploaded)
        {
            var session = user.Session;
            session.Files.Add(new File()
            {
                Type = (Models.Type)type,
                FileName = fileName,
                DownloadCode = downloadCode,
                Message = message,
                User = user,
                DateTimeUploaded = dateTimeUploaded
            });
            _sessionRepository.SaveChanges();
        }
        public void AddSession(Session session)
        {
            _sessionRepository.Add(session);
            _sessionRepository.SaveChanges();
        }
        public void RemoveSession(Session session)
        {
            foreach (var file in session.Files.ToList())
            {
                _fileService.Remove(file);
            }
            
            _sessionRepository.SaveChanges();
            var users = _userRepository.Where(x => x.Session.Id == session.Id).ToList();
            foreach (var item in users)
            {
                item.Session = null;
            }

            _sessionRepository.SaveChanges();
            _sessionRepository.Remove(session);
            _sessionRepository.SaveChanges();
        }
    }
}
