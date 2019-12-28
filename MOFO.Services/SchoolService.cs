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
        public List<School> GetSchoolsInCity(string cityName, string schoolName)
        {
            if (string.IsNullOrEmpty(schoolName))
            {
                return _schoolRepository.Where(x => x.City.Name == cityName).ToList();
            }
            return _schoolRepository.Where(x => x.City.Name==cityName && x.Name.Contains(schoolName)).ToList();
        }
        public List<City> SearchCity(string cityName)
        {
            return _cityRepository.Where(x => x.Name.Contains(cityName)).ToList();
        }
        public List<School> SearchSchool(string schoolName, int cityId, int status)
        {
            List<School> results = new List<School>();
            if (!string.IsNullOrEmpty(schoolName))
            {
                results = _schoolRepository.SearchSchool(schoolName).ToList();
                
            }
            else
            {
                results = _schoolRepository.Where(x => x.Id > 0, x => x.City).ToList();
            }
            if (cityId != 0)
            {
                results = results.Where(x => x.City.Id == cityId).ToList();
            }
            if (status != 0)
            {
                var booleanStatus = (status - 1) == 0 ? false : true;
                results = results.Where(x => x.IsVerified == booleanStatus).ToList();
            }
            return results;
        }
        public School GetSchoolById(int id)
        {
            return _schoolRepository.WhereIncludeAll(x => x.Id == id).FirstOrDefault();
        }
        public City GetCityById(int id)
        {
            return _cityRepository.Where(x => x.Id == id).FirstOrDefault();
        }
        public List<City> GetAllCities()
        {
            return _cityRepository.GetAll().ToList();
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
