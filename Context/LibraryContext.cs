using Microsoft.EntityFrameworkCore;
using Nathan.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Nathan.Context
{
    public class LibraryContext: IdentityDbContext<UserModel>
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        // Define DbSet properties for your entities (e.g., User)
        public DbSet<Author> Authors { get; set; }

        public DbSet<Book> Books { get; set; }
    }
}
