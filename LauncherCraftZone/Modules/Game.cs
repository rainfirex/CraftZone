using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LauncherCraftZone.Models;
using System.Diagnostics;
using LauncherCraftZone.Modules;
using System.Net;

namespace LauncherCraftZone
{
    class Game
    {
        string mainClass;  // Класс запуска

        string modParams = ""; // параметры

        List<String> libraries;

        private String UUID = Guid.NewGuid().ToString().Replace("-", "");

        private String accessToken = Guid.NewGuid().ToString().Replace("-", "");

        /// <summary>
        /// директория версий клиента
        /// </summary>
        ///                                                                               
        const string DIR_GAME_VERSIONS = @"\versions";

        public DirectoryInfo[] getGameVersions() {
            DirectoryInfo[] dirs =  OS.getDirs(ConfigApp.currentDir + DIR_GAME_VERSIONS);
            return dirs.OrderByDescending(x => x.Name).ToArray();
        }

        public bool run(ConfigClient cfg, string java) {

            string pathNatives = ConfigApp.currentDir + @"\natives\" + cfg.VersionGame;

            if (!existNatives(pathNatives))
            {
                System.Windows.MessageBox.Show("Библиотеки natives не найдены");
                return false;
            }

            libraries = new List<String>();
            JsonForge jsonForge = null;
            JsonMinecraft jsonNative = null;
            JsonRift jsonRift = null;

            // Загрузка Rift библиотек
            if (cfg.IsRift)
                jsonRift = addRaftLibs(cfg.VersionGame);

            // Загрузка Forge библиотек
            if (cfg.IsForge)
                jsonForge = loadLibrariesForge(cfg.VersionGame);

            //Загрузка Native библиотек
            jsonNative = loadNativeLibraries(cfg.VersionGame);

            if (jsonNative == null) { System.Windows.MessageBox.Show("Библиотеки не загружены"); return false; }

            // Установка класса запуска
            if (cfg.IsForge)
            {
                mainClass = jsonForge.mainClass;
            }
            else
            {
                mainClass = jsonNative.mainClass;
            }

            // Переустановка модов
            if (cfg.IsinstallMods)
                ModForge.install(cfg.VersionGame);

            // Строка запуска
            string param = generateParamString(cfg.Memory, pathNatives, jsonNative, cfg.Username, cfg.HostServer, cfg.PortGameServer);

            if (cfg.IsForge)
            {
                if (cfg.VersionGame == "1.12.2" || cfg.VersionGame == "1.11.2")
                    param += @" --tweakClass net.minecraftforge.fml.common.launcher.FMLTweaker";         // --tweakClass cpw.mods.fml.common.launcher.FMLTweaker
              
                param += " " + modParams;
            }

            try
            {
                Process start = new Process();
                start.StartInfo.CreateNoWindow = true;
                start.StartInfo.UseShellExecute = false;
                start.StartInfo.Arguments = param;
                start.StartInfo.FileName = java + @"\javaw.exe";
                start.Start();
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message + " " + ex.StackTrace);
            }

            return false;
        }

        private string generateParamString(string memory, string pathNatives, JsonMinecraft json, string username, string server, string port) 
        {
            // Начальная строка запуска
            string param = @" -Dfml.ignoreInvalidMinecraftCertificates=true -Dfml.ignorePatchDiscrepancies=true -Xmx" + memory + "M -Djava.library.path=" + pathNatives + " -cp ";
            // Формирование перебором добавленных файлов библиотоек из списка, в строку с ;
            foreach (string file in libraries)
                param += file + ";";

            //Параметры запуска!
            param += ConfigApp.currentDir + @"\versions\" + json.id + @"\" + json.id + ".jar " + mainClass //" net.minecraft.launchwrapper.Launch   net.minecraft.client.main.Main"
             + @" --username " + username
             + @" --version " + json.id
             + @" --gameDir " + ConfigApp.currentDir
             + @" --assetsDir " + ConfigApp.currentDir + @"\" + "assets"
             + @" --assetIndex " + json.assets
             + @" --uuid " + UUID
             + @" --accessToken " + accessToken   //"FML"  //Сессия
             + @" --userType legacy"                       //Тип аккаунта	mojang/legacy	mojang
             + @" --versionType CraftZone";
             //+ @" --server localhost"                             //IP сервера, к которому будет осуществлено подключение после запуска игры(Необязательно)
             //+ @" --port 25565";                     //Порт сервера, к которому будет осуществлено подключение после запуска игры(При введённом IP)
             //+ @" --userProperties {} " без этого параметра все отлично запускается!!!;

            if (!String.IsNullOrEmpty(server) && !String.IsNullOrEmpty(port))
            {
                param += @" --server " + server + @" --port " + port;
            }

            return param;
        }



