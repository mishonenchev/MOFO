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
        void AddRoom(Room room);
        Room GetRoomByDeskCode(string deskCode);
        Room GetRoomById(int roomId);
        List<Room> GetRoomsBySchool(int schoolId);
        void SaveChanges();
    }
}
