using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace Background_sounds.Models
{
    public class Album
    {
        public string Name { get; set; }
        public ObservableCollection<Sound> Sounds { get; set; }
        [XmlIgnore]
        public Uri Image { get; set; }

        [XmlAttribute("Image")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string UriString
        {
            get { return Image?.ToString(); }
            set { Image = value == null ? null : new Uri(value); }
        }
        public Album() { }

        public Album(string name,IEnumerable<Sound> sounds, Uri image)
        {
            Name = name;
            Sounds = new ObservableCollection<Sound> (sounds);
            Image = image;
        }

    }
}
