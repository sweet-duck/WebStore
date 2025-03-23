using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Entities
{
    [Table("item")]
    public class Item
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("product_id")]
        [ForeignKey(nameof(Product))]
        public int ProductID { get; set; }

        [Column("serial_number")]
        public string SerialNumber { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("warehouse_location")]
        public string WarehouseLocation { get; set; }

        public Item()
        {
            ID = 0;
            ProductID = 0;
            SerialNumber = string.Empty;
            Status = string.Empty;
            WarehouseLocation = string.Empty;
        }
        public Item(int productId, string serialNumber, string status, string warehouseLocation)
        {
            ID = 0;
            ProductID = productId;
            SerialNumber = serialNumber;
            Status = status;
            WarehouseLocation = warehouseLocation;
        }

        // Musí být použit až v IEnumerable
        public Boolean IsInStock()
        {
            if (Status == null) return false;
            return Status.Contains("sklad", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
