using System.ComponentModel.DataAnnotations;

namespace VestiModa.Models
{
    public class CartPurchaseItem
    {
        [Key]
        public int CartId { get; set; }



        public Product Product { get; set; } = null!; 
        public int Amount { get; set; }



        [StringLength(200)]
        public string CartPurchaseId { get; set; } = string.Empty;
    }
}
