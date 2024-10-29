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
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Ticket { get; set; }= string.Empty;
        public string Project { get; set; } = string.Empty;
        public bool IsJira { get; set; } = true;
        public bool Sync { get; set; }

        public override string ToString()
        {
            var displayName = Name;
            if (!string.IsNullOrEmpty(Ticket))
            {
                displayName = $"{Ticket}: {Name}";
            }
            if (!string.IsNullOrEmpty(Description))
            {
                displayName = $"{displayName} ({Description})";
            }
            if (End == null)
            {
                return $"{Start:t} - {displayName}";
            }
            return $"{Start:t} - {End:t} ({(End!.Value - Start).ToString(@"hh\:mm")}) : {displayName}";
        }
    }
}
