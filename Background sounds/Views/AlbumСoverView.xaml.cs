using Background_sounds.Models;
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

namespace Background_sounds.Views
{
    /// <summary>
    /// Логика взаимодействия для AlbumСoverView.xaml
    /// </summary>
   
    public partial class AlbumСoverView : UserControl
    {
        public Album Album
        {
            get { return (Album)GetValue(AlbumProperty); }
            set { SetValue(AlbumProperty, value); }
        }
        public AlbumСoverView(Album album)
        {
            Album = album;
            InitializeComponent();
        }

        public event RoutedEventHandler Delete;

        public static readonly DependencyProperty AlbumProperty =
    DependencyProperty.Register("Album", typeof(Album), typeof(AlbumСoverView), new PropertyMetadata(null, null));

        private void BDelete_Click(object sender, RoutedEventArgs e)
        {
            if(Message.Show("Действительно удалить альбом?", "Удаление", true) == false)return;
            Settings.Current.Albums.Remove(Album);
            Delete?.Invoke(this, e);
        }
    }
}
