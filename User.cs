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
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(256)]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(50)]
        public string Salt { get; set; }
        
        [MaxLength(100)]
        public string FirstName { get; set; }
        
        [MaxLength(100)]
        public string LastName { get; set; }
        
        [MaxLength(150)]
        public string Email { get; set; }
        
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        
        [MaxLength(255)]
        public string Address { get; set; }
        
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        
        public bool IsActive { get; set; } = true;
        
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        
        public virtual Role Role { get; set; }
    }
}
