using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Collection.model
{
    class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("DefaultConnection")
        {
        }
        public DbSet<Collect> Collects { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Collect>()
            .HasKey(p => new { p.Id });
            modelBuilder.Entity<Collect>().Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
