using Background_sounds.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace Background_sounds
{
    public class Settings
    {
        private const string SettingsFileName = "user.settings";
        private static Settings _instance;

        private bool _isDarkTheme;

        public static Settings Current
        {
            get { return _instance ?? (_instance = new Settings()); }
            set { _instance = value; }
        }

        public ObservableCollection<Album> Albums { get; set; } = new ObservableCollection<Album>();
        public bool IsDarkTheme
        {
            get { return _isDarkTheme; }
            set
            {
                if (value == _isDarkTheme) return;
                _isDarkTheme = value;
                if (value)
                {
                    var uri = new Uri("Themes\\Dark.xaml", UriKind.Relative);
                    ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
                    Application.Current.Resources.Clear();
                    Application.Current.Resources.MergedDictionaries.Add(resourceDict);
                }
                else
                {
                    var uri = new Uri("Themes\\Light.xaml", UriKind.Relative);
                    ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
                    Application.Current.Resources.Clear();
                    Application.Current.Resources.MergedDictionaries.Add(resourceDict);
                }
            }
        }
        public static void Save()
        {
            try
            {
                var x = new XmlSerializer(Current.GetType());
                using (var stringWriter = new StringWriter())
                {
                    x.Serialize(stringWriter, Current);
                    using (var sw = new StreamWriter(SettingsFileName, false, Encoding.Default))
                    {
                        sw.Write(stringWriter.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Message.Show(e.Message + "\n" + e.InnerException?.Message + "\n" + e.InnerException?.InnerException?.Message,"Ошибка сохраения");
            }
        }
        public static void Load()
        {
            if (!File.Exists(SettingsFileName)) return;
            using (var sr = new StreamReader(SettingsFileName, Encoding.Default))
            {
                try
                {
                    Current = (Settings)new XmlSerializer(Current.GetType()).Deserialize(sr);
                }
                catch (Exception e){ Message.Show(e.Message,"Ошибка загрузки"); }
            }
        }
    }
}
