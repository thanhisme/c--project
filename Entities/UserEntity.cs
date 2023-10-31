using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Entities
{
    [Index(nameof(Username), IsUnique = true, Name = "Username")]
    public class UserEntity
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
