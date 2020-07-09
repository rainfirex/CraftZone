using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherCraftZone.Models
{
    class JsonForge 
    {
        public string[] _comment_ = null;
        public string id = null;
        public List<Libraries> libraries = null;
        public Logging logging = null;
        public Arguments arguments = null;
        public string mainClass = null;
        public string inheritsFrom = null;
        public string minecraftArguments = null;
        public string releaseTime = null;
        public string time = null;
        public string type = null;

        public class Logging
        {
            public JsonClient client = null;

            public class JsonClient
            {
                public string argument = null;
                public JsonFile file = null;
                public string type = null;
            }

            public class JsonFile
            {
                public string id = null;
                public string sha1 = null;
                public string size = null;
                public string url = null;
            }
        }

        public class Arguments
        {
            public string[] game = null;
        }

        public class Libraries
        {
            public string name = null;
            public JsonArtifact downloads = null;
            public object natives = null;
            public object rules = null;
            public bool serverreq = false;
            public bool clientreq = false;

            public class JsonArtifact
            {
                public JsonFile artifact = null;
                public object classifiers = null;
            }

            public class JsonFile
            {
                public int size = 0;
                public string sha1 = null;
                public string path = null;
                public string url = null;
            }
        }           
    }
}
