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
        School GetSchoolByCity(School school);
        List<School> GetAll();
    }
}
