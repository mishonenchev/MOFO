using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Services.Contracts
{
    public interface IStudentService
    {
        void AddStudent(Student student);
        void RemoveStudent(Student student);
        Student GetStudentByAuth(string auth);
        List<Student> GetAll();
    }
}
