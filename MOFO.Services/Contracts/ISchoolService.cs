using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Services.Contracts
{
    public interface ISchoolService
    {
        void AddSchool(School school);
        void RemoveSchool(School school);
        List<School> GetSchoolsInCity(string cityName, string schoolName);
        List<School> GetAll();
        List<City> SearchCity(string cityName, int status);
        City GetCityById(int id);
        void VerifyCityById(int id);
        List<School> GetSchoolsByCityId(int cityId);
        School GetSchoolById(int id);
        List<City> GetAllCities();
        void AddCity(City city);
        void RemoveCity(City city);
        List<School> SearchSchool(string schoolName, int cityId, int status);
    }
}
