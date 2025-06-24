using Microsoft.EntityFrameworkCore;
using PluginManagerObs.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PluginManagerObs.Classes
{
    public class DBOBSPathEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OBSPathId { get; set; }
        public string Path { get; set; }

        public virtual ICollection<Classes.DBPluginEntry> Plugins { get; set; }
    }

    public class DBPluginEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PluginId { get; set; }
        public string Name { get; set; }
        public bool IsInstalled { get; set; }
        public long InstalledDate { get; set; }
        public int OBSPathId { get; set; }
        public virtual DBOBSPathEntry OBSPath { get; set; }
    }


    public class DBPluginsContext : DbContext
    {
        public string DbPath { get; }
        public DBPluginsContext()
        {
            DbPath = "plugins.db";
        }

        public DbSet<DBPluginEntry> Plugins { get; set; }
        public DbSet<DBOBSPathEntry> OBSPaths { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBPluginEntry>().ToTable("Plugins");
            modelBuilder.Entity<DBPluginEntry>(entity =>
            {
                entity.HasKey(e => e.PluginId);
            });
            modelBuilder.Entity<DBOBSPathEntry>().ToTable("OBSPaths");
            modelBuilder.Entity<DBOBSPathEntry>(entity =>
            {
                entity.HasKey(e => e.OBSPathId);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
