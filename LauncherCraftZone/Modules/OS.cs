using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LauncherCraftZone
{                                                                          
    public delegate void minecraftRunning(Process minecraft);

    public delegate void interfaceMinecraftDisabled();

    public delegate void serverRunning(Process server);

    public delegate void interfaceServerDisabled();

    class OS
    {

        private Process server; 
        private Process minecraft;

        private bool isServerRunning = false;
        private bool isMinecraftRunning = false;


        public minecraftRunning minecraftRunning = null;

        public interfaceMinecraftDisabled interfaceMinecraftDisabled = null;

        public serverRunning serverRunning = null;

        public interfaceServerDisabled interfaceServerDisabled = null;

        const int THREAD_SLEEP = 7000;

        private const string JAVA_FOLDER_NAME = @"\Java\";

        /// <summary>
        /// Имя пользователя
        /// </summary>
        private string username = Environment.UserName;

        private bool isOSx64 = Environment.Is64BitOperatingSystem;

        private int[] DDR;

        private string javaX86;

        private string javaX64;

        public OS()
        {
            this.initJavaX86();

            if (isOSx64)
            {
                this.initJavaX64();
            }
        }

        //пОЛУЧЕНИЕ СВОБОДНОЙ ПАМЯТИ
        public int[] freeMemory()
        {
            PerformanceCounter remCount = new PerformanceCounter("Memory", "Available MBytes"); //("Memory", "TotalPhysicalMemory");
            long freeDDR = remCount.RawValue;
            int start = (int)freeDDR / 1024;
            DDR = new int[start];

            for (int i = 0; i < start; i++) {
                DDR[i] = 1024 * (i + 1);
            }
            return DDR.OrderByDescending(x => x).ToArray();            
        }

        public string getUserName() {
            return username;
        }

        public string getIsOSx64() {
            if (isOSx64)
                return "Ваша система 64х битная.";
            else
                return "Ваша система 32х битная.";
        }

        private void initJavaX86()
        {
            string pf = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            string jv = pf + JAVA_FOLDER_NAME;
            if (Directory.Exists(jv))
            {
                DirectoryInfo di = new DirectoryInfo(jv);
                DirectoryInfo[] dirs = di.GetDirectories();
                if (dirs.Length > 0)
                {
                    jv += dirs[0] + @"\bin";

                    if (Directory.Exists(jv))
                        javaX86 = jv;
                }
            }
        }

        private void initJavaX64()
        {
            string pf = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string pattern = "(x86)";
            pf = pf.Replace(pattern, "").Trim();

            string jv = pf + JAVA_FOLDER_NAME;
            if (Directory.Exists(jv))
            {
                DirectoryInfo di = new DirectoryInfo(jv);
                DirectoryInfo[] dirs = di.GetDirectories();

                Regex regex = new Regex(@"jre(\w*)");
                
                for (int i = 0; i < dirs.Length; i++)
                {
                    MatchCollection match = regex.Matches(dirs[i].Name);
                    if (match.Count > 0)
                    {
                        jv += dirs[i] + @"\bin";

                        if (Directory.Exists(jv))
                            javaX64 = jv;

                        return;
                    }
                }

                    
            }
        }

        public string getJavaX86()
        {
            return javaX86;
        }

        public string getJavaX64()
        {
            return javaX64;
        }

        public string getJavaInfo() {
            string textInfo = "";
            if(Directory.Exists(javaX86))
                textInfo = " Установлена Java x86.";
            if(Directory.Exists(javaX64))
                textInfo += " Установлена Java x64.";
            if (String.IsNullOrEmpty(textInfo))
                textInfo = "На копмьютере Java не установлена!";
                
            return textInfo;
        }

        public string selectJava(string memory)
        {
            int selectMemory = Convert.ToInt32(memory);
            if (selectMemory < 2048)
                return getJavaX86();
            else
                return getJavaX64();
        }

        public static DirectoryInfo[] getDirs(string dir)
        {
            DirectoryInfo info = new DirectoryInfo(dir);
            return (Directory.Exists(dir)) ? info.GetDirectories() : null;
        }

        /// <summary>
        /// Получить процессы сервера и игры
        /// </summary>
        private void getProccess()
        {
            Process[][] java = new Process[2][];
            java[0] = Process.GetProcessesByName("java");
            java[1] = Process.GetProcessesByName("javaw");

            
            foreach (Process[] p in java)
            {
                if (p.Length <= 0) continue;

                // bat файл сервер
                if (p[0].ProcessName == "java" && p[0].MainWindowTitle == "")
                {
                    server = p[0];
                    this.isServerRunning = true;
                    continue;
                }

                // Сервер gui
                if (p[0].ProcessName == "javaw" && p[0].MainWindowTitle == "Minecraft server")
                {
                    continue;
                }


                Regex regex = new Regex(@"minecraft (\w*)", RegexOptions.IgnoreCase);
                MatchCollection matches = regex.Matches(p[0].MainWindowTitle);

                // Minecraft
                if (p[0].ProcessName == "javaw" && matches.Count == 1)
                {
                    minecraft = p[0];
                    this.isMinecraftRunning = true;
                    continue;
                }

                
            }

        }

        /// <summary>
        /// Запустить отслеживание
        /// </summary>     
        public void startMonitor()
        {
            Thread th = new Thread(monitoring);
            th.IsBackground = true;
            th.Start(); 
        }

        /// <summary>
        /// Отслеживание
        /// </summary>
        private void monitoring()
        {           

            while (true)
            {
                isServerRunning = false;
                isMinecraftRunning = false;
                server = null;
                minecraft = null;

                getProccess();


                if (isServerRunning)
                {
                    if (serverRunning != null)
                        serverRunning(this.server);
                }
                else
                {
                    if (interfaceServerDisabled != null)
                        interfaceServerDisabled();
                }

                if (isMinecraftRunning)
                {
                    if (minecraftRunning != null)
                        minecraftRunning(this.minecraft);
                }
                else
                {
                    if (interfaceMinecraftDisabled != null)
                        interfaceMinecraftDisabled();
                }
                System.Threading.Thread.Sleep(100);             //THREAD_SLEEP
            }            
        }
    }
}
