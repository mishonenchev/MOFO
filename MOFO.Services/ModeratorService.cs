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
    public class ModeratorService:IModeratorService
    {
        private readonly IModeratorRepository _moderatorRepository;
        public ModeratorService(IModeratorRepository moderatorRepository)
        {
            _moderatorRepository = moderatorRepository;
        }
        public void AddModerator(Moderator moderator)
        {
            _moderatorRepository.Add(moderator);
            _moderatorRepository.SaveChanges();
        }
        public void RemoveModerator(Moderator moderator)
        {
            _moderatorRepository.Remove(moderator);
            _moderatorRepository.SaveChanges();
        }
        public string NewAuthString()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var result = "";
            Random rn = new Random();
            for (int i = 0; i < 64; i++)
            {
                result += chars[rn.Next(0, chars.Length - 1)];
            }
            if (_moderatorRepository.Where(x => x.Auth == result).Count() == 0)
            {
                return result;
            }
            else return NewAuthString();
        }
        public Moderator GetModeratorByAuth(Moderator moderator)
        {
            return _moderatorRepository.Where(x => x.Auth == moderator.Auth).FirstOrDefault();
        }
        public List<Moderator> GetAll()
        {
            return _moderatorRepository.GetAll().ToList();
        }
    }
}