        /// <summary>
        /// Нативные библотеки
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        private JsonMinecraft loadNativeLibraries(string version)
        {
            string jsonMinecraft = ConfigApp.currentDir + DIR_GAME_VERSIONS + @"\" + version + @"\" + version + ".json";

            if (File.Exists(jsonMinecraft))
            {
                JsonMinecraft json = jsonParseMinecraft(jsonMinecraft);
                //mainClass = json.mainClass;

                foreach (JsonMinecraft.Libraries lib in json.libraries)
                {
                    if (lib.downloads.artifact == null)
                    {
                        Console.WriteLine("artifact: null");
                        continue;
                    }
                     

                    string fileJar = ConfigApp.currentDir + @"\libraries\" + lib.downloads.artifact.path.Replace('/', '\\');
                    if (File.Exists(fileJar))
                    {
                        string item = libraries.Find(x => x.Contains(fileJar));
                        //foreach (var item in libraries)
                        //{
                        //    if (item == fileJar)
                        //        break;
                        //}

                        if (item == null)
                        {
                            libraries.Add(fileJar);
                        }
                        else
                        {
                          Console.WriteLine("Повторный элемент: " + item);
                        }

                    }
                    else
                    {
                        Console.WriteLine("Не найден файл: " + fileJar);
                        Console.WriteLine(lib.downloads.artifact.url);
                        Console.WriteLine("---------------");

                        //try
                        //{
                            //Directory.CreateDirectory(fileJar);

                            // Качает файл
                            //WebClient wb = new WebClient();
                            //wb.DownloadFile(lib.downloads.artifact.url, fileJar);
                        //}
                        //catch (WebException ex) { Console.WriteLine(ex.InnerException); }
                    }
                        
                }
                return json;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Forge библиотеки
        /// </summary>
        /// <param name="cfg"></param>
        private JsonForge loadLibrariesForge(string version)
        {
            string jsonFileVersionForge = ConfigApp.currentDir + DIR_GAME_VERSIONS + @"\" + version + @"\" + version + "-forge.json";

            if (File.Exists(jsonFileVersionForge))
            {
                JsonForge json = jsonParseForge(jsonFileVersionForge);

                //mainClass = json.mainClass;
                if (json.arguments != null)
                    modParams = String.Join(" ", json.arguments.game);

                foreach (JsonForge.Libraries lib in json.libraries)
                {
                    if (json.inheritsFrom == "1.12.2" || json.inheritsFrom == "1.11.2")
                    {
                        //string name = lib.name.Replace(":", @"\");
                        string[] dirs = lib.name.Split(':');
                        string name = dirs[0].Replace('.', '\\');

                        for (int j = 1; j < dirs.Length; j++)
                        {
                            name += "\\" + dirs[j];
                        }

                        string dir = ConfigApp.currentDir + @"\libraries\" + name + "\\";
                        if (Directory.Exists(dir))
                        {
                            string[] files = Directory.GetFiles(dir);
                            for (int i = 0; i < files.Length; i++)
                            {
                                libraries.Add(files[i]);
                            }
                        }

                    }
                    else
                    {
                        string filepath = ConfigApp.currentDir + @"\libraries\" + lib.downloads.artifact.path.Replace('/', '\\');
                        libraries.Add(filepath);
                    }
                }
                return json;
            }
            return null;
        }

        /// <summary>
        /// Raft библиотеки
        /// </summary>
        /// <param name="cfg"></param>
        private JsonRift addRaftLibs(string version)
        {
            string jsonFileRift = ConfigApp.currentDir + DIR_GAME_VERSIONS + @"\" + version + @"\" + version + "-rift.json";
            if (File.Exists(jsonFileRift))
            {
                JsonRift jsonRift = JsonParseRift(jsonFileRift);

                mainClass = jsonRift.mainClass;
                if (jsonRift.arguments != null)
                {
                    string par = String.Join(" ", jsonRift.arguments.game);
                    modParams += " " +par;
                }

                foreach (JsonRift.Libraries lib in jsonRift.libraries)
                {
                    string name = lib.name.Replace('.', '\\');
                }

                return jsonRift;
            }
            return null;
        }

    

        /// <summary>
        /// Проверка natives библиотек
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool existNatives(string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo d = new DirectoryInfo(path);
                FileInfo[] files = d.GetFiles();
                if (files.Length > 0) return true;
            }

            return false;
        }

        private JsonMinecraft jsonParseMinecraft(string file)
        {
            if (File.Exists(file)) {
                string textJson = File.ReadAllText(file);
                return JsonConvert.DeserializeObject<JsonMinecraft>(textJson);
            }
            return null;
        }

        private JsonForge jsonParseForge(string file)
        {
            if (File.Exists(file))
            {
                string textJson = File.ReadAllText(file);
                return JsonConvert.DeserializeObject<JsonForge>(textJson);
            }
            return null;
        }  
        
        private JsonRift JsonParseRift(string file) {
            if (File.Exists(file))
            {
                string textJson = File.ReadAllText(file);
                return JsonConvert.DeserializeObject<JsonRift>(textJson);
            }
            return null;
        }
    }
}
