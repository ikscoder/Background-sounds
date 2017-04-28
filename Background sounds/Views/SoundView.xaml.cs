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
        }

        public static readonly DependencyProperty SoundProperty =
DependencyProperty.Register("Sound", typeof(Sound), typeof(SoundView), new PropertyMetadata(null, null));
    }
}
