using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginManagerObs.Models
{
    public class Plugin
    {
        public decimal PluginId { get; set; }
        public string Name { get; set; }
        public bool IsInstalled { get; set; }
        public long InstalledDate { get; set; }
        public decimal OBSPath { get; set; }
    }
}
