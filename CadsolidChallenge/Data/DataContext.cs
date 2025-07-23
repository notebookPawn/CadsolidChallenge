using Microsoft.EntityFrameworkCore;
using CadsolidChallenge.Shared;

namespace CadsolidChallenge.Server.Data
{
    public class DataContext : DbContext
    {

        public  DataContext(DbContextOptions<DataContext> options) : base(options) {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Availability)
                .WithOne(a => a.Equipment)
                .HasForeignKey<Availability>(a => a.EquipmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Equipment> Equipment { get; set; }

        public DbSet<Availability> Availability { get; set; }

    }
}
