namespace MyLittleInstagram.Data.Entities;

public class Image : BaseEntity
{
    public string Name { get; set; }
    public DateTime CreationDateTime { get; set; }
    public string Content { get; set; }
    public bool IsAvatar { get; set; }

    public uint UserId { get; set; }
    public virtual User User { get; set; }

}