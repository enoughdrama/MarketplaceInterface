using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppAuthorization
{
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
        
        public string Status { get; set; }
        
        public string TransactionId { get; set; }
    }
}
