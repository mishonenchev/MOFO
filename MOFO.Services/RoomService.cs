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
            return _roomRepository.WhereIncludeAll(x => x.Desks.Any(y => y.Code == deskCode) == true).FirstOrDefault();
        }
    }
}
