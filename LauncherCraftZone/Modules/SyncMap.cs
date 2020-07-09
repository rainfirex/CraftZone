using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LauncherCraftZone.Models;

namespace LauncherCraftZone.Modules
{
    class SyncMap
    {
        const string DIR_SYNC_MAP = @"\sync_map";


        public void mapInfo(string path)
        {
            DirectoryInfo mapFolder = new DirectoryInfo(path);
        }

        public bool backup(ConfigServer cfg)
        {
            DirectoryInfo currentMap = new DirectoryInfo(cfg.MapDir);

            if (!Directory.Exists(cfg.BackupMapDir + DIR_SYNC_MAP + @"\" + currentMap.Name))
                Directory.CreateDirectory(cfg.BackupMapDir + DIR_SYNC_MAP + @"\" + currentMap.Name);


            string source = cfg.MapDir + @"\";
            string destination = cfg.BackupMapDir + DIR_SYNC_MAP + @"\" + currentMap.Name;
            
            scanFiles(source, destination);

            return true;
        }

        private void copyFiles(string source, string destination)
        {
            string[] files = Directory.GetFiles(source, "*");

            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);

                File.Copy(file, destination  + @"\" + fileInfo.Name, true);
            }
        }


        private void scanFiles(string source, string destination)
        {

            foreach (string dir in Directory.GetDirectories(source))
            {       
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                     
                if (!Directory.Exists(destination + @"\" + dirInfo.Name))
                    Directory.CreateDirectory(destination + @"\" + dirInfo.Name);

                copyFiles(dir, destination + @"\" + dirInfo.Name);

                scanFiles(dir, destination + @"\" + dirInfo.Name);
            }
        }



        public void recovery(ConfigServer cfg)
        {
            DirectoryInfo currentMap = new DirectoryInfo(cfg.MapDir);
        }
    }
}
