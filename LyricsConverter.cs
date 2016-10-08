using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace CSGOLyricsConverter
{
    public class LyricsConverter
    {
        private const string defaultCfgPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Counter-Strike Global Offensive\\csgo\\cfg\\lyrics.cfg";
        private const string defaultLyricsCfgPath = "\\steamapps\\common\\Counter-Strike Global Offensive\\csgo\\cfg\\lyrics.cfg";
        private string cfgPath;
        private string bindKey;

        public string filePath;
        public bool IsSteamInstalled = false;

        /// <summary>
        /// Loads Steam installation directory from machines registry, and creates path from it.
        /// </summary>
        /// <returns>Boolean, based on if Steam is installed or not.</returns>
        public bool LoadSteamPath()
        {
            string registryPath;

            registryPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam", "InstallPath", null);
            if (registryPath != null)
            {
                cfgPath = registryPath + defaultLyricsCfgPath;
                IsSteamInstalled = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Browses lyrics file throught <seealso cref="OpenFileDialog"/> and sets path from it.
        /// </summary>
        /// <returns>Boolean, based on if lyrics file browsing was completed or not.</returns>
        public bool BrowseLyricsFile()
        {
            if (!IsSteamInstalled)
                return false;

            var dialog = new OpenFileDialog();
            dialog.Filter = "Text files (*.txt)|*.txt";

            if (dialog.ShowDialog() == true)
            {
                filePath = dialog.FileName;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Converts given file to CS:GO config file with keybind for each line of text.
        /// </summary>
        /// <param name="key">Given CS:GO key to bind lyrics for.</param>
        public void ConvertToLyricsFile(string key)
        {
            try
            {
                using (var streamReader = new StreamReader(filePath))
                {
                    bindKey = key;

                    var lineCount = File.ReadAllLines(filePath).Length;
                    var convertedText = "alias nextLine \"l0\"";
                    convertedText += "\r\nbind " + bindKey + " \"nextLine\"";

                    while (streamReader.Peek() > 0)
                        for (var i = 0; i < lineCount; i++)
                            convertedText += string.Format("\r\nalias l{0} \"say {1};alias nextLine l{2}\"", i, streamReader.ReadLine(), i + 1);

                    convertedText += "\r\nexec slam\r\nla\r\n1";
                    File.WriteAllText(cfgPath, convertedText);
                }

                MessageBox.Show("File succesfully converted!", "CSGOLyricsConverter", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Select a file please.", "CSGOLyricsConverter", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public string GetDefaultConfigPath()
        {
            if (cfgPath != null)
                return cfgPath;


            return defaultCfgPath;
        }
    }
}
