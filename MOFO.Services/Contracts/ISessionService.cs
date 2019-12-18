using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Services.Contracts
{
    public interface ISessionService
    {
        bool HasActiveSessionByRoom(Room room);
        Session GetSessionByRoom(Room room);
        void AddMessage(int type, string fileName, string downloadCode, string message, User user, DateTime dateTimeUploaded);
        void AddSession(Session session);
        void RemoveSession(Session session);
    }
}
