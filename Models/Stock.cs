using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string BrandName { get; set; }
        public string Generic { get; set; }
        public int CollectedStock { get; set; }
        public int SoldStock { get; set; }
        public int Balance { get; set; }
    }
}
