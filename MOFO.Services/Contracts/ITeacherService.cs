using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Services.Contracts
{
    public interface ITeacherService
    {
        void AddTeacher(Teacher teacher);
        void RemoveTeacher(Teacher teacher);
        Teacher GetTeacherByAuth(string auth);
        List<Teacher> GetAll();
        bool IsVerifiedByUserId(string userId);
    }
}
