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
using System.Windows.Threading;

namespace Background_sounds.Views
{
    /// <summary>
    /// Логика взаимодействия для SoundView.xaml
    /// </summary>
    public partial class SoundView : UserControl
    {
        public Sound Sound
        {
            get { return (Sound)GetValue(SoundProperty); }
            set { SetValue(SoundProperty, value); }
        }
        public SoundView() { }
        public SoundView(Sound sound)
        {
            Sound = sound;
            InitializeComponent();
            Media.MediaEnded += (sender, e) => { Media.Position = new TimeSpan(0); };
            Media.Play();
            Media.MediaOpened+=(s,e)=>PositionSlider.Maximum = Media.NaturalDuration.TimeSpan.TotalMilliseconds;
            var timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += new EventHandler(Tick);
            timer.Start();
        }

        public static readonly DependencyProperty SoundProperty =
DependencyProperty.Register("Sound", typeof(Sound), typeof(SoundView), new PropertyMetadata(null, null));

        private void BPlay_Click(object sender, RoutedEventArgs e)
        {
            Media.Play();
        }

        private void Expanded(object sender, RoutedEventArgs e)
        {
            Height += 30;
        }

        private void Collapsed(object sender, RoutedEventArgs e)
        {
            Height -= 30;
        }

        private void BPause_Click(object sender, RoutedEventArgs e)
        {
            Media.Pause();
        }

        private void BStop_Click(object sender, RoutedEventArgs e)
        {
            Media.Stop();
        }

        void Tick(object sender, EventArgs e)
        {
            PositionSlider.Value = Media.Position.TotalMilliseconds;
        }

        private void PositionSlider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Media.Position = TimeSpan.FromMilliseconds(PositionSlider.Value).Duration();
        }
    }

    public class DoubleToPercentConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            double.TryParse(value.ToString(), out double d);
            return (d * 100).ToString("F2") + " %";
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
