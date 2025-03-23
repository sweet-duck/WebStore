namespace WebStore.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CheckoutViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        [Required(ErrorMessage = "Zvolte způsob doručení.")]
        public string DeliveryMethod { get; set; }

        [Required(ErrorMessage = "Zvolte způsob platby.")]
        public string PaymentMethod { get; set; }

        public List<Cart.CartItem> CartItems { get; set; }
    }

}
