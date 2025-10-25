using QWellApp.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Models
{
    public class ActivityLog
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public EntitiesEnum AffectedEntity { get; set; }
        public int AffectedEntityId { get; set; }
        public ActionTypeEnum ActionType { get; set; }
        public string? OldValues{ get; set; }
        public string? NewValues { get; set; }
    }

    public class ActivityLogView
    {
        public string Date { get; set; }
        public string UserName { get; set; }
        public virtual User User { get; set; } // kept this cz GetActivityLogs method
        public EntitiesEnum AffectedEntity { get; set; }
        public int AffectedEntityId { get; set; }
        public ActionTypeEnum ActionType { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
    }
}
