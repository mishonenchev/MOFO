using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Services.Contracts
{
    public interface IMessageService
    {
        void Remove(Message message);
        string NewDownloadCode();
        File GetFileByDownloadCode(string downloadCode);
        IEnumerable<Message> GetMessagesByUserSession(Session session);
    }
}
