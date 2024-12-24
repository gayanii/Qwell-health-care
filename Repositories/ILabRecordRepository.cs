using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Repositories
{
    public interface ILabRecordRepository
    {
        bool Add(LabRecord labRecordModel, List<int> labRecordTestData, Dictionary<int, int> labData);
        bool Edit(LabRecord labRecordModel, List<int> labRecordTestData, Dictionary<int, int> labData);
        bool Remove(int id);
        LabRecord GetByID(int id);
        IEnumerable<LabRecordView> GetAll(string searchWord);
    }
}
