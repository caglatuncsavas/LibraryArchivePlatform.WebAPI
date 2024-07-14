namespace LibraryArchieve.WebAPI.Data.Entities;

public sealed class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<BookCategory>? BookCategories { get; set; }
}
