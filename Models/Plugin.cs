﻿using System;
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PluginId { get; set; }
        public string Name { get; set; }
        public bool IsInstalled { get; set; }
        public long InstalledDate { get; set; }
        public int OBSPathId { get; set; }
        [ForeignKey("OBSPathId")]

        public virtual OBSPath OBSPath { get; set; }
    }
}
