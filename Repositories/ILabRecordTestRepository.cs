using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Repositories
{
    public interface ILabRecordTestRepository
    {
        bool Add(LabRecordTest labRecordTestModel);
        bool Edit(LabRecordTest labRecordTestModel);
        bool Remove(int id);
        //IEnumerable<LabRecordTest> GetByID(int id);
        IEnumerable<LabRecordTest> GetAll(int labRecordId);
    }
}
