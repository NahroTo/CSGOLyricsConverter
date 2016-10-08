using System.Windows;

namespace CSGOLyricsConverter
{
    public partial class MainWindow : Window
    {
        private LyricsConverter converter = new LyricsConverter();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (converter.LoadSteamPath())
            {
                cfgPathBox.Text = converter.GetDefaultConfigPath();
                convertFileBtn.IsEnabled = true;
                return;
            }

            browseFileBtn.IsEnabled = false;
            convertFileBtn.IsEnabled = false;
            MessageBox.Show("Steam is not installed on this machine.", "CSGOLyricsConverter", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void browseFile_click(object sender, RoutedEventArgs e)
        {
            if (converter.BrowseLyricsFile())
            {
                fileInput.Text = converter.filePath;
                return;
            }

            MessageBox.Show("Lyrics file path not found!", "CSGOLyricsConverter", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void convertFile_click(object sender, RoutedEventArgs e)
        {
            if (converter.IsSteamInstalled)
                converter.ConvertToLyricsFile(bindKeyBox.Text);
        }

        private void defaultCfgPath_click(object sender, RoutedEventArgs e)
        {
            cfgPathBox.Text = converter.GetDefaultConfigPath();
        }
    }
}
