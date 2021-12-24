using Decorus.Models;
using Microsoft.EntityFrameworkCore;

namespace Decorus.DataAccess
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Cover> Covers { get; set; }
    }
}
