using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TestTask.DAL.Models;

namespace TestTask.DAL
{
    public class TestTaskDbContext : DbContext
    {
        public TestTaskDbContext(DbContextOptions<TestTaskDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductionRoom> ProductionRooms { get; set; }
        public DbSet<Contract> Contracts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }

        public async Task<bool> SaveAndCompareAffectedRowsAsync()
        {
            try
            {
                var affectedRows = GetAffectedRows();
                return await SaveChangesAsync() >= affectedRows.Count;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        private List<EntityEntry> GetAffectedRows()
        {
            var affectedRows = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added
                            || e.State == EntityState.Modified
                            || e.State == EntityState.Deleted)
                .ToList();
            foreach (var affectedRow in affectedRows)
            {
                var property = affectedRow.Properties.FirstOrDefault(p => p.Metadata.Name == "Modified");
                if (property != null)
                    property.CurrentValue = DateTime.UtcNow;
            }

            return affectedRows;
        }
    }
}