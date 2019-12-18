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
    public class MessageService: IMessageService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IMessageRepository _messageRepository;
        public MessageService(IFileRepository fileRepository, IMessageRepository messageRepository)
        {
            _fileRepository = fileRepository;
            _messageRepository = messageRepository;
        }
        public void Remove(Message message)
        {
            if (message.File != null)
            {
                _fileRepository.Remove(message.File);
            }
            _messageRepository.Remove(message);
            _fileRepository.SaveChanges();
        }
        public string NewDownloadCode()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var result = "";
            Random rn = new Random();
            for (int i = 0; i < 12; i++)
            {
                result += chars[rn.Next(0, chars.Length - 1)];
            }
            if (_fileRepository.Where(x => x.DownloadCode == result).Count() == 0)
            {
                return result;
            }
            else return NewDownloadCode();
        }
        public File GetFileByDownloadCode(string downloadCode)
        {
            return _fileRepository.Where(x => x.DownloadCode == downloadCode).FirstOrDefault();
        }
        public IEnumerable<Message> GetMessagesByUserSession(Session session)
        {
            return _messageRepository.WhereIncludeAll(x => x.User.Session.Id == session.Id).ToList();
        }
    }
}
