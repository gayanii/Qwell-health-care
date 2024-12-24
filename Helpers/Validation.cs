using QWellApp.DBConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Helpers
{
    public class Validation
    {
        public bool IsChitNumberUnique(AppDataContext context, string chitNumber, DateTime admitDate)
        {
            return !context.MedicalRecords.Any(mr => mr.ChitNumber == chitNumber && mr.AdmitDate.Date == admitDate) &&
                   !context.ProcedureRecords.Any(pr => pr.ChitNumber == chitNumber && pr.AdmitDate.Date == admitDate) &&
                   !context.LabRecords.Any(lr => lr.ChitNumber == chitNumber && lr.AdmitDate.Date == admitDate);
        }
    }
}
