using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace CostPlan
{
    class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("DefaultConnection")
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>()
            .HasKey(p => new { p.Id });
            modelBuilder.Entity<Expense>().Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
