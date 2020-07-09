using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherCraftZone
{
    class ConfigClient
    {
        public string Username { get; set; }
        public string Memory { get; set; }
        public string VersionGame { get; set; }
        public bool IsinstallMods { get; set; }
        public bool IsForge { get; set; }
        public bool IsRift { get; set; }
        public string HostServer { get; set; }
        public string PortGameServer { get; set; }
        public string PortMapServer { get; set; }
    }
}
