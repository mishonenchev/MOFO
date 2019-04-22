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
        private readonly ISessionRepository _sessionRepository;
        public RoomService(IRoomRepository roomRepository, ISessionRepository sessionRepository)
        {
            _roomRepository = roomRepository;
            _sessionRepository = sessionRepository;
        }
        public Room GetRoomByDeskCode(string deskCode)
        {
            return _roomRepository.Where(x => x.Desks.Any(y => y.Code == deskCode) == true, x=>x.Desks).FirstOrDefault();
        }
        public bool HasActiveSessionByRoom(int roomId)
        {
            return _sessionRepository.Where(x => x.Room.Id == roomId).FirstOrDefault() != null;
        }
        public void AddSession(Session session)
        {
            _sessionRepository.Add(session);
            _sessionRepository.SaveChanges();
        }
    }
}
