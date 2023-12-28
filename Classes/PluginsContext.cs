using Microsoft.EntityFrameworkCore;
using PluginManagerObs.Models;

namespace PluginManagerObs.Classes
{
    public class PluginsContext : DbContext
    {
        public string DbPath { get; }
        public PluginsContext()
        {
            DbPath = "plugins.db";
        }

        public DbSet<Plugin> Plugins { get; set; }
        public DbSet<OBSPath> OBSPaths { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plugin>().ToTable("Plugins");
            modelBuilder.Entity<Plugin>(entity =>
            {
                entity.HasKey(e => e.PluginId);
            });
            modelBuilder.Entity<OBSPath>().ToTable("OBSPaths");
            modelBuilder.Entity<OBSPath>(entity =>
            {
                entity.HasKey(e => e.OBSPathId);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
