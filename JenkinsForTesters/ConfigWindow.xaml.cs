using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using JenkinsForTesters.Core;

namespace JenkinsForTesters
{
    /// <summary>
    /// Logika interakcji dla klasy ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        private static readonly Config _config = new Config();
        private static readonly string _filepath = "Config/Config.json";

        public ConfigWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _config.IPAdress = adress.Text;
            _config.Port = Int32.Parse(port.Text);
            _config.Login = login.Text;
            _config.Password = pass.Text;
            _config.Token = token.Text;

            string json = JsonConvert.SerializeObject(_config);
            File.WriteAllText(_filepath, json);

            this.Close();
        }
    }
}
