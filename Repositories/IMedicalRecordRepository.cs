using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Repositories
{
    public interface IMedicalRecordRepository
    {
        bool Add(MedicalRecord medicalRecordModel, Dictionary<int, int> medicalDoseData);
        bool Edit(MedicalRecord medicalRecordModel, Dictionary<int, int> medicalDoseData);
        bool Remove(int id);
        MedicalRecord GetByID(int id);
        IEnumerable<MedicalRecordView> GetAll(string searchWord);
    }
}
