using System.ComponentModel.DataAnnotations;

namespace AppAuthorization
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        
        [Required]
        public string RoleName { get; set; }
        
        public string Description { get; set; }
    }
}
