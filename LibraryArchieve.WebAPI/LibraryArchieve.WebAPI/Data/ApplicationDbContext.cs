using LibraryArchieve.WebAPI.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace LibraryArchieve.WebAPI.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<AppUserRole> AppUserRoles { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<BookCategory> BookCategories { get; set; }
    public DbSet<BookShelf> BookShelves { get; set; }
    public DbSet<Note> Notes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<BookCategory>().HasKey(p => new { p.BookId, p.CategoryId });

        builder.Entity<BookCategory>().HasOne(bc => bc.Book)
                .WithMany(b => b.BookCategories)
                .HasForeignKey(bc => bc.BookId);

        builder.Entity<BookCategory>().HasOne(bc => bc.Category)
                .WithMany(c => c.BookCategories)
                .HasForeignKey(bc => bc.CategoryId);

        builder.Entity<AppUserRole>().HasKey(p => new { p.UserId, p.RoleId });

        builder.Ignore<IdentityUserRole<Guid>>();
        builder.Ignore<IdentityRoleClaim<Guid>>();
        builder.Ignore<IdentityUserClaim<Guid>>();
        builder.Ignore<IdentityUserLogin<Guid>>();
        builder.Ignore<IdentityUserToken<Guid>>();

        builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Roman", IsActive = true, IsDeleted = false },
                new Category { Id = 2, Name = "Bilim Kurgu", IsActive = true, IsDeleted = false },
                new Category { Id = 3, Name = "Felsefe", IsActive = true, IsDeleted = false },
                new Category { Id = 4, Name = "Tarih", IsActive = true, IsDeleted = false },
                new Category { Id = 5, Name = "Psikoloji", IsActive = true, IsDeleted = false },
                new Category { Id = 6, Name = "Din", IsActive = true, IsDeleted = false },
                new Category { Id = 7, Name = "Bilim", IsActive = true, IsDeleted = false },
                new Category { Id = 8, Name = "Edebiyat", IsActive = true, IsDeleted = false },
                new Category { Id = 9, Name = "Sanat", IsActive = true, IsDeleted = false },
                new Category { Id = 10, Name = "Çocuk", IsActive = true, IsDeleted = false },
                new Category { Id = 11, Name = "Dergi", IsActive = true, IsDeleted = false },
                new Category { Id = 12, Name = "Diğer", IsActive = true, IsDeleted = false },
                new Category { Id = 13, Name = "Bilgisayar", IsActive = true, IsDeleted = false },
                new Category { Id = 14, Name = "Mizah", IsActive = true, IsDeleted = false },
                new Category { Id = 15, Name = "Kurgu", IsActive = true, IsDeleted = false }
                );
    }
}
