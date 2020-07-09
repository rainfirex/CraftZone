using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherCraftZone.Modules
{
    class ModForge
    {
        static string PATH_MOD_INSTALLER;

        static string PATH_MOD;

        /// <summary>
        /// Директория
        /// </summary>
        const string DIR_MOD = @"\mods";

        /// <summary>
        /// Директория
        /// </summary>
        const string DIR_MOD_INSTALLER = @"\mods_install";

        static List<String> listMods;

        public static void install(string versionGame)
        {
            if (Directory.Exists(ConfigApp.currentDir + DIR_MOD))
                PATH_MOD = ConfigApp.currentDir + DIR_MOD;
            if (Directory.Exists(ConfigApp.currentDir + DIR_MOD_INSTALLER))
                PATH_MOD_INSTALLER = ConfigApp.currentDir + DIR_MOD_INSTALLER;


            listMods = new List<string>();

            clearMods();

            if (initMods(versionGame))
            {
                copyMods();
            }
        }

        private static bool initMods(string versionGame)
        {
            if (Directory.Exists(PATH_MOD_INSTALLER + @"\" + versionGame))
                listMods.AddRange(Directory.GetFiles(PATH_MOD_INSTALLER + @"\" + versionGame));

            return (listMods.Count > 0) ? true : false;
        }

        private static void clearMods() {
            if (Directory.Exists(PATH_MOD)) {
                Directory.Delete(PATH_MOD, true);
            }
        }

        private static void copyMods() 
        {
            if (!Directory.Exists(ConfigApp.currentDir + DIR_MOD)) {
                PATH_MOD = ConfigApp.currentDir + DIR_MOD;
                Directory.CreateDirectory(PATH_MOD);
            }                

            listMods.ForEach(delegate(string file) {
                FileInfo fi = new FileInfo(file);
                File.Copy(file, PATH_MOD+@"\"+fi.Name);
                Console.WriteLine(file);
            });
        }
    }
}
