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
    public class RoomService: IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }
        public Room GetRoomByDeskCode(string deskCode)
        {
            return _roomRepository.WhereIncludeAll(x => x.Cards.Any(y => y.Code == deskCode) == true).FirstOrDefault();
        }
        public List<Room> GetRoomsBySchool(int schoolId)
        {
            return _roomRepository.WhereIncludeAll(x => x.School.Id == schoolId).ToList();
        }
        public Room GetRoomById(int roomId)
        {
            return _roomRepository.WhereIncludeAll(x => x.Id == roomId).FirstOrDefault();
        }
        public void AddRoom(Room room)
        {
            _roomRepository.Add(room);
            _roomRepository.SaveChanges();
        }
        public void SaveChanges()
        {
            _roomRepository.SaveChanges();
        }
    }
}
