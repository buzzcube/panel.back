using Microsoft.EntityFrameworkCore;


namespace panel.back
{
    public class DataContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<User> Users { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { }
    }
}
