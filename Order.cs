using System;
using System.Collections.Generic;
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
        
        [MaxLength(50)]
        public string Status { get; set; } = "Новый";
        
        [ForeignKey("Buyer")]
        public int BuyerId { get; set; }
        public virtual User Buyer { get; set; }
        
        [ForeignKey("Seller")]
        public int SellerId { get; set; }
        public virtual User Seller { get; set; }
        
        // Navigation property
        public virtual ICollection<OrderItem> Items { get; set; }
        
        public Order()
        {
            Items = new HashSet<OrderItem>();
        }
    }
}
