using System.ComponentModel.DataAnnotations;

namespace AppAuthorization
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
    }
}
