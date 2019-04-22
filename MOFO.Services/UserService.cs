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
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public void AddUser(User user)
        {
            _userRepository.Add(user);
            _userRepository.SaveChanges();
        }
        public void RemoveUser(User user)
        {
            _userRepository.Remove(user);
            _userRepository.SaveChanges();
        }
        public User GetUserByAuth(string auth)
        {
            return _userRepository.WhereIncludeAll(x => x.Auth == auth).FirstOrDefault();
        }
        public List<User> GetAll()
        {
            return _userRepository.WhereIncludeAll(x => x.Id > 0).ToList();
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
            if (_userRepository.Where(x => x.Auth == result).Count() == 0)
            {
                return result;
            }
            else return NewAuthString();
        }
        public void Update()
        {
            _userRepository.SaveChanges();
        }
    }
}
