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

        public List<Alias> aliases { get; set; }


        public class TrackerKey
        {
            public string key { get; set; }
            public string value { get; set; }
        }

        public class Alias
        {
            public string name { get; set; }
            public string value { get; set; }

            public override string ToString()
            {
                return $"{name} ({value})";
            }
        }

        public Dictionary<string, string> GetAliases()
        {
            return aliases.ToDictionary(a => a.name, a => a.value);
        }
    }
}
