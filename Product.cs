using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppAuthorization
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        public int Quantity { get; set; }
        
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        
        [ForeignKey("Seller")]
        public int SellerId { get; set; }
        public virtual User Seller { get; set; }
    }
}
