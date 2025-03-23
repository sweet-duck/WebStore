using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Entities
{
    [Table("review")]
    public class Review
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("product_id")]
        [ForeignKey(nameof(Product))]
        public int ProductID { get; set; }

        [Column("user_id")]
        [ForeignKey(nameof(Account))]
        public int AccountID { get; set; }

        [Column("rating")]
        public int Rating { get; set; }

        [Column("comment")]
        public string Comment { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        [Column("anonymous")]
        public bool Anonymous { get; set; }

        public virtual Account Account { get; set; }
        public virtual Product Product { get; set; }

        public Review()
        {
            ID = 0;
            AccountID = 0;
            ProductID = 0;
            Rating = 0;
            Comment = String.Empty;
            CreatedAt = DateTime.Now;
            DeletedAt = null;
            Anonymous = true;
        }
        public Review(int productId, int userId, int rating, string comment, DateTime createdAt, DateTime deletedAt, bool anonymous)
        {
            ID = 0;
            AccountID = userId;
            ProductID = productId;
            Rating = rating;
            Comment = comment;
            CreatedAt = createdAt;
            DeletedAt = deletedAt;
            Anonymous = anonymous;
        }
    }
}
