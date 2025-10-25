using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Models
{
    public class MedicalSummary
    {
        public int Id { get; set; }
        public string ChitNumber { get; set; }
        public string AdmitDate { get; set; }
        public float OPDCharge { get; set; }
        public float PharmacyBill { get; set; }
        public float ConsultantFee { get; set; }
        public float OtherCharges { get; set; }
        public float TotalCommisions { get; set; }
        public float TotalBill { get; set; }
    }

    public class LabSummary
    {
        public int Id { get; set; }
        public string ChitNumber { get; set; }
        public string AdmitDate { get; set; }
        public float LabBill { get; set; }
        public float LabPaidCost { get; set; }
        public float ConsultantFee { get; set; }
        public float ConsumableBill { get; set; }
        public float TotalCommisions { get; set; }
        public float TotalBill { get; set; }
    }

    public class ProcedureSummary
    {
        public int Id { get; set; }
        public string ChitNumber { get; set; }
        public string AdmitDate { get; set; }
        public float OPDCharge { get; set; }
        public float ProcedureBill { get; set; }
        public float ConsultantFee { get; set; }
        public float OtherCharges { get; set; }
        public float TotalCommisions { get; set; }
        public float TotalBill { get; set; }
    }

    public class ChannelSummary
    {
        public int Id { get; set; }
        public string ChitNumber { get; set; }
        public string AdmitDate { get; set; }
        public float OPDCharge { get; set; }
        public float PharmacyBill { get; set; }
        public float ConsultantFee { get; set; }
        public float OtherCharges { get; set; }
        public float TotalCommisions { get; set; }
        public float TotalBill { get; set; }
    }
}
