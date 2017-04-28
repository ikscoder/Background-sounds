using Background_sounds.Models;
using Background_sounds.Views;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace Background_sounds
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PopSettings.Closed += (s, e) => { BodyGrid.Effect = null; };
            var TrayMenu = new ContextMenu
            {
                Padding = new Thickness(0),
            };
            var mi = new MenuItem
            {
                Header = "Выход",
                Icon = new Path
                {
                    Data = (Geometry)Application.Current.Resources["Exit"],
                    Fill = (SolidColorBrush)Application.Current.Resources["SecondaryColor"],
                    Stretch = Stretch.Uniform
                }

            };
            mi.Click += BExit_Click;
            TrayMenu.Items.Add(mi);
            //DirectoryInfo dir = new DirectoryInfo(@"Sounds");
            //int d = 0;
            //foreach (var directory in dir.GetDirectories())
            //{
            //    List<Sound> sounds = new List<Sound>();
            //    var subdir= new DirectoryInfo(@"Sounds\" + directory.Name);
            //    foreach (var file in subdir.GetFiles("*.mp3"))
            //    {

            //        sounds.Add(new Sound(directory.Name + "." + file.Name.Replace(".mp3", ""), new Uri(@"Sounds\" + directory.Name + '\\' + file.Name, UriKind.Relative)) { Volume=0});
            //    }
            //    d++;
            //    Settings.Current.Albums.Add(new Album(directory.Name, sounds, new Uri(@"pack://siteoforigin:,,,/Images/img" + d+".jpg", UriKind.Absolute)));               
            //}
            Settings.Load();
            Settings.Current.Albums.CollectionChanged += AlbumsChanged;
            Settings.Current.Albums=new System.Collections.ObjectModel.ObservableCollection<Album>(Settings.Current.Albums.OrderBy(x => x.Name));
            AlbumsChanged(null,null);

            var ni = new System.Windows.Forms.NotifyIcon()
            {
                Icon = Properties.Resources.BS,
                Visible = true,
                Text="Background Sounds"
            };
            ni.Click += (s, e) => {
                if ((e as System.Windows.Forms.MouseEventArgs)?.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    Show();
                    WindowState = WindowState.Normal;
                }
                else
                {
                    TrayMenu.IsOpen = true;
                    Activate();
                }

            };
        }

        private void BExit_Click(object sender, RoutedEventArgs e)
        {
            Settings.Save();
            Application.Current.Shutdown();
        }

        private void BMaximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void BMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BSettings_Click(object sender, RoutedEventArgs e)
        {
            BodyGrid.Effect = new BlurEffect();
            PopSettings.IsOpen = true;
        }

        public void Drag(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Opacity = 0.5;
                DragMove();
                Opacity = 1;
            }
            //if (e.ClickCount == 2 && e.ChangedButton == MouseButton.Left)
            //{
            //    WindowState = WindowState != WindowState.Minimized ? WindowState.Minimized : WindowState.Normal;
            //}
        }

        public void SetAlbum(object sender, MouseButtonEventArgs e)
        {
            var acv = (sender as AlbumСoverView);
            if (acv == null) return;
            Albums.Visibility = Visibility.Collapsed;
            LAlbum.Visibility = Visibility.Visible;
            var albumView = new AlbumView(acv.Album);
            albumView.Click += BackFromAlbum;
            LAlbum.Content = albumView;           
        }

        public void SetNewAlbum(object sender, MouseButtonEventArgs e)
        {
            Albums.Visibility = Visibility.Collapsed;
            LAlbum.Visibility = Visibility.Visible;
            var albumView = new AlbumCreateView();
            albumView.Click += BackFromAlbum;
            LAlbum.Content = albumView;
        }

        private void BackFromAlbum(object sender, RoutedEventArgs e)
        {
            Albums.Visibility = Visibility.Visible;
            LAlbum.Visibility = Visibility.Collapsed;
            LAlbum.Content = null;
            AlbumsChanged(null,null);
        }

        private void AlbumsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Albums.Children.Clear();
            foreach (var album in Settings.Current.Albums)
            {
                var albumСoverView = new AlbumСoverView(album) { Width = 200, Height = 200 };
                albumСoverView.MouseDown += SetAlbum;
                albumСoverView.Delete += (s, ee) => { AlbumsChanged(null, null); };
                Albums.Children.Add(albumСoverView);
            }
            var nacv = new NewAlbumCoverView { Width = 200, Height = 200 };
            nacv.MouseDown += SetNewAlbum;
            Albums.Children.Add(nacv);
        }

        private void BAlwaysOnTop_Click(object sender, RoutedEventArgs e)
        {
            BAlwaysOnTop.RenderTransform = Topmost ? null : new RotateTransform { Angle = -45 };
            Topmost = !Topmost;           
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }
    }
}
