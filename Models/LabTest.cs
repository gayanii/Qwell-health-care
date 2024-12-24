using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Models
{
    public class LabTest
    {
        [Key]
        public int Id { get; set; }
        public string HospitalName { get; set; }
        public string TestName { get; set; }
        public float Cost { get; set; }
        public float? Discount { get; set; }
        public string? LabPaid { get; set; }
        public string Status { get; set; }
    }

    public class LabTestView
    {
        public int Id { get; set; }
        public string HospitalName { get; set; }
        public string TestName { get; set; }
        public float Cost { get; set; }
        public float? Discount { get; set; }
        public string? LabPaid { get; set; }
        public string Status { get; set; }
    }
}
