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
        List<City> SearchCity(string cityName);
        City GetCityById(int id);
        School GetSchoolById(int id);
        List<City> GetAllCities();
        void AddCity(City city);
        List<School> SearchSchool(string schoolName, int cityId, int status);
    }
}
