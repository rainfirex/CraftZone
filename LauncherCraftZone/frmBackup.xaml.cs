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
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;
using LauncherCraftZone.Models;

namespace LauncherCraftZone
{
    /// <summary>
    /// Interaction logic for frmBackup.xaml
    /// </summary>
    public partial class frmBackup : Window
    {
        SyncMap syncMap = new SyncMap();

        ConfigServer cfg;
        DirectoryInfo mapFolder;

        public frmBackup(ConfigServer cfg)
        {
            InitializeComponent();
            this.cfg = cfg;
            txtMap.Text = cfg.MapDir;
            txtDirBackup.Text = cfg.BackupMapDir;
        }

        private void frmBackup1_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = " Резерное копирование карты";

            getMapInfo(cfg.MapDir);
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog openDialog = new FolderBrowserDialog();
            openDialog.SelectedPath = ConfigApp.currentDir + Server.SERVER_DIR;
            DialogResult rs = openDialog.ShowDialog();
            if (rs == System.Windows.Forms.DialogResult.OK)
            {
                cfg.MapDir = openDialog.SelectedPath;
                getMapInfo(openDialog.SelectedPath);
            }

            //OpenFileDialog d = new OpenFileDialog();
            //d.InitialDirectory = ConfigApp.currentDir;
            //if (d.ShowDialog() == true)
            //{
            //    txtMap.Text = d.FileName;
            //}
        }

        private void getMapInfo(string path)
        {
            mapFolder = new DirectoryInfo(path);
            txtMap.Text = cfg.Version + @"\" + mapFolder.Name;

            DateTime creating = mapFolder.CreationTime;
            DateTime accesse = mapFolder.LastAccessTime;
            DateTime write = mapFolder.LastWriteTime;

            txtTimeCreating.Text = creating.ToShortTimeString();
            txtTimeAccess.Text = accesse.ToShortTimeString();
            txtTimeWrite.Text = write.ToShortTimeString();
        }

        private void btnOpenDirBackup_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog openDialog = new FolderBrowserDialog();
            DialogResult rs = openDialog.ShowDialog();
            if (rs == System.Windows.Forms.DialogResult.OK)
            {
                txtDirBackup.Text = openDialog.SelectedPath;
                cfg.BackupMapDir = openDialog.SelectedPath;
            }
        }

        private void btnSync_Click(object sender, RoutedEventArgs e)
        {
            if (syncMap.backup(cfg))
            {
                System.Windows.Forms.MessageBox.Show("Копирование завершено", "Выполнено", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnRecovery_Click(object sender, RoutedEventArgs e)
        {
            syncMap.recovery(cfg);
        }
    }
}
