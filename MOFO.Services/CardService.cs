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
    public class CardService:ICardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly IRoomRepository _roomRepository;
        public CardService(ICardRepository cardRepository, IRoomRepository roomRepository)
        {
            _cardRepository = cardRepository;
            _roomRepository = roomRepository;
        }
        public List<Card> GetCardsByRoomId(int roomId)
        {
            var room = _roomRepository.WhereIncludeAll(x => x.Id == roomId).FirstOrDefault();
            if (room != null)
            {
                return room.Cards.ToList();
            }
            return null;
        }
    }
}
