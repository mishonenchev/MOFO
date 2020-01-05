using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Services.Contracts
{
    public interface ICardService
    {
        void AddCard(Card card);
        List<Card> GetAll();
        Card GetCardByCode(string code);
        Card GetCardByQRCode(string qrCode);
        Card GetCardByRefNumber(int refNumber);
        List<Card> GetCardsByRoomId(int roomId);
        string GetNewQRCode();
        int GetReferenceNumber();
        List<Card> SearchCards(string refNumber = null, int cityId = 0, int schoolId = 0, int roomId = 0);
    }
}
