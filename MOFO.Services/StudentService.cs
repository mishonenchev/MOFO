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
    public class StudentService:IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public void AddStudent(Student student)
        {
            _studentRepository.Add(student);
            _studentRepository.SaveChanges();
        }
        public void RemoveStudent(Student student)
        {
            _studentRepository.Remove(student);
            _studentRepository.SaveChanges();
        }
        public Student GetStudentByAuth(string auth)
        {
            return _studentRepository.WhereIncludeAll(x => x.User.Auth == auth).FirstOrDefault();
        }
        public List<Student> GetAll()
        {
            return _studentRepository.GetAll().ToList();
        }
    }
}
