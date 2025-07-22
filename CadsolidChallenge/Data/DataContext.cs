using Microsoft.EntityFrameworkCore;
using CadsolidChallenge.Shared;

namespace CadsolidChallenge.Server.Data
{
    public class DataContext : DbContext
    {

        public  DataContext(DbContextOptions<DataContext> options) : base(options) {
        }

        public DbSet<Equipment> Equipment { get; set; }

        public DbSet<Availability> Availability { get; set; }

    }
}
