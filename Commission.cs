using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppAuthorization
{
    public class Commission
    {
        [Key]
        public int CommissionId { get; set; }
        
        [ForeignKey("Payment")]
        public int PaymentId { get; set; }
        public virtual Payment? Payment { get; set; }
        
        [Required]
        public decimal Rate { get; set; }
        
        [Required]
        public decimal Amount { get; set; }
        
        [Required]
        public DateTime CalculationDate { get; set; } = DateTime.Now;
    }
}
