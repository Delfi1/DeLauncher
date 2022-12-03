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
        string version = "0.0.1";

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
            string get_ver = DownloadStr("");
            VersionServer.Content = "Server version: " + get_ver;
            if (version.Contains(get_ver))
            {

            }
        }

        void InitializeUpdater(){
            Check_update();
            Version.Content = "Version: " + version;

        }

        public Updater()
        {
            InitializeComponent();
            InitializeUpdater();
            
        }
    }
}
