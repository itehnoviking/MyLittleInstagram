using System.Xml.Linq;

namespace MyLittleInstagram.Data.Entities;

public class User : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string? NormalizedEmail { get; set; }
    public string? PasswordHash { get; set; }
    public DateTime RegistrationDate { get; set; }

    public virtual IEnumerable<Image> Images { get; set; }
    public virtual IEnumerable<Friendship> Friendships { get; set; }
    public virtual IEnumerable<RefreshToken> RefreshTokens { get; set; }
}