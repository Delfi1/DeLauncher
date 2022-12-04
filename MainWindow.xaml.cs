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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Установка файла с сайта:
        public void DownloadFile(string requestString, string path){
            HttpClient httpClient = new HttpClient();
            var GetTask = httpClient.GetAsync(requestString);
            GetTask.Wait(1000);
            if (!GetTask.Result.IsSuccessStatusCode){return;}
            using (var fs = new FileStream(path, FileMode.CreateNew)){
                var ResponseTask = GetTask.Result.Content.CopyToAsync(fs);
                ResponseTask.Wait(1000);
            }
            System.Threading.Thread.Sleep(200);

        }
        // Установка исходного кода с сайта:
        public string DownloadStr(string requestString){
            // Новый webClient
            WebClient client = new WebClient();
            Uri requestUri = new Uri(requestString);
            string str = client.DownloadString(requestUri);
            return str;
        }

        void InitializeWorld(){
            //Updater updater = new Updater();
            this.Title = "De:World";
            //Version.Content = updater.Version.Content;
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

        private void Game_Btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
