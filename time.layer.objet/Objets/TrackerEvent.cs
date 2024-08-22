using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace time.layer.objet.Objets
{
    public class TrackerEvent
    {
        public string Guid { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string Name { get; set; }
        public bool Sync { get; set; }

        public override string ToString()
        {
            if (End == null)
            {
                return $"{Start:t} - {Name}";
            }
            return $"{Start:t} - {End:t} ({(End!.Value - Start).ToString(@"hh\:mm")}) : {Name}";
        }
    }
}
