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
        
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string Address { get; set; }
        
        [Required]
        public DateTime RegistrationDate { get; set; }
        
        [Required]
        public bool IsActive { get; set; }
        
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
