using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Models
{
    public class Report
    {
        public int Id { get; set; }
        public float? TotalIncome { get; set; } = 0.0f;
        public float? TotalLabPaid { get; set; } = 0.0f;
        public float? QwellCommission { get; set; } = 0.0f;
        public float? TotalCommissions { get; set; } = 0.0f;
        public float? Balance { get; set; }
        public IEnumerable<Commission> Commissions { get; set; }
    }
}
