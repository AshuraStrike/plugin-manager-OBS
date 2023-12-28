using Microsoft.EntityFrameworkCore;
using PluginManagerObs.Models;

namespace PluginManagerObs.Classes
{
    public class PluginsContext : DbContext
    {
        public DbSet<Plugin> Plugins { get; set; }
        public string DbPath { get; }
        public PluginsContext()
        {
            DbPath = "plugins.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plugin>().ToTable("Plugins");
            modelBuilder.Entity<Plugin>(entity =>
            {
                entity.HasKey(e => e.Name);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
