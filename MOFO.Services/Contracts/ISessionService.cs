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
        void AddMessage(int type, string fileName, string downloadCode, string message, User user, DateTime dateTimeUploaded, string fileSize);
        void AddSession(Session session);
        void AddSessionHistory(SessionHistory sessionHistory);
        void AddUserToCurrentSessionHistory(User user, int roomId);
        void RemoveAllUsersFromSession(Session session);
        Session GetSessionById(int id);
        SessionHistory GetCurrentSessionHistoryByRoom(int roomId);
        void RemoveSession(int sessionId);
        SessionHistory GetSessionHistoryById(int id);
        void RemoveSessionHistory(SessionHistory sessionHistory);
    }
}
