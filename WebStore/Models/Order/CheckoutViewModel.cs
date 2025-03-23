namespace WebStore.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CheckoutViewModel
    {
    /*    [Required(ErrorMessage = "Zadejte své celé jméno!")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Zadejte E-Mailovou Adresu!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Zadejte Telefonní Číslo!")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Zadejte Adresu doručení!")]
        public string Address { get; set; }*/

        [Required(ErrorMessage = "Zvolte způsob doručení.")]
        public string DeliveryMethod { get; set; }

        [Required(ErrorMessage = "Zvolte způsob platby.")]
        public string PaymentMethod { get; set; }

        public List<Cart.CartItem> CartItems { get; set; }
    }

}
