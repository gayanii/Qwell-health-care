using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Repositories
{
    public interface IActivityLogRepository
    {
        void AddLog(ActivityLog log, UserDetails currentUser);
        void CleanupOldActivityLogs();
        Task<IEnumerable<ActivityLogView>> GetActivityLogs(string searchWord);
    }
}
