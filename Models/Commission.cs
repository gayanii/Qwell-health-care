using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Models
{
    public class Commission
    {
        public List<string> Date { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public List<string> ChitNumber { get; set; }
        public float TotalCommisssion { get; set; }
    }
}
