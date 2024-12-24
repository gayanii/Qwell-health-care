using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QWellApp.Repositories;

namespace QWellApp.Models
{
    public class LabRecord : IAdmitDateRecord
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("PatientId")]
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        [ForeignKey("DoctorId")]
        public int? DoctorId { get; set; }
        public virtual User? Doctor { get; set; }
        [ForeignKey("Nurse1Id")]
        public int? Nurse1Id { get; set; }
        public virtual User? Nurse1 { get; set; }
        [ForeignKey("Nurse2Id")]
        public int? Nurse2Id { get; set; }
        public virtual User? Nurse2 { get; set; }
        public string ChitNumber { get; set; }
        public DateTime AdmitDate { get; set; }
        public float LabBill { get; set; }
        public float? ConsultantFee { get; set; }
        public float? OtherCharges { get; set; }
        public float TotalBill { get; set; }
        public float TotalLabPaidCost { get; set; }
        public int AddedBy { get; set; }
        public float DocComm { get; set; }
        public float Nurse1Comm { get; set; }
        public float Nurse2Comm { get; set; }

        public LabRecord()
        {
            // Set the default value to today date
            AdmitDate = DateTime.Now;
        }
    }

    public class LabRecordView()
    {
        public int Id { get; set; }
        public string ChitNumber { get; set; }
        public float TotalBill { get; set; }
        public string PatientName { get; set; }
        public string AdmitDate { get; set; }
        public string AddedBy { get; set; }
    }
}
