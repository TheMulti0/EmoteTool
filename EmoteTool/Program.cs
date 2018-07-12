using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EmoteTool
{
    public class Program
    {
        private static string _settingsFolderPath;
        public static string SettingsPath { get; set; }
        public static AppSettings Settings { get; set; }

        [STAThread]
        public static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;

            SetPath();
            ReadSettings();

            App.Main();
        }

        private static void SetPath()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _settingsFolderPath = Path.Combine(appData, @"EmoteTool\");
            SettingsPath = Path.Combine(_settingsFolderPath, "settings.json");

            if (Directory.Exists(_settingsFolderPath))
            {
                return;
            }
            Directory.CreateDirectory(_settingsFolderPath);
        }

        private static void ReadSettings()
        {
            Settings = new AppSettings();
            try
            {
                string json = File.ReadAllText(SettingsPath);
                var jsonObject = (JObject) JsonConvert.DeserializeObject(json);

                Settings.SavedEmotes = JsonConvert.DeserializeObject<List<string>>(jsonObject["SavedEmotes"].ToString());
            }
            catch
            {
                // ignored
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