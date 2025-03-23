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

        public virtual Account Account { get; set; }
        public virtual Item Item { get; set; }

        public Order()
        {
            ID = 0;
            AccountID = 0;
            ItemID = 0;
        }
        public Order(int userId, int itemId)
        {
            ID = 0;
            AccountID = userId;
            ItemID = itemId;
        }
    }
}
