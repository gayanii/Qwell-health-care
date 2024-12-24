using QWellApp.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNum { get; set; }
        public string? TelephoneNum { get; set; }
        public string Gender { get; set; }
        public string NIC { get; set; }
        public string Age { get; set; }
        public string? AllergicHistory { get; set; }
        public string? Weight { get; set; }
        public string Status { get; set; }
    }

    public class PatientView
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNum { get; set; }
        public string NIC { get; set; }
        public string Age { get; set; }
        public string Status { get; set; }
    }
}
