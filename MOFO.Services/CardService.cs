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
        private readonly IRoomService _roomService;
        private readonly ISchoolService _schoolService;
        public CardService(ICardRepository cardRepository, IRoomRepository roomRepository, IRoomService roomService, ISchoolService schoolService)
        {
            _cardRepository = cardRepository;
            _roomRepository = roomRepository;
            _roomService = roomService;
            _schoolService = schoolService;
        }
        public List<Card> GetCardsByRoomId(int roomId)
        {
            var cards = _cardRepository.WhereIncludeAll(x=>x.Room.Id == roomId).ToList();
            return cards;
        }
        public int GetReferenceNumber()
        {
            if (_cardRepository.GetAll().Count() == 0) return 1000;
            else
            {
                return _cardRepository.GetAll().Max(x => x.ReferenceNumber) + 1;
            }
        }
        public Card GetCardByRefNumber(int refNumber)
        {
            return _cardRepository.WhereIncludeAll(x => x.ReferenceNumber == refNumber).FirstOrDefault();
        }
        public Card GetCardByCode(string code)
        {
            return _cardRepository.WhereIncludeAll(x => x.Code == code).FirstOrDefault();
        }
        public Card GetCardByQRCode(string qrCode)
        {
            return _cardRepository.WhereIncludeAll(x => x.QRCode == qrCode).FirstOrDefault();
        }
        public string GetNewQRCode()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var result = "";
            Random rn = new Random();
            for (int i = 0; i < 20; i++)
            {
                result += chars[rn.Next(0, chars.Length - 1)];
            }
            if (_cardRepository.Where(x => x.QRCode == result).Count() == 0)
            {
                return result;
            }
            else return GetNewQRCode();
        }
        public List<Card> GetAll()
        {
            return _cardRepository.WhereIncludeAll(x => x.Id > 0).ToList();
        }
        public List<Card> SearchCards(string refNumber = null, int cityId = 0, int schoolId = 0, int roomId = 0)
        {
            var results = new List<Card>();
            if (roomId != 0)
            {
                    var cards = this.GetCardsByRoomId(roomId);
                    foreach (var item in cards)
                    {
                        results.Add(item);
                    }
            }else if (schoolId != 0)
            {
                var rooms = _roomService.GetRoomsBySchool(schoolId);
                foreach (var room in rooms)
                {
                    var cards = this.GetCardsByRoomId(room.Id);
                    foreach (var item in cards)
                    {
                        results.Add(item);
                    }
                }
            }else if(cityId != 0)
            {
                var schools = _schoolService.GetSchoolsByCityId(cityId);

                foreach (var school in schools)
                {
                    var rooms = _roomService.GetRoomsBySchool(school.Id);
                    foreach (var room in rooms)
                    {
                        var cards = this.GetCardsByRoomId(room.Id);
                        foreach (var item in cards)
                        {
                            results.Add(item);
                        }
                    }
                }
            }
            else
            {
                results = GetAll();
            }

            if (!string.IsNullOrWhiteSpace(refNumber))
            {
                refNumber = refNumber.Trim();
                if (refNumber.ToLower().Contains("ref"))
                {
                    refNumber = refNumber.Replace("ref", "");
                }
                if (int.TryParse(refNumber, out int intRef))
                {
                    results = results.Where(x => x.ReferenceNumber == intRef).ToList();
                }

            }
            return results;
        }
        public void AddCard (Card card)
        {
            _cardRepository.Add(card);
            _cardRepository.SaveChanges();
        }
    }
}
