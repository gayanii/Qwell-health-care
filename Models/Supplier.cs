using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string TelephoneNum { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
    }

    public class SupplierView
    {
        public int Id { get; set; }
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string TelephoneNum { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
    }
}
