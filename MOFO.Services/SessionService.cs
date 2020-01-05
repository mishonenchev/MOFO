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
        private readonly IMessageRepository _messageRepository;
        public SessionService(ISessionRepository sessionRepository, IMessageService fileService, IUserRepository userRepository, IFileRepository fileRepository, IMessageRepository messageRepository)
        {
            _sessionRepository = sessionRepository;
            _messageService = fileService;
            _userRepository = userRepository;
            _fileRepository = fileRepository;
            _messageRepository = messageRepository;

        }
        public bool HasActiveSessionByRoom(Room room)
        {
            return _sessionRepository.Where(x => x.Room.Id == room.Id).FirstOrDefault() != null;
        }
        public Session GetSessionByRoom(Room room)
        {
            return _sessionRepository.Where(x => x.Room.Id == room.Id, x=>x.Room).FirstOrDefault();
        }
        public void AddMessage(int type, string fileName, string downloadCode, string message, User user, DateTime dateTimeUploaded)
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
                    FileName = fileName
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
        public void RemoveSession(Session session)
        {
            foreach (var message in session.Messages.ToList())
            {
                _messageService.Remove(message);
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
