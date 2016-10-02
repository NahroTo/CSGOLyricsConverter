using System;
using System.Collections.Generic;
using System.Linq;
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
using System.IO;
using Microsoft.Win32;

namespace CSGOLyricsConverter
{
    public partial class MainWindow : Window
    {
        private const string defaultCfgPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Counter-Strike Global Offensive\\csgo\\cfg\\lyrics.cfg";
        private string cfgPath;
        private string bindKey;
        private string filePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void browseFile_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.txt)|*.txt";
            if (ofd.ShowDialog() == true)
            {
                filePath = ofd.FileName;
                fileInput.Text = filePath;
            }
        }
        public void convertFile_click(object sender, RoutedEventArgs e)
        {
            try
            {
                StreamReader sr = new StreamReader(filePath);
                int lineCount = File.ReadAllLines(filePath).Length;
                bindKey = bindKeyBox.Text;
                cfgPath = cfgPathBox.Text;
                string convertedText = "alias nextLine \"l0\"";
                convertedText += "\r\nbind " + bindKey + " \"nextLine\"";
                while (sr.Peek() > 0)
                {
                    for (int i = 0; i < lineCount; i++)
                    {
                        convertedText += String.Format("\r\nalias l{0} \"say {1};alias nextLine l{2}\"", i, sr.ReadLine(), i + 1);
                    }
                }
                convertedText += "\r\nexec slam\r\nla\r\n1";
                File.WriteAllText(cfgPath, convertedText);
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, "Select a file please.", "CSGOLyricsConverter", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void defaultCfgPath_click(object sender, RoutedEventArgs e)
        {
            cfgPathBox.Text = defaultCfgPath;
        }
    }
}
