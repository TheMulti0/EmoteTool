using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
using EmoteTool.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EmoteTool
{
    public class Program
    {
        private static string _settingsFolderPath;
        public static string SettingsPath { get; set; }
        public static string ImagesPath { get; set; }
        public static AppSettings Settings { get; set; }

        [STAThread]
        public static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;

            SetPath();
            ReadSettings();

            ImagesPath = Path.Combine(
                _settingsFolderPath,
                @"Images\");

            App.Main();
        }

        private static void SetPath()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _settingsFolderPath = Path.Combine(appData, @"EmoteTool\");
            SettingsPath = Path.Combine(_settingsFolderPath, "settings.json");

            if (!Directory.Exists(_settingsFolderPath))
            {
                Directory.CreateDirectory(_settingsFolderPath);
            }
        }

        private static void ReadSettings()
        {
            Settings = new AppSettings();
            try
            {
                string json = File.ReadAllText(SettingsPath);
                var jsonObject = (JObject) JsonConvert.DeserializeObject(json);

                Settings.SavedEmotes = JsonConvert.DeserializeObject<List<(int, string, string, string, ItemSizeMode)>>(jsonObject["SavedEmotes"].ToString());
            }
            catch
            {
                Settings.SavedEmotes = new List<(int id, string name, string imagePath, string actualImagePath, ItemSizeMode sizeMode)>();
            }
        }

        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            var assemblyName = new AssemblyName(args.Name);

            string path = assemblyName.Name + ".dll";
            if (assemblyName.CultureInfo.Equals(CultureInfo.InvariantCulture) == false)
            {
                path = $@"{assemblyName.CultureInfo}\{path}";
            }

            using (Stream stream = executingAssembly.GetManifestResourceStream(path))
            {
                if (stream == null)
                {
                    return null;
                }

                byte[] assemblyRawBytes = new byte[stream.Length];
                stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);
                return Assembly.Load(assemblyRawBytes);
            }
        }
    }
}