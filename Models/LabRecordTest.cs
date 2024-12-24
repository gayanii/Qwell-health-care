using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Models
{
    public class LabRecordTest
    {
        public int Id { get; set; }
        [ForeignKey("LabTestId")]
        public int LabTestId{ get; set; }
        public virtual LabTest LabTest { get; set; }

        [ForeignKey("LabRecordId")]
        public int LabRecordId { get; set; }
        public virtual LabRecord LabRecord { get; set; }
    }
}
