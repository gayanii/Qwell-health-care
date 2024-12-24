using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Repositories
{
    public interface ISummaryRepository
    {
        Task<IEnumerable<MedicalSummary>> GetMedicalSummary(DateTime startDate, DateTime endDate);
        Task<IEnumerable<ProcedureSummary>> GetProcedureSummary(DateTime startDate, DateTime endDate);
        Task<IEnumerable<LabSummary>> GetLabSummary(DateTime startDate, DateTime endDate);
        Task<Report> GenerateReport<T>(IEnumerable<T> summary, DateTime startDate, DateTime endDate) where T : class;
        Report GenerateFullReport(Report medicalReport, Report procedureReport, Report labReport);
    }
}
