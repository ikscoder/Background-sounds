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
    /// Логика взаимодействия для AlbumView.xaml
    /// </summary>
    public partial class AlbumView : UserControl
    {
        public Album Album
        {
            get { return (Album)GetValue(AlbumProperty); }
            set { SetValue(AlbumProperty, value); }
        }
        public AlbumView(Album album)
        {
            InitializeComponent();
            Album = album;
        }

        private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var av = (sender as AlbumView);
            if (av == null) return;
            av.Body?.Children.Clear();
            foreach (var sv in av.Album.Sounds)
                av.Body?.Children.Add(new SoundView(sv) { Margin = new Thickness(4) });
        }

        public event RoutedEventHandler Click;

        private void BBack_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }

        public static readonly DependencyProperty AlbumProperty =
    DependencyProperty.Register("Album", typeof(Album), typeof(AlbumView), new PropertyMetadata(null, OnPropertyChanged));
    }

    public class SoundToViewConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value is Sound)
                return new SoundView(value as Sound);
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
