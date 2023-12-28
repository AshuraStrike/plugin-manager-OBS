using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginManagerObs.Models
{
    public class OBSPath
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OBSPathId { get; set; }
        public string Path { get; set; }

        public virtual ICollection<Plugin> Plugins { get; set; }
    }
}
