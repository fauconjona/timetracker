using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace time.layer.objet.Objets
{
    public class TrackerConfig
    {
        public string url { get; set; }
        public string login { get; set; }
        public string token { get; set; }
        public string project { get; set; }
        public bool autoStart { get; set; }
        public bool session { get; set; }

        public List<TrackerKey> keys { get; set; }

        public class TrackerKey
        {
            public string key { get; set; }
            public string value { get; set; }
        }
    }
}
