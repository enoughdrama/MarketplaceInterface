using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppAuthorization
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Salt { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string Address { get; set; }
        
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        
        public bool IsActive { get; set; } = true;
        
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        
        public virtual Role Role { get; set; }
    }
}
