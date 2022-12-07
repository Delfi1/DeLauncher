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
        // Установка исходного кода с сайта:
        
        public void SetupUpdate(){
            File.Delete(gamePath + "\\Test1.pck");
            DownloadFile
                (@"https://github.com/Delfi1/Godot_Test/blob/master/Export/Test1.pck?raw=true",
                gamePath + "\\Test1.pck");
        }

        void InitializeWorld(){
            Directory.CreateDirectory(gamePath);
            Updater updater = new Updater();
            this.Title = "De:World";
            Version.Content = updater.Version.Content;
        }

        public MainWindow()
        {
            InitializeComponent();
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            InitializeWorld();
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            Updater updater = new Updater();
            updater.Show();
        }

        private async void Game_Btn_Click(object sender, RoutedEventArgs e)
        {
            Game_Btn.IsEnabled = false;
            if (!File.Exists(gamePath + "\\Test1.exe")) {
                DownloadFile
                    (@"https://github.com/Delfi1/Godot_Test/blob/master/Export/Test1.exe?raw=true",
                    gamePath + "\\Test1.exe");
                await Task.Delay(100);
                SetupUpdate();
                await Task.Delay(100);
            }
            System.Diagnostics.Process.Start(gamePath + "\\Test1.exe");
            await Task.Delay(100);
            Game_Btn.IsEnabled = true;
        }

        private async void Update_GameBtn_Click(object sender, RoutedEventArgs e){
            Update_GameBtn.IsEnabled = false;
            SetupUpdate();
            await Task.Delay(1000);
            Update_GameBtn.IsEnabled = true;
        }

    }
}
