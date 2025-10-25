using QWellApp.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Models
{
    public class ProductMedicalRecord
    {
        public int Id { get; set; }
        public DateTime AdmitDate { get; set; }
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int Units { get; set; }
        public float SoldPrice { get; set; }
        [ForeignKey("RecordTypeId")] //Medical = 1, Lab = 2, Procedure = 3
        public int RecordTypeId { get; set; }
        public virtual RecordType RecordType { get; set; }
        //[ForeignKey("RecordId")]
        //public int RecordId { get; set; }
        [ForeignKey("MedicalRecordId")]
        public int? MedicalRecordId { get; set; }
        public virtual MedicalRecord? MedicalRecord { get; set; }
        [ForeignKey("ProcedureRecordId")]
        public int? ProcedureRecordId { get; set; }
        public virtual ProcedureRecord? ProcedureRecord { get; set; }
        [ForeignKey("LabRecordId")]
        public int? LabRecordId { get; set; }
        public virtual LabRecord? LabRecord { get; set; }
        [ForeignKey("ChannelRecordId")]
        public int? ChannelRecordId { get; set; }
        public virtual ChannelRecord? ChannelRecord { get; set; }
    }
}
