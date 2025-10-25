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
        Task<IEnumerable<Commission>> GetMedicalCommissions(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Commission>> GetLabCommissions(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Commission>> GetProcedureCommissions(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Commission>> GetChannelCommissions(DateTime startDate, DateTime endDate);
    }
}
