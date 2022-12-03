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
using System.Windows.Shapes;

namespace DeWorld
{
    #pragma warning disable SYSLIB0014
    /// <summary>
    /// Логика взаимодействия для Updater.xaml
    /// </summary>
    public partial class Updater : Window
    {
        // Переменные:
        string version = "0.1.0";
        string fullPath = Environment.CurrentDirectory;

        // Установка файла с сайта:
        public void DownloadFile(string requestString, string path)
        {
            HttpClient httpClient = new HttpClient();
            var GetTask = httpClient.GetAsync(requestString);
            GetTask.Wait(1000);
            if (!GetTask.Result.IsSuccessStatusCode) { return; }
            using (var fs = new FileStream(path, FileMode.CreateNew))
            {
                var ResponseTask = GetTask.Result.Content.CopyToAsync(fs);
                ResponseTask.Wait(1000);
            }
            System.Threading.Thread.Sleep(200);

        }
        // Установка исходного кода с сайта:
        public string DownloadStr(string requestString)
        {
            // Новый webClient
            WebClient client = new WebClient();
            Uri requestUri = new Uri(requestString);
            string str = client.DownloadString(requestUri);
            return str;
        }

        //Проверка обновления:
        void Check_update()
        {
            string get_ver = DownloadStr("https://raw.githubusercontent.com/Delfi1/DeLauncher/master/version.txt");
            VersionServer.Content = "Server version: " + get_ver;
            if (version.Contains(get_ver)) {UpdateBtn.IsEnabled = false;}
            else{ UpdateBtn.IsEnabled = true; }
        }

        async void Setup_Update(){
            await Task.Delay(10);
            File.Move(fullPath + "\\DeWorld.exe", fullPath + "\\DeWorld_old.exe");
            await Task.Delay(10);
            DownloadFile(@"https://github.com/Delfi1/DeLauncher/blob/master/bin/Release/net6.0-windows/DeWorld.exe?raw=true", fullPath + "\\DeWorld.exe");
            DownloadFile(@"https://github.com/Delfi1/DeLauncher/blob/master/bin/Release/net6.0-windows/DeWorld.dll?raw=true", fullPath + "\\DeWorld.exe");
            DownloadFile(@"https://github.com/Delfi1/DeLauncher/blob/master/bin/Release/net6.0-windows/DeWorld.pdb?raw=true", fullPath + "\\DeWorld.exe");
            await Task.Delay(1000);
            System.Diagnostics.Process.Start(fullPath + "\\DeWorld.exe");
            await Task.Delay(5000);
            Environment.Exit(0);
        }

        async void InitializeUpdater(){
            Version.Content = "Version: " + version;
            await Task.Delay(200);
            Check_update();
            Version.Content = "Version: " + version;

        }

        public Updater()
        {
            InitializeComponent();
            InitializeUpdater();
            
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            Setup_Update();
        }

        async private void CheckBtn_Click(object sender, RoutedEventArgs e)
        {
            Check_update();
            CheckBtn.IsEnabled = false;
            await Task.Delay(1000);
            CheckBtn.IsEnabled = true;
        }
    }
}
