using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginManagerObs.Models
{
    public class Plugin
    {
        public string Name { get; set; }
        public PluginInstallationType Installed { get; set; }
        public PluginManagerObs.Classes.DBPluginEntry dbEntry { get; set; }
    }
}
