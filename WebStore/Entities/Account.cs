using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Entities
{
    [Table("account")]
    public class Account
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("role")]
        public string Role { get; set; }

        public Account()
        {
            ID = 0;
            Username = string.Empty;
            Password = string.Empty;
            Role = "user";
        }
        public Account(string username, string password, string role)
        {
            ID = 0;
            Username = username;
            Password = password;
            Role = role;
        }
    }
}
