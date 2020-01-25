﻿using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Services.Contracts
{
    public interface IUserService
    {
        void AddUser(User user);
        void RemoveUser(User user);
        User GetUserByAuth(string auth);
        List<User> GetAll();
        string NewAuthString();
        void Update();
        bool IsTelephoneValid(string telephone);
        User GetUserByUserId(string userId);
        void SaveChanges();
        List<User> GetUsersBySession(int sessionId);
        List<SessionHistory> GetLastSessionHistoriesByUserId(int userId, int take = 0);
    }
}
