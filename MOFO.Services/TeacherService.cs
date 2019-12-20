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
    public class TeacherService:ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        public TeacherService(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }
        public void AddTeacher(Teacher teacher)
        {
            _teacherRepository.Add(teacher);
            _teacherRepository.SaveChanges();
        }
        public void RemoveTeacher(Teacher teacher)
        {
            _teacherRepository.Remove(teacher);
            _teacherRepository.SaveChanges();
        }
        public Teacher GetTeacherByAuth(string auth)
        {
            return _teacherRepository.WhereIncludeAll(x => x.User.Auth == auth).FirstOrDefault();
        }
        public List<Teacher> GetAll()
        {
            return _teacherRepository.GetAll().ToList();
        }
    }
}
