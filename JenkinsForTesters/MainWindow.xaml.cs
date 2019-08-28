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
        /// <summary>
        ///     Configuration file location.
        /// </summary>
        private static readonly string _filepath = "Config/Config.json";

        private static Account _account;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Get information about about available jobs from Jenkins server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitButton_Clicked(object sender, RoutedEventArgs e)
        {
            Config config = GetConfig();

            string adress = config.IPAdress;
            int port = config.Port;
            string url = $"http://{adress}:{port}/api/json?tree=jobs[name,url,color]&pretty=true";
            string login = config.Login;
            string apiToken = config.Token;

            WebClient client = new WebClient();

            try
            {
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(login + ":" + apiToken));
                client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
                string result = client.DownloadString(url);
                _account = JsonConvert.DeserializeObject<Account>(client.DownloadString(url));
                myComboBox.ItemsSource = _account.Jobs;

                MessageBox.Show("DONE");
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

        /// <summary>
        ///     Show configuration interface
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ConfigWindow();
            window.Show();
        }

        /// <summary>
        ///     Get configuration
        /// </summary>
        /// <returns> configuration information </returns>
        private Config GetConfig()
        {
            Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(_filepath));

            return config;
        }

        /// <summary>
        ///      Build failing jobs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BuildButton_Click(object sender, RoutedEventArgs e)
        {
            Config config = GetConfig();

            string login = config.Login;
            string apiToken = config.Token;

            Uri uri;
            WebClient client = new WebClient();

            try
            {
                List<Job> failingJobs = _account.Jobs.FindAll(x => x.Color == "red");

                foreach (Job job in failingJobs)
                {
                    uri = new Uri($"{job.Url}/build");

                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(login + ":" + apiToken));
                    client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
                    client.UploadData(uri, "POST", Encoding.ASCII.GetBytes($"?token={apiToken}"));

                    await Task.Delay(5000); //Preemptive Rate limit (more or less)
                }

                MessageBox.Show("DONE");
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
    }
}
