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
        Uri GameUri = new Uri("");

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

        void Check()
        {
            while(get_ver.Length < 2){
                CheckBtn.IsEnabled = false;
            }
            CheckBtn.IsEnabled = true;
        }

        private async void CheckUpdate(){
            if (get_ver.Contains(ver)){ UpdateGame.IsEnabled = false; }
            else { UpdateGame.IsEnabled = true; }
            ServerVersion.Content = "Server version:" + get_ver;
        }

        async void InUpdater()
        {
            while (true)
            {
                get_ver = await client.DownloadStringTaskAsync(GameUri);
                await Task.Delay(5000);
                
            }
        }

        private void SetupUpdate(){
            Settings.Default.SVer1 = get_ver;
            Settings.Default.Save();
        }

        void InitializeLauncher(){
            UpdateGame.IsEnabled = false;
            Check();
            InUpdater();
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
    }
}
