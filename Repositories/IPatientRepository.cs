using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Repositories
{
    public interface IPatientRepository
    {
        bool Add(Patient patientModel);
        bool Edit(Patient patientModel);
        bool Remove(int id);
        Patient GetByID(int id);
        IEnumerable<Patient> GetAll(string searchWord);
    }
}
