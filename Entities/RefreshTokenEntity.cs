using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Entities
{
    public class RefreshTokenEntity
    {
        [Key]
        public int Id { get; set; }

        public virtual UserEntity User { get; set; }

        public string Token { get; set; }

        public DateTime Expiry { get; set; }

        [DefaultValue(false)]
        public bool IsRevoked { get; set; }
    }
}
