using QWellApp.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Models
{
    public class ChannelRecord : IAdmitDateRecord
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("PatientId")]
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }

        [ForeignKey("DoctorId")]
        public int DoctorId { get; set; }
        public virtual User Doctor { get; set; }
        public string ChitNumber { get; set; }
        public float? OPDCharge { get; set; }
        public float? OtherCharges { get; set; }
        public float PharmacyBill { get; set; }
        public DateTime AdmitDate { get; set; }
        public float TotalBill { get; set; }
        public int AddedBy { get; set; }
        [ForeignKey("Nurse1Id")]
        public int? Nurse1Id { get; set; }
        public virtual User? Nurse1 { get; set; }
        [ForeignKey("Nurse2Id")]
        public int? Nurse2Id { get; set; }
        public virtual User? Nurse2 { get; set; }
        public float DocComm { get; set; }
        public float Nurse1Comm { get; set; }
        public float Nurse2Comm { get; set; }
        public float? ConsultantFee { get; set; }

        public ChannelRecord()
        {
            // Set the default value to today date
            AdmitDate = DateTime.Now;
        }
    }
     public class ChannelRecordView
    {
        public int Id { get; set; }
        public string ChitNumber { get; set; }
        public float TotalBill { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string AdmitDate { get; set; }
        public string AddedBy { get; set; }
    }
}
