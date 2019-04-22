using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Services.Contracts
{
    public interface IRoomService
    {
        Room GetRoomByDeskCode(string deskCode);
        bool HasActiveSessionByRoom(int roomId);
        void AddSession(Session session);
    }
}
