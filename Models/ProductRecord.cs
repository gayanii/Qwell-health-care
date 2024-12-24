using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Models
{
    public class ProductRecord
    {
        [Key]
        public int Id { get; set; }
        public string? Barcode { get; set; }

        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public float SupplierPrice { get; set; }
        public float SellingPrice { get; set; }
        public int OrderedQuantity { get; set; }
        public DateTime? ExpDate { get; set; }
        public DateTime ReceivedDate { get; set; }

        [ForeignKey("SupplierId")]
        public int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }

        [ForeignKey("AddedBy")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }

    public class ProductRecordView
    {
        public int Id { get; set; }
        public string? Barcode { get; set; }
        public string BrandName { get; set; }
        public string Generic { get; set; }
        public string ReceivedDate { get; set; }
        public int OrderedQty { get; set; }
        public string SupplierName { get; set; }
        public string AddedBy { get; set; }
    }
}
