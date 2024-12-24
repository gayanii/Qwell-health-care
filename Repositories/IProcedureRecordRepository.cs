using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Repositories
{
    public interface IProcedureRecordRepository
    {
        bool Add(ProcedureRecord procedureRecordModel, Dictionary<int, int> procedureData);
        bool Edit(ProcedureRecord procedureRecordModel, Dictionary<int, int> procedureData);
        bool Remove(int id);
        ProcedureRecord GetByID(int id);
        IEnumerable<ProcedureRecordView> GetAll(string searchWord);
    }
}
