using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Entities
{
    [Table("order")]
    public class Order
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("user_id")]
        [ForeignKey(nameof(Account))]
        public int AccountID { get; set; }

        [Column("item_id")]
        [ForeignKey(nameof(Item))]
        public int ItemID { get; set; }

        [Column("delivery_method")]
        public string DeliveryMethod { get; set; }

        [Column("payment_method")]
        public string PaymentMethod { get; set; }

        public virtual Account Account { get; set; }
        public virtual Item Item { get; set; }

        public Order()
        {
            ID = 0;
            AccountID = 0;
            ItemID = 0;
            DeliveryMethod = string.Empty;
            PaymentMethod = string.Empty;
        }

        public Order(int userId, int itemId, string deliveryMethod, string paymentMethod)
        {
            ID = 0;
            AccountID = userId;
            ItemID = itemId;
            DeliveryMethod = deliveryMethod;
            PaymentMethod = paymentMethod;
        }
    }
}
