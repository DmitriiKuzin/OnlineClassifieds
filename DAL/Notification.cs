namespace DAL;

public class Notification
{
    public long Id { get; set; }
    public long UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; } = null!;
    public required string Message { get; set; }
}