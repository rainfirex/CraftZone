using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherCraftZone.Models
{
    public class ConfigServer
    {
        public string Memory { get; set; }
        public string Version { get; set; }
        public bool IsGUI { get; set; }
        public string BackupMapDir { get; set; }
        public string MapDir { get; set; }
    }
}
