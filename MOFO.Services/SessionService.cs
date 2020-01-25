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
    //session service history 
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository; 
        private readonly IMessageService _messageService;
        private readonly IFileRepository _fileRepository;
        private readonly ISessionHistoryRepository _sessionHistoryRepository;
        private readonly IMessageRepository _messageRepository;
        public SessionService(ISessionRepository sessionRepository, IMessageService fileService, IUserRepository userRepository, IFileRepository fileRepository, IMessageRepository messageRepository, ISessionHistoryRepository sessionHistoryRepository)
        {
            _sessionRepository = sessionRepository;
            _messageService = fileService;
            _userRepository = userRepository;
            _fileRepository = fileRepository;
            _messageRepository = messageRepository;
            _sessionHistoryRepository = sessionHistoryRepository;

        }
        public bool HasActiveSessionByRoom(Room room)
        {
            return _sessionRepository.Where(x => x.Room.Id == room.Id&&x.IsActive).FirstOrDefault() != null;
        }
        public Session GetSessionByRoom(Room room)
        {
            return _sessionRepository.Where(x => x.Room.Id == room.Id&&x.IsActive, x=>x.Room).FirstOrDefault();
        }
        public void AddMessage(int type, string fileName, string downloadCode, string message, User user, DateTime dateTimeUploaded, string fileSize)
        {
            var session = user.Session;
            var messageObj = new Message()
            {
                Text = message,
                User = user,
                DateTimeUploaded = dateTimeUploaded
            };
            if (!string.IsNullOrEmpty(fileName))
            {
                messageObj.File = new File()
                {
                    DownloadCode = downloadCode,
                    DateTimeUploaded = dateTimeUploaded,
                    FileName = fileName,
                     Size = fileSize
                };
                _fileRepository.Add(messageObj.File);
                _fileRepository.SaveChanges();
            }
            _messageRepository.Add(messageObj);
            _sessionRepository.SaveChanges();
            if (session.Messages != null)
            {
                session.Messages.Add(messageObj);
            }
            _sessionRepository.SaveChanges();
        }
        public void AddSession(Session session)
        {
            _sessionRepository.Add(session);
            _sessionRepository.SaveChanges();
        }
        public void AddSessionHistory(SessionHistory sessionHistory)
        {
            _sessionHistoryRepository.Add(sessionHistory);
            _sessionHistoryRepository.SaveChanges();
        }
        public void RemoveSessionHistory(SessionHistory sessionHistory)
        {
            _sessionHistoryRepository.Remove(sessionHistory);
            _sessionHistoryRepository.SaveChanges();
        }
        public void AddUserToCurrentSessionHistory(User user, int roomId)
        {
            var sessionHistory = _sessionHistoryRepository.WhereIncludeAll(x => x.Room.Id == roomId).Where(x => x.StartDateTime == x.FinishDateTime).OrderByDescending(x => x.StartDateTime).FirstOrDefault();

            if (sessionHistory != null && user != null)
            {
               if(!sessionHistory.Users.Any(x=>x.Id == user.Id))
                {
                    sessionHistory.Users.Add(user);
                    _sessionHistoryRepository.SaveChanges();
                }
               
            }
        }
        public void RemoveSession(int sessionId)
        {
            var session = _sessionRepository.WhereIncludeAll(x => x.Id == sessionId).FirstOrDefault();
            session.Messages = null;
            
            _sessionRepository.SaveChanges();
            RemoveAllUsersFromSession(session);

            
            _sessionRepository.Remove(session);
            _sessionRepository.SaveChanges();
        }
        public void RemoveAllUsersFromSession(Session session)
        {
            if (session != null)
            {
                var users = _userRepository.Where(x => x.Session.Id == session.Id).ToList();
                foreach (var item in users)
                {
                    item.Session = null;
                }
                _sessionRepository.SaveChanges();
            }
        }
        public Session GetSessionById(int id)
        {
            return _sessionRepository.WhereIncludeAll(x => x.Id == id).FirstOrDefault();
        }
        public SessionHistory GetSessionHistoryById(int id)
        {
            return _sessionHistoryRepository.WhereIncludeAll(x => x.Id == id).FirstOrDefault();
        }
        public SessionHistory GetCurrentSessionHistoryByRoom(int roomId)
        {
            return _sessionHistoryRepository.Where(x => x.Room.Id == roomId).Where(x => x.StartDateTime == x.FinishDateTime).FirstOrDefault();
        }
    }
}
