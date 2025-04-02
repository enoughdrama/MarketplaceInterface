using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppAuthorization
{
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
        
        public string Status { get; set; }
        
        public string TransactionId { get; set; }
    }
}
