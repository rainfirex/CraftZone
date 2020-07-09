using LauncherCraftZone.Modules;
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
using System.Windows.Shapes;
using LauncherCraftZone.Models;

namespace LauncherCraftZone
{
    /// <summary>
    /// Interaction logic for frmConfigServer.xaml
    /// </summary>
    public partial class frmConfigServer : Window
    {
        ConfigServer cfg;
        Server server = new Server();

        public frmConfigServer(ConfigServer cfg)
        {
            InitializeComponent();
            this.cfg = cfg;
        }

        private void frmConfigServer1_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = "Настройка";
            
            server.getConfig(cfg);


            //TextRange range = new TextRange(serverConfig.Document.ContentStart, serverConfig.Document.ContentEnd);
            //range.Text = server.getConfig(cfg);
            serverConfig.Document.Blocks.Clear();
            serverConfig.Selection.Text = server.getConfig(cfg);
        }

        private void serverConfig_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnUpdateConfig_Click(object sender, RoutedEventArgs e)
        {
            //string content = serverConfig.Selection.Text.Trim();

            TextRange range = new TextRange(serverConfig.Document.ContentStart, serverConfig.Document.ContentEnd);
            server.saveConfig(range.Text); 
            this.Close();
        }

        
    }
}
