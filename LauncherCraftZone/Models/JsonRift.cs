using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherCraftZone.Models
{
    class JsonRift
    {
        public string id = null;
        public string inheritsFrom = null;
        public string releaseTime = null;
        public string time = null;
        public string type = null;
        public Arguments arguments = null;
        public string mainClass = null;
        public List<Libraries> libraries = null;


         public class Arguments
         {
             public string[] game = null;
         }

         public class Libraries {
             public string name = null;
             public string url = null;
         }
    }
}
