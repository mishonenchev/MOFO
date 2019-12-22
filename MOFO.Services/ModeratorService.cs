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
     
        public bool IsVerifiedByUserId(string userId)
        {
            return _moderatorRepository.Where(x => x.AspUserId == userId).First().IsVerified;
        }
        public List<Moderator> GetAll()
        {
            return _moderatorRepository.GetAll().ToList();
        }
    }
}
