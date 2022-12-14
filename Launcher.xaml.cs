using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.Net.Http;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DeWorld.Properties;
using System.IO;

namespace DeWorld
{
    /// <summary>
    /// Логика взаимодействия для Launcher.xaml
    /// </summary>
    
    public partial class Launcher : Window
    {
        WebClient client = new WebClient();

        string gamePath = Environment.CurrentDirectory + "\\Game";
        string ver = Settings.Default.SVer1;
        string get_ver = "";
        Uri GameVerUri = new Uri("https://raw.githubusercontent.com/Delfi1/Test2/Develop/Export/game.txt");
        Uri GameUri = new Uri("https://github.com/Delfi1/Test2/blob/Develop/Export/Test2.exe?raw=true");
        Uri GamePckUri = new Uri("https://github.com/Delfi1/Test2/blob/Develop/Export/Test2.pck?raw=true");

        public async void DownloadFile(Uri requestUri, string path)
        {
            HttpClient httpClient = new HttpClient();
            var GetTask = httpClient.GetAsync(requestUri);
            GetTask.Wait(1500);
            if (!GetTask.Result.IsSuccessStatusCode) { return; }
            using (var fs = new FileStream(path, FileMode.CreateNew))
            {
                var ResponseTask = GetTask.Result.Content.CopyToAsync(fs);
                ResponseTask.Wait(500);
            }
            await Task.Delay(1);
        }

        async void Check()
        {
            while (get_ver.Length < 2){
                CheckBtn.IsEnabled = false;
                await Task.Delay(150);
            }
            CheckBtn.IsEnabled = true;
            StartBtn.IsEnabled = true;
        }

        private void CheckUpdate(){
            if (get_ver.Contains(ver)){ UpdateGame.IsEnabled = false;
                ReloadGame.IsEnabled = true; }
            else { UpdateGame.IsEnabled = true; }
            ServerVersion.Content = "Server version: " + get_ver;
        }

        void Loading(Label lb)
        {
            lb.Content = "Loading...";
        }

        async void InUpdater()
        {
            while (true)
            {
                get_ver = await client.DownloadStringTaskAsync(GameVerUri);
                CheckUpdate();
                await Task.Delay(5000);
                Loading(ServerVersion);
                await Task.Delay(150);
            }
        }

        private void SetupUpdate(bool delete){
            if (!File.Exists(gamePath + "\\Test2.exe"))
            {
                StartBtn.IsEnabled = false;
                DownloadFile(GamePckUri, gamePath + "\\Test2.pck");
                DownloadFile(GameUri, gamePath + "\\Test2.exe");
            }
            else{
                if (delete){
                    File.Delete(gamePath + "\\Test2.exe");
                }
            }
            File.Delete(gamePath + "\\Test2.pck");
            DownloadFile(GamePckUri, gamePath + "\\Test2.pck");
            Settings.Default.SVer1 = get_ver;
            Settings.Default.Save();
            GameVersion.Content = "Game version: " + get_ver;
            UpdateGame.IsEnabled = false;
        }

        void InitializeLauncher(){
            ReloadGame.IsEnabled = false;
            UpdateGame.IsEnabled = false;
            InUpdater();
            Check();
        }

        public Launcher()
        {
            InitializeComponent();
            InitializeLauncher();
        }

        private void CheckBtn_Click(object sender, RoutedEventArgs e)
        {
            CheckUpdate();
        }

        private void UpdateGame_Click(object sender, RoutedEventArgs e){
            SetupUpdate(false);
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e){
            System.Diagnostics.Process.Start(gamePath + "\\Test2.exe");
        }

        private void ReloadGame_Click(object sender, RoutedEventArgs e){
            SetupUpdate(true);
        }
    }
}
