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
        string version = "0.1.9";

        string fullPath = Environment.CurrentDirectory;

        // Новый webClient
        WebClient client = new WebClient();

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
            Uri requestUri = new Uri(requestString);
            string str = client.DownloadString(requestUri);
            return str;
        }

        void Load_Log(){
            Log.Text = DownloadStr(@"https://raw.githubusercontent.com/Delfi1/DeLauncher/master/log.txt");
        }

        //Проверка обновления:
        void Check_update()
        {
            string get_ver = DownloadStr(@"https://raw.githubusercontent.com/Delfi1/DeLauncher/master/version.txt");
            VersionServer.Content = "Server version: " + get_ver;
            if (version.Contains(get_ver)) {UpdateBtn.IsEnabled = false;}
            else{ UpdateBtn.IsEnabled = true; }
        }

        async void Setup_Update(){
            System.Diagnostics.Process.Start(fullPath + "\\Updater.exe");
            await Task.Delay(100);
            await Task.Delay(500);
            Environment.Exit(0);
        }

        async void InitializeUpdater(){
            Version.Content = "Version: " + version;
            await Task.Delay(200);
            //Load_Log();
            //await Task.Delay(10);
            //Check_update();
        }

        public Updater()
        {
            InitializeComponent();
            InitializeUpdater();
            
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            Setup_Update();
            UpdateBtn.IsEnabled = false;
            
        }

        async private void CheckBtn_Click(object sender, RoutedEventArgs e)
        {
            Check_update();
            Load_Log();
            CheckBtn.IsEnabled = false;
            await Task.Delay(1000);
            CheckBtn.IsEnabled = true;
        }
    }
}
