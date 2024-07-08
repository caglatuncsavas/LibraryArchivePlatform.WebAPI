namespace LibraryArchive.Domain.Entities;
public sealed class AppUserRole
{
    public Guid RoleId { get; set; }
    public AppRole? Role { get; set; }
    public Guid UserId { get; set; }
}
