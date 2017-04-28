using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Background_sounds.Models
{
    public class Sound
    {
        private double _volume=1;
        public string Name { get; set; }

        [XmlIgnore]
        public Uri Source { get; set; }

        [XmlAttribute("Source")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string UriString
        {
            get { return Source?.ToString(); }
            set { Source = value == null ? null : new Uri(value,UriKind.Relative); }
        }

        public double Volume { get { return _volume; } set { if (value <= 1 && value>=0) _volume = value; } }

        public Sound() { }
        public Sound(string name, Uri source)
        {
            Name = name;
            Source = source;
        }
    }
}
