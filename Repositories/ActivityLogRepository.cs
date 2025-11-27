using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using QWellApp.DBConnection;
using QWellApp.Enums;
using QWellApp.Models;
using QWellApp.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace QWellApp.Repositories
{
    public class ActivityLogRepository : BaseRepository, IActivityLogRepository
    {
        //public IUserRepository userRepository;

        public ActivityLogRepository()
        {
            //userRepository = new UserRepository();
        }

        public void AddLog(ActivityLog log, UserDetails currentUser)
        {
            try
            {
                using (var context = new AppDataContext())
                {
                    //UserDetails currentUser = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
                    log.UserId = currentUser.Id;
                    log.Date = DateTime.Now;
                    context.ActivityLogs.Add(log);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                throw;
            }
        }

        public void CleanupOldActivityLogs()
        {
            try
            {
                using (var context = new AppDataContext())
                {
                    // Calculate the cutoff date (3 months ago)
                    DateTime cutoffDate = DateTime.Now.AddMonths(-3);

                    // Direct SQL delete is faster
                    context.Database.ExecuteSqlRaw(
                        "DELETE FROM ActivityLogs WHERE Date < {0}", cutoffDate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cleaning old activity logs: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<ActivityLogView>> GetActivityLogs(string searchWord)
        {
            try
            {
                using (var context = new AppDataContext())
                {
                    string normalizedSearchWord = searchWord.Trim().ToLower();
                    DateTime? searchDate = DateTime.TryParse(searchWord, out var parsedDate) ? parsedDate : null;

                    var activityLogs = await context.ActivityLogs
                        .Include(a => a.User)
                        .Where(a =>
                            (searchDate.HasValue && a.Date.Date == searchDate.Value.Date) ||  // Compare dates directly
                            EF.Functions.Like(a.User.FirstName, $"%{searchWord}%") || EF.Functions.Like(a.User.LastName, $"%{searchWord}%") || 
                            EF.Functions.Like(a.OldValues, $"%{searchWord}%") || 
                            EF.Functions.Like(a.NewValues, $"%{searchWord}%"))
                        .OrderByDescending(a => a.Date)
                        .ToListAsync(); // Fetch data first

                    // Convert Enums to Strings after fetching
                    return activityLogs.Select(a => new ActivityLogView
                    {
                        Date = a.Date.ToString("dd-MMM-yyyy HH:mm"),
                        UserName = $"{a.User.FirstName} {a.User.LastName}",
                        AffectedEntity = a.AffectedEntity,
                        AffectedEntityId = a.AffectedEntityId,
                        ActionType = a.ActionType,
                        OldValues = a.OldValues,
                        NewValues = a.NewValues
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                throw;
            }
        }

    }
}
