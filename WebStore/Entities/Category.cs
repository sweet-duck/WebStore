using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Entities
{
    [Table("category")]
    public class Category
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        public Category()
        {
            ID = 0;
            Name = string.Empty;
            Description = string.Empty;
        }

        public Category(string name, string description)
        {
            ID = 0;
            Name = name;
            Description = description;
        }
    }
}
