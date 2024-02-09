namespace MyLittleInstagram.Data.Entities;

public class Friendship : BaseEntity
{
    public uint User1Id { get; set; }
    public User User1 { get; set; }

    public uint User2Id { get; set; }
    public User User2 { get; set; }
}