using iText.Kernel.Pdf.Canvas.Parser.ClipperLib;
using QWellApp.DBConnection;
using QWellApp.Enums;
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

        public DateTime CalculateStartDateTime(DateTime startDate, int startTime)
        {
            return startTime switch
            {
                (int)TimeframeListEnum.FirstPhase => startDate.AddHours(7), //7am
                (int)TimeframeListEnum.SecondPhase => startDate.AddHours(17).AddMinutes(1), //5.01pm
                (int)TimeframeListEnum.ThirdPhase => startDate.AddHours(22).AddMinutes(1), //10.01pm
                _ => startDate
            };
        }

        public DateTime CalculateEndDateTime(DateTime endDate, int endTime)
        {
            return endTime switch
            {
                (int)TimeframeListEnum.FirstPhase => endDate.AddHours(17), //5pm
                (int)TimeframeListEnum.SecondPhase => endDate.AddHours(22), //10pm
                (int)TimeframeListEnum.ThirdPhase => endDate.AddDays(1).AddHours(6).AddMinutes(59), //6.59am next day
                _ => endDate
            };
        }

        public string GenerateRandomPassword(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@$?";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
