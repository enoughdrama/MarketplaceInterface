using System;
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
        
        [Required]
        public int Quantity { get; set; }
        
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        
        [ForeignKey("Seller")]
        public int SellerId { get; set; }
        public virtual User Seller { get; set; }
    }
    
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public decimal Price { get; set; }
    }
    
    public class PaymentMethod
    {
        [Key]
        public int PaymentMethodId { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
    }
    
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        
        [ForeignKey("PaymentMethod")]
        public int PaymentMethodId { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        
        [Required]
        public decimal Amount { get; set; }
        
        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        
        [Required]
        public string Status { get; set; } = "Completed";
        
        public string TransactionId { get; set; }
    }
    
    public class Payout
    {
        [Key]
        public int PayoutId { get; set; }
        
        [ForeignKey("Seller")]
        public int SellerId { get; set; }
        public virtual User Seller { get; set; }
        
        [Required]
        public decimal Amount { get; set; }
        
        [Required]
        public DateTime PayoutDate { get; set; } = DateTime.Now;
        
        [Required]
        public string Status { get; set; } = "Pending";
        
        public string Notes { get; set; }
    }
}
