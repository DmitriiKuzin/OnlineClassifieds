using Classifieds.Auth;

namespace DAL;

public class UserProfile
{
    public long Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public string PasswordHash { get; set; }
    public int RoleId { get; set; } = (int) Roles.User;
    public Role Role { get; set; }
}

public class Role
{
    public int Id { get; set; }
    public required string Name { get; set; }
}