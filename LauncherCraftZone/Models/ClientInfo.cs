using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherCraftZone.Models
{
    class ClientInfo
    {
        public string Login { get; set; }
        public string IpAddress { get; set; }
        public string VersionClient { get; set; }
        public int Memory { get; set; }
        public bool IsForge { get; set; }
        public string VersionLauncher { get; set; }        
    }
}
