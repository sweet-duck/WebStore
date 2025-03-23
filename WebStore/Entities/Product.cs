using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Entities
{
    [Table("product")]
    public class Product
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("category_id")]
        [ForeignKey(nameof(Category))]
        public int CategoryID { get; set; }

        [Column("product_name")]
        public string Name { get; set; }

        [Column("product_description")]
        public string Description { get; set; }

        [Column("product_brand")]
        public string Brand { get; set; }

        [Column("product_price")]
        public decimal Price { get; set; }

        /***
         * Sale range 1-100 
        **/
        [Column("product_sale")]
        public byte Sale {  get; set; }

        public virtual Category Category { get; set; }

        public Product()
        {
            ID = 0;
            CategoryID = 0;
            Name = string.Empty;
            Description = string.Empty;
            Brand = string.Empty;
            Price = 0;
            Sale = 0;
        }
        public Product(int categoryID, string name, string description, string brand, decimal price, byte sale)
        {
            ID = 0;
            CategoryID = categoryID;
            Name = name;
            Description = description;
            Brand = brand;
            Price = price;
            Sale = sale;
        }
    }
}
