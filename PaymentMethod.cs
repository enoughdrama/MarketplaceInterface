using System.ComponentModel.DataAnnotations;

namespace AppAuthorization
{
    public class PaymentMethod
    {
        [Key]
        public int PaymentMethodId { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
    }
}
