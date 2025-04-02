using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppAuthorization
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        
        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        
        [Required]
        public decimal TotalAmount { get; set; }
        
        public string Status { get; set; }
        
        [ForeignKey("Buyer")]
        public int BuyerId { get; set; }
        public virtual User Buyer { get; set; }
        
        [ForeignKey("Seller")]
        public int SellerId { get; set; }
        public virtual User Seller { get; set; }
    }
}
