using MOFO.Database;
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
        private readonly ICityRepository _cityRepository;
        public SchoolService(ISchoolRepository schoolRepository, ICityRepository cityRepository)
        {
            _schoolRepository = schoolRepository;
            _cityRepository = cityRepository;
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
        public List<City> SearchCity(string cityName)
        {
            return _cityRepository.Where(x => x.Name.Contains(cityName)).ToList();
        }
        public City GetCityById(int id)
        {
            return _cityRepository.Where(x => x.Id == id).FirstOrDefault();
        }
        public void AddCity(City city)
        {
            _cityRepository.Add(city);
            _cityRepository.SaveChanges();
        }
        public List<School> GetAll()
        {
            return _schoolRepository.GetAll().ToList();
        }
    }
}
