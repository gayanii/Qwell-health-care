using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Repositories
{
    public interface ICommissionRepository
    {
        //bool Add(List<Commission> commissions);
        Task<IEnumerable<Commission>> GetMedicalCommissions(DateTime startDateTime, DateTime endDateTime);
        Task<IEnumerable<Commission>> GetLabCommissions(DateTime startDateTime, DateTime endDateTime);
        Task<IEnumerable<Commission>> GetProcedureCommissions(DateTime startDateTime, DateTime endDateTime);
        Task<IEnumerable<Commission>> GetChannelCommissions(DateTime startDateTime, DateTime endDateTime);
    }
}
