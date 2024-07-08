namespace LibraryArchieve.WebAPI.Data.Entities;

public class AppUserRole
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public AppRole? Role { get; set; }
}
