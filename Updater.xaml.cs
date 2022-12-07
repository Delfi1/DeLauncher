using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
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
        string version = "0.2.6";
        string fullPath = Environment.CurrentDirectory;
        string get_ver = "";
        string get_log = "";
        // Новый webClient
        WebClient client = new WebClient();
        Uri UriLog = new Uri("https://raw.githubusercontent.com/Delfi1/DeLauncher/master/log.txt");
        Uri VersionUri = new Uri("https://raw.githubusercontent.com/Delfi1/DeLauncher/master/version.txt");
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
        
        void Load_Log(){
            TextBlock1.Text = get_log;
            ReadMore.IsEnabled = true;
        }

        async void Check(){
            while(get_log.Length < 50){
                CheckBtn.IsEnabled = false;
                await Task.Delay(100);
            }
            CheckBtn.IsEnabled = true;
        }

        //Проверка обновления:
        void Check_update()
        {
            VersionServer.Content = "Server version: " + get_ver;
            if (version.Contains(get_ver)) {UpdateBtn.IsEnabled = false;}
            else{ UpdateBtn.IsEnabled = true; }
        }

        async void CheckBtn_func(bool check) {
            if (check) {CheckBtn.IsEnabled = false; }
            Loading(TextBlock1);
            Loading(VersionServer);
            await Task.Delay(250);
            Check_update();
            Load_Log();
            CheckBtn.IsEnabled = true;
        }

        async void Setup_Update(){
            System.Diagnostics.Process.Start(fullPath + "\\Updater.exe");
            await Task.Delay(100);
            Environment.Exit(0);
        }

        async void InUpdater()
        {
            while (true)
            {
                get_ver = await client.DownloadStringTaskAsync(VersionUri);
                get_log = await client.DownloadStringTaskAsync(UriLog);
                await Task.Delay(10000);
                CheckBtn_func(false);
            }
        }


        async void InitializeUpdater(){
            Version.Content = "Version: " + version;
            await Task.Delay(200);
            InUpdater();
            Check();
        }

        void Loading(TextBlock tb){
            ReadMore.IsEnabled = false;
            tb.Text = "Loading...";
        }
        void Loading(Label lb){
            lb.Content = "Loading...";
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

        private void CheckBtn_Click(object sender, RoutedEventArgs e)
        {
            CheckBtn_func(true);
        }

        private void ReadMore_Click(object sender, RoutedEventArgs e)
        {
            Log log = new Log();
            log.TextBox1.Text = TextBlock1.Text;
            log.Show();
        }
    }
}
