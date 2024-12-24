using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string Generic { get; set; }
        public int CurrentQuantity { get; set; }
        public float SellingPrice { get; set; }
        public string Status { get; set; }
    }

    public class ProductView
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string Generic { get; set; }
        public int CurrentQuantity { get; set; }
        public float SellingPrice { get; set; }
        public string Status { get; set; }
    }
}
