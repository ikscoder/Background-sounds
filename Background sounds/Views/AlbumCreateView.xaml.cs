using Background_sounds.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace Background_sounds.Views
{
    /// <summary>
    /// Логика взаимодействия для AlbumCreateView.xaml
    /// </summary>
    public partial class AlbumCreateView : UserControl
    {
        public ObservableCollection<Sound> Sounds { get; set; }
        public AlbumCreateView()
        {
            InitializeComponent();
            Album = new Album();
            Sounds = new ObservableCollection<Sound>();
            Album.Sounds = new ObservableCollection<Sound>();
            DirectoryInfo dir = new DirectoryInfo(@"Sounds");
            foreach (var file in dir.GetFiles("*.mp3"))
            {
                Sounds.Add(new Sound(file.Name.Replace(".mp3", ""), new Uri(@"Sounds\" + file.Name, UriKind.Relative)) { Volume = 1 });
            }
            foreach (var directory in dir.GetDirectories())
            {               
                var subdir = new DirectoryInfo(@"Sounds\" + directory.Name);
                foreach (var file in subdir.GetFiles("*.mp3"))
                {
                    Sounds.Add(new Sound(directory.Name + "." + file.Name.Replace(".mp3", ""), new Uri(@"Sounds\" + directory.Name + '\\' + file.Name, UriKind.Relative)) { Volume = 1 });
                }
            }
            AllSounds.ItemsSource = Sounds;
        }
        public Album Album
        {
            get { return (Album)GetValue(AlbumProperty); }
            set { SetValue(AlbumProperty, value); }
        }

        public event RoutedEventHandler Click;

        private void BCreate_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(Album.Name))
            {
                Message.Show("Нужно задать имя!","Не создать=(");
                return;
            }
            if (SelectedSounds.Items.Count==0)
            {
                Message.Show("Нужно выбрать звуки!", "Не создать=(");
                return;
            }
            if (Album.Image==null)
            {
                if(Message.Show("Не выбранна обложка. Да и хрен с ней?", "Не создать?",true)==false)return;
            }
            Settings.Current.Albums.Add(Album);
            Click?.Invoke(this, e);
        }

        private void SelectImage(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Изображения|*.jpeg;*.jpg;*.png;*.ico",
                CheckFileExists = true,
                Multiselect=false
            };
            if (dialog.ShowDialog() != true) return;
            string destPath = @"Images\"+ dialog.FileName.Substring(dialog.FileName.LastIndexOf('\\')+1);
            try
            {
                File.Copy(dialog.FileName, destPath);
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message + "\n" + ex.InnerException?.Message + "\n" + ex.InnerException?.InnerException?.Message,"Ошибка");
            }
            Album.Image = new Uri(@"pack://siteoforigin:,,,/"+destPath.Replace('\\','/'), UriKind.Absolute);
            Image.Source = new BitmapImage(Album.Image);
        }

        private void BBack_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }

        private void Play(object sender, RoutedEventArgs e)
        {
            var sound = (sender as Button)?.Tag as Sound;
            if (sound == null) return;
            Media.Source = sound.Source;
            Media.Play();
        }

        public static readonly DependencyProperty AlbumProperty =
    DependencyProperty.Register("Album", typeof(Album), typeof(AlbumCreateView), new PropertyMetadata(null, null));

        private void AllSounds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = AllSounds.SelectedItem as Sound;
            if (item == null) return;
            Sounds.Remove(item);
            Album.Sounds.Add(item);
            SelectedSounds.ItemsSource = Album.Sounds;
        }

        private void SelectedSounds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = SelectedSounds.SelectedItem as Sound;
            if (item == null) return;
            Album.Sounds.Remove(item);
            Sounds.Add(item);
            SelectedSounds.ItemsSource = Album.Sounds;
        }
    }

    public class TextToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
                return string.IsNullOrEmpty(value.ToString())?Visibility.Visible:Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
