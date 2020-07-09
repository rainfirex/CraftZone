using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LauncherCraftZone.Models;

namespace LauncherCraftZone.Modules
{
    class Server
    {
        public const string SERVER_DIR = @"\server";

        private const string SERVER_FILE = "server.jar";

        private const string CONFIG_FILE = "server.properties";

        private string currentVersion;

        const string BAT_FILE_NAME = "run.bat";

        /// <summary>
        /// Если запуск сервера скидывается, то скорее всего занят порт нужно убить процес 
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="java"></param>
        public bool run(ConfigServer cfg, string java) 
        {
            string batFile = ConfigApp.currentDir + SERVER_DIR + @"\" + cfg.Version + @"\" + BAT_FILE_NAME;

            if (File.Exists(batFile))
            {
                Process start = new Process();
                start.StartInfo.WorkingDirectory = ConfigApp.currentDir + SERVER_DIR + @"\" + cfg.Version + @"\";

                if (cfg.IsGUI)
                {
                    string serverFile = ConfigApp.currentDir + SERVER_DIR + @"\" + cfg.Version + @"\" + SERVER_FILE;
                    string param = String.Format(" -Xms512M -Xmx{0}M -jar {1}", cfg.Memory, serverFile);
                    start.StartInfo.CreateNoWindow = true;
                    start.StartInfo.UseShellExecute = false;
                    start.StartInfo.Arguments = param;
                    start.StartInfo.FileName = java + @"\javaw.exe";
                }
                else
                {
                    string param = String.Format("java -Xms512M -Xmx{0}M -jar {1} nogui", cfg.Memory, SERVER_FILE);
                    System.IO.File.WriteAllText(batFile, param);
                    start.StartInfo.FileName = batFile;
                }

                start.Start();
                start.Close();
                return true;
            }
            return false;            
        }

       public System.IO.DirectoryInfo[] getVersions() {
           if (Directory.Exists(ConfigApp.currentDir + SERVER_DIR)) {
               DirectoryInfo[] dirs = OS.getDirs(ConfigApp.currentDir + SERVER_DIR);
               return dirs.OrderByDescending(x => x.Name).ToArray();
           }
         return null;
       }

       public string getConfig(ConfigServer cfg) 
       {
           string file = ConfigApp.currentDir + @"\" + SERVER_DIR + @"\" + cfg.Version + @"\" + CONFIG_FILE;
           if (File.Exists(file))
           {
               currentVersion = cfg.Version;
               return File.ReadAllText(file);
           }
           return "";
       }

       public void saveConfig(string content)
       {
           string file = ConfigApp.currentDir + @"\" + SERVER_DIR + @"\" + currentVersion + @"\" + CONFIG_FILE;
           File.WriteAllText(file, content);
       }

       public void openServer(string versionDir)
       {
           Process.Start(ConfigApp.currentDir + SERVER_DIR + @"\" + versionDir);
       }
    }
}
