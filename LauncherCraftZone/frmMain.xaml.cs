using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LauncherCraftZone.Modules;
using LauncherCraftZone.Models;
using LauncherCraftZone.Modules.http;
using System.Diagnostics;

namespace LauncherCraftZone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ConfigClient cfgClient;
        ConfigServer cfgServer;
        string[] listServer;

        OS os;
        Game game = new Game();
        Server server = new Server();

        WebServer webServer;

        public MainWindow()
        {
            InitializeComponent();            
        }        

        private void frmMain_Loaded(object sender, RoutedEventArgs e)
        {
            ConfigApp.initDirSetting();

            webServer = new WebServer(clientInfoAdd);
    
            os = new OS();
            os.interfaceServerDisabled = interfaceServerDisabled;
            os.interfaceMinecraftDisabled = interfaceMinecraftDisabled;
            os.serverRunning = serverRunning;
            os.minecraftRunning = minicraftRunning;
            os.startMonitor();

            this.frmMain.Title = ConfigApp.getAppTitle();
            
            cfgClient  = ConfigApp.LoadClientSetting();
            cfgServer  = ConfigApp.LoadServerSetting();
            listServer = ConfigApp.LoadListServer();


            cmbMemory.ItemsSource = os.freeMemory();
            //cmbMemory.ite;
            cmbMemoryServer.ItemsSource = os.freeMemory();
            lblIsx64.Content = os.getIsOSx64() + os.getJavaInfo();
            cmbVersionGame.ItemsSource = game.getGameVersions();
            cmbVersionServer.ItemsSource = server.getVersions();


            //SERVER SETTING
            for (int i = 0; i < listServer.Length; i++)
            {
                cmbGameServer.Items.Add(listServer[i]);
            } 

            // CLIENT
            txtUserName.Text = (!String.IsNullOrEmpty(cfgClient.Username)) ? cfgClient.Username : os.getUserName();
            IsForge.IsChecked = cfgClient.IsForge;
            isReMods.IsChecked = cfgClient.IsinstallMods;
            cmbMemory.Text = (!String.IsNullOrEmpty(cfgClient.Memory)) ? cfgClient.Memory.Trim() : "";
            cmbVersionGame.Text = (!String.IsNullOrEmpty(cfgClient.VersionGame)) ? cfgClient.VersionGame.Trim() : "";
            //txtHostServerMap.Text = cfgClient.HostServer;
            cmbGameServer.Text = cfgClient.HostServer;
            txtPortGameServer.Text = cfgClient.PortGameServer;
            txtPortMapServer.Text = cfgClient.PortMapServer;

            //SERVER
            cmbMemoryServer.Text = (!String.IsNullOrEmpty(cfgServer.Memory)) ? cfgServer.Memory.Trim() : "";
            cmbVersionServer.Text = (!String.IsNullOrEmpty(cfgServer.Version)) ? cfgServer.Version.Trim() : "";
            isGUI.IsChecked = cfgServer.IsGUI;
 

            // Отправить данные на сервер
            string postData = String.Format("login={0}&memory={1}&version={2}&is_forge={3}&version_launcher={4}",
                cfgClient.Username, cfgClient.Memory, cfgClient.VersionGame, cfgClient.IsForge, ConfigApp.getVersion());

            new WebClientInfo(serverResponseInfo).send(postData);
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(cfgClient.VersionGame))
                    return;

                var java = os.selectJava(cmbMemory.Text.Trim());
                if (game.run(cfgClient, java))
                {
                    btnRun.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString());
            }
        }
         
        private void btnRunServer_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(cfgServer.Version))
                return;

            var java = os.selectJava(cmbMemory.Text.Trim());
            if (server.run(cfgServer, java)) {
                btnRunServer.IsEnabled = false;
            }
        }

        private void cmbVersionGame_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cfgClient.VersionGame = cmbVersionGame.SelectedItem.ToString();
        }

        private void cmbMemory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cfgClient.Memory = cmbMemory.SelectedItem.ToString();
        }
        
        private void IsForge_Click(object sender, RoutedEventArgs e)
        {
            cfgClient.IsForge = IsForge.IsChecked.Value;
        }

        private void isReMods_Click(object sender, RoutedEventArgs e)
        {
            cfgClient.IsinstallMods = isReMods.IsChecked.Value;
        }

        private void isRift_Click(object sender, RoutedEventArgs e)
        {
            cfgClient.IsRift = isRift.IsChecked.Value;
        }

        private void cmbMemoryServer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cfgServer.Memory = cmbMemoryServer.SelectedItem.ToString();
        }

        private void cmbVersionServer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cfgServer.Version = cmbVersionServer.SelectedItem.ToString();
        }

        private void cmbGameServer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cfgClient.HostServer = cmbGameServer.SelectedItem.ToString();
        }

        private void txtUserName_KeyUp(object sender, KeyEventArgs e)
        {
            cfgClient.Username = txtUserName.Text.Trim();
        }

        private void txtPortGameServer_KeyUp(object sender, KeyEventArgs e)
        {
            cfgClient.PortGameServer = txtPortGameServer.Text.Trim();
        }

        private void txtPortMapServer_KeyUp(object sender, KeyEventArgs e)
        {
            cfgClient.PortMapServer = txtPortMapServer.Text.Trim();
        }


        private void frmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ConfigApp.SaveClientSetting(cfgClient);
            ConfigApp.SaveServerSetting(cfgServer);
        }

        private void isGUI_Click(object sender, RoutedEventArgs e)
        {
            cfgServer.IsGUI = isGUI.IsChecked.Value;
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            server.openServer(cmbVersionServer.Text.Trim());
        }

        private void btnConfigServer_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(cfgServer.Version))
                return;

            frmConfigServer frmConfigServer = new frmConfigServer(cfgServer);
            frmConfigServer.ShowDialog();
        }

        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(cfgServer.Version))
                return;

            frmBackup frmBackup = new frmBackup(cfgServer);
            frmBackup.ShowDialog();
        }

        private void interfaceServerDisabled()
        {
            this.Dispatcher.Invoke((Action)delegate
            {
                cmbVersionServer.IsEnabled = true;
                cmbMemoryServer.IsEnabled = true;
                isGUI.IsEnabled = true;
                btnBackup.IsEnabled = true;
                btnConfigServer.IsEnabled = true;
                btnRunServer.IsEnabled = true;
            });
        }

        private void interfaceMinecraftDisabled()
        {
            this.Dispatcher.Invoke((Action)delegate
            {
                txtUserName.IsEnabled = true;
                cmbMemory.IsEnabled = true;
                cmbVersionGame.IsEnabled = true;
                IsForge.IsEnabled = true;
                isRift.IsEnabled = true;
                btnRun.IsEnabled = true;
                isReMods.IsEnabled = true;
            });
        }

        private void serverRunning(Process server) {
            this.Dispatcher.Invoke((Action)delegate
            {
                cmbVersionServer.IsEnabled = false;
                cmbMemoryServer.IsEnabled = false;
                isGUI.IsEnabled = false;
                btnBackup.IsEnabled = false;
                btnConfigServer.IsEnabled = false;
                btnRunServer.IsEnabled = false;
            });
        }

        private void minicraftRunning(Process minecraft) {
            this.Dispatcher.Invoke((Action)delegate
            {
                txtUserName.IsEnabled = false;
                cmbMemory.IsEnabled = false;
                cmbVersionGame.IsEnabled = false;
                IsForge.IsEnabled = false;
                isRift.IsEnabled = false;
                btnRun.IsEnabled = false;
                isReMods.IsEnabled = false;
            });
        }

        private void btnHttpRun_Click(object sender, RoutedEventArgs e)
        {  
            webServer.run();
            btnHttpRun.IsEnabled = false;
        }   

        private void serverResponseInfo(string response)
        {
            MessageBox.Show(response, "Данные получены сервером!");
        }

        private void clientInfoAdd(ClientInfo client)
        {
            this.Dispatcher.Invoke((Action) delegate {
                listClients.Items.Add(client);
            });               
        }

        private void btnCommand_Click(object sender, RoutedEventArgs e)
        {
            new frmCommand().Show();
        }

        private void HyperlinkSkins_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void urlServerMap_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(cfgClient.HostServer) && !string.IsNullOrEmpty(cfgClient.PortMapServer))
                {
                    Uri baseUri = new Uri(String.Format("http://{0}:{1}", cfgClient.HostServer, cfgClient.PortMapServer));

                    Process.Start(new ProcessStartInfo(baseUri.AbsoluteUri));
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }            
        }

        private void btnAddGameServer_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtGameServer.Text.Trim())) {

                cmbGameServer.Items.Add(txtGameServer.Text.Trim());

                ConfigApp.SaveListServer(txtGameServer.Text.Trim());

                txtGameServer.Text = null; 
            }
        }

        private void btnRemoveGameServer_Click(object sender, RoutedEventArgs e)
        {
            if (cmbGameServer.SelectedItem != null)
            {
                string rt = cmbGameServer.SelectedItem.ToString();
                cmbGameServer.Items.Remove(cmbGameServer.SelectedItem);

                string[] array = cmbGameServer.Items.Cast<string>().ToArray();
                ConfigApp.ReSaveListServer(array);
                
            }
            
        }

    }
}
