using QWellApp.Enums;
using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Repositories
{
    public interface IProductMedicalRecordRepository
    {
        bool Add(ProductMedicalRecord medicalDoseModel);
        bool Edit(ProductMedicalRecord medicalDoseModel);
        bool RemoveMedicalRecord(int id);
        bool RemoveProcedureRecord(int id);
        bool RemoveLabRecord(int id);
        bool RemoveChannelRecord(int id);
        //IEnumerable<ProductMedicalRecord> GetByID(int id);
        IEnumerable<ProductMedicalRecord> GetAll(int recordId, RecordTypeEnum recordTypeId);
    }
}
