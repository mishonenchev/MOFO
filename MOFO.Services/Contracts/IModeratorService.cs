using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Services.Contracts
{
    public interface IModeratorService
    {
        void AddModerator(Moderator Moderator);
        void RemoveModerator(Moderator Moderator);
        string NewAuthString();
        Moderator GetModeratorByAuth(Moderator moderator);
        List<Moderator> GetAll();
    }
}
