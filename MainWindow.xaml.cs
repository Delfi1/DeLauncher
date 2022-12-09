using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
using DeWorld.Properties;

namespace DeWorld
{
#pragma warning disable SYSLIB0014
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Переменные:
        string fullPath = Environment.CurrentDirectory;
        string gamePath = Environment.CurrentDirectory + "\\Game";
        
        private void InitializeGame(string name, string version){
            Launcher launcher = new Launcher();
            
            launcher.GameName.Content = "Game name: " + name;

            launcher.Title = name + " | " + version;

            launcher.GameVersion.Content = "Game version: " + version;
            
            launcher.Show();
        }

        // Установка файла с сайта:
        public async void DownloadFile(string requestString, string path)
        {
            HttpClient httpClient = new HttpClient();
            var GetTask = httpClient.GetAsync(requestString);
            GetTask.Wait(1500);
            if (!GetTask.Result.IsSuccessStatusCode) { return; }
            using (var fs = new FileStream(path, FileMode.CreateNew))
            {
                var ResponseTask = GetTask.Result.Content.CopyToAsync(fs);
                ResponseTask.Wait(500);
            }
            await Task.Delay(1);
        }

        string SaveVer(string name){
            if (File.Exists(fullPath + "\\save" + name + ".txt")){
                StreamReader sr = new StreamReader(fullPath + "\\save" + name + ".txt");
                string get_ver = sr.ReadToEnd();
                return get_ver;
                sr.Close();
                File.Delete(fullPath + "\\save" + name + ".txt");
            }
            return "";
        }

        void InitializeWorld(){
            Directory.CreateDirectory(gamePath);
            Updater updater = new Updater();
            this.Title = "De:World";
            Version.Content = updater.Version.Content;

            if (SaveVer("Test1").Length >= 2){
                Settings.Default.SVer1 = SaveVer("Test1");
                Settings.Default.Save();
            }

        }

        public MainWindow(){
            InitializeComponent();
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            InitializeWorld();
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            Updater updater = new Updater();
            updater.Show();
        }

        private void Game_Btn_Click(object sender, RoutedEventArgs e)
        {
            InitializeGame("Test1", Settings.Default.SVer1);
        }

    }
}
