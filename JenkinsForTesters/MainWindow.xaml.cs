using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using JenkinsForTesters.Core;

namespace JenkinsForTesters
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string _filepath = "Config/Config.json";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SubmitButton_Clicked(object sender, RoutedEventArgs e)
        {
            Config config = GetConfig();

            string adress = config.IPAdress;
            int port = config.Port;
            string url = $"http://{adress}:{port}/api/json?tree=jobs[name,url,color]&pretty=true";
            string login = config.Login;
            string password = config.Password;

            WebClient client = new WebClient();

            try
            {
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(login + ":" + password));
                client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
                string result = client.DownloadString(url);
                Account json = JsonConvert.DeserializeObject<Account>(client.DownloadString(url));
                myComboBox.ItemsSource = json.Jobs;
                MessageBox.Show("DONE\n\n" + result);

                //BuildUnbuildedJobs(json, login, password, config.Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something goes wrong...\n{ex.Message}");
            }
            finally
            {
                client.Dispose();
            }
        }

        private Task BuildUnbuildedJobs(Account account, string login, string password, string token)
        {
            //DOES NOT WORK
            WebClient client = new WebClient();
            Uri uri = new Uri($"{account.Jobs.FirstOrDefault(x => x.Color == "red").Url}/build");

            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(login + ":" + password));
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.UploadData(uri, "POST", Encoding.ASCII.GetBytes($"?token={token}"));
            return Task.CompletedTask;
        }

        private void ConfigButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ConfigWindow();
            window.Show();
        }

        private Config GetConfig()
        {
            //problem
            Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(_filepath));

            return config;
        }
    }
}
