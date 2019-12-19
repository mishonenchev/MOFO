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
    public class SchoolService : ISchoolService
    {
        private readonly ISchoolRepository _schoolRepository;
        public SchoolService(ISchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }
        public void AddSchool(School school)
        {
            _schoolRepository.Add(school);
            _schoolRepository.SaveChanges();
        }
        public void RemoveSchool(School school)
        {
            _schoolRepository.Remove(school);
            _schoolRepository.SaveChanges();
        }
        public School GetSchoolByCity(School school)
        {
            return _schoolRepository.Where(x => x.City.Id == school.City.Id).FirstOrDefault();
        }
        public List<School> GetAll()
        {
            return _schoolRepository.GetAll().ToList();
        }
    }
}
