using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LauncherCraftZone.Models;

namespace LauncherCraftZone
{    
    class ConfigApp
    {
        const double VERSION = 19;

        const string USER_VERSION = "2.8";

        const string APP_NAME = "Запуск minecraft";

        const string FILE_CLIENT_CONFIG = "client.info";

        const string FILE_SERVER_CONFIG = "server.info";

        const string FILE_LIST_SERVER_CONFIG = "list-server.info";

        public static string currentDir = Directory.GetCurrentDirectory();

        const string DIR_SETTING = "craftzone.config";

        /// <summary>
        /// Директория игры
        /// </summary>
        static string DIR_ROOT = Directory.GetCurrentDirectory();

        public static string getVersion()
        {
            return String.Format("версия {0}", USER_VERSION);
        }

        public string getApplicationName() {
            return APP_NAME;
        }

        public static string getAppTitle() {
            return String.Format("{0} версия {1}", APP_NAME, USER_VERSION);
        }

        /// <summary>
        /// Создание директории для хранение файлов лаунчера
        /// </summary>
        public static void initDirSetting()
        {
             if(!Directory.Exists(DIR_ROOT + @"\"+ DIR_SETTING))
             {
                 Directory.CreateDirectory(DIR_ROOT + @"\" + DIR_SETTING);
             }
        }

        //Сохранить конфигурацию
        public static void SaveClientSetting(ConfigClient cf)
        {
            string fileConfig = DIR_ROOT + @"\" + DIR_SETTING + @"\" + FILE_CLIENT_CONFIG;

            StreamWriter save = new StreamWriter(fileConfig);
            save.WriteLine(cf.Username);
            save.WriteLine(cf.Memory);
            save.WriteLine(cf.VersionGame);
            save.WriteLine(cf.IsForge);
            save.WriteLine(cf.IsinstallMods);
            save.WriteLine(cf.HostServer);
            save.WriteLine(cf.PortGameServer);
            save.WriteLine(cf.PortMapServer);
            save.Close();
        }

        public static void SaveServerSetting(ConfigServer cf)
        {
            string fileConfig = DIR_ROOT + @"\" + DIR_SETTING + @"\" + FILE_SERVER_CONFIG;

            StreamWriter save = new StreamWriter(fileConfig);
            save.WriteLine(cf.Memory);
            save.WriteLine(cf.Version);
            save.WriteLine(cf.IsGUI);
            save.WriteLine(cf.BackupMapDir);
            save.WriteLine(cf.MapDir);
            save.Close();
        }

        public static void SaveListServer(string text)
        {
            string file = DIR_ROOT + @"\" + DIR_SETTING + @"\" + FILE_LIST_SERVER_CONFIG;
            StreamWriter sw = File.AppendText(file);
            sw.WriteLine(text);
            sw.Close();
        }

        // Пересохраняет список полностью
        public static void ReSaveListServer(string[] array)
        {
            string file = DIR_ROOT + @"\" + DIR_SETTING + @"\" + FILE_LIST_SERVER_CONFIG;
            File.WriteAllLines(file, array);
        }

        public static string[] LoadListServer()
        {
            string file = DIR_ROOT + @"\" + DIR_SETTING + @"\" + FILE_LIST_SERVER_CONFIG;
            
            if (File.Exists(file))
                return File.ReadAllLines(file);
            else
                return new string[0];
        }

        //Загрузка настроек
        public static ConfigClient LoadClientSetting()
        {
            ConfigClient cf = new ConfigClient();
            string fileConfig = DIR_ROOT + @"\"+ DIR_SETTING + @"\" + FILE_CLIENT_CONFIG;
            if (!File.Exists(fileConfig))
            {
                return cf;
            }

            StreamReader load = new StreamReader(fileConfig);
            cf.Username = load.ReadLine();
            cf.Memory = load.ReadLine();
            cf.VersionGame = load.ReadLine();            
            cf.IsForge = Convert.ToBoolean(load.ReadLine());
            cf.IsinstallMods = Convert.ToBoolean(load.ReadLine());
            cf.HostServer = load.ReadLine();
            cf.PortGameServer = load.ReadLine();
            cf.PortMapServer = load.ReadLine();
            load.Close();
            return cf;
        }

        public static ConfigServer LoadServerSetting()
        {
            ConfigServer cf = new ConfigServer();
            string fileConfig = DIR_ROOT + @"\"+ DIR_SETTING + @"\" + FILE_SERVER_CONFIG;
            if (!File.Exists(fileConfig))
            {
                return cf;
            }

            StreamReader load = new StreamReader(fileConfig);
            cf.Memory = load.ReadLine();
            cf.Version = load.ReadLine();
            cf.IsGUI = Convert.ToBoolean(load.ReadLine());
            cf.BackupMapDir = load.ReadLine();
            cf.MapDir = load.ReadLine();
            load.Close();
            return cf;
        }
                                                                      
    }    
}
