using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherCraftZone.Models
{
    public class JsonMinecraft
    {
        public string id;
        public string assets;
        public AssetIndex assetIndex;
        public Downloads downloads;
        public List<Libraries> libraries;
        public Logging logging;
        public Arguments arguments;
        public string mainClass;
        public string minecraftArguments;
        public int minimumLauncherVersion;
        public string releaseTime;
        public string time;
        public string type;

        public class Logging
        {
            public JsonClient client;

            public class JsonClient
            {
                public string argument;
                public JsonFile file;
                public string type;
            }

            public class JsonFile
            {
                public string id;
                public string sha1;
                public string size;
                public string url;
            }
        }

        public class Arguments
        {
            public object[] game;
            
            public object[] jvm;
        }

        public class Libraries
        {
            public string name;
            public Artifact downloads;
            public object natives;
            public object rules;
            public bool serverreq;
            public bool clientreq;

            public class Artifact
            {
                public File artifact;
                public object classifiers;

                public class File
                {
                    public int size;
                    public string sha1;
                    public string path;
                    public string url;
                }
            }
        }

        public class Downloads
        {
            public AbstractObj client;
            public AbstractObj server;
            public AbstractObj client_mappings;
            public AbstractObj server_mappings;

            public class AbstractObj
            {
                public string sha1;
                public int size;
                public string url;
            }
        }

        public class AssetIndex
        {
            public string id;
            public string sha1;
            public int size;
            public int totalSize;
            public string url;
        }
    }
}
