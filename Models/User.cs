using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Models
{
    public class UserDetails
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string MobileNum { get; set; }
        public string? TelephoneNum { get; set; }
        public string Gender { get; set; }
        public string? NIC { get; set; }
        public string EmployeeType { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string Status { get; set; }

        [ForeignKey("RoleId")]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }

    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNum { get; set; }
        public string? TelephoneNum { get; set; }
        public string Gender { get; set; }
        public string? NIC { get; set; }
        public string EmployeeType { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; } // For securely storing passwords
        public string PasswordSalt { get; set; } // For password hashing
        public string Status { get; set; }

        [ForeignKey("RoleId")]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }

    public class UserAccount
    {
        public string Username { get; set; }
    }

    public class ChangePassword
    {
        public string Username { get; set; }
        public string NewPassword { get; set; }
    }

    public class UserView
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string MobileNum { get; set; }
        public string EmployeeType { get; set; }
        public string Status { get; set; }
    }
}

