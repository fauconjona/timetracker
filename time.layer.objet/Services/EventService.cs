using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using time.layer.objet.Interfaces;
using time.layer.objet.Objets;

namespace time.layer.objet.Services
{
    public class EventService : Interfaces.IEventService
    {
        public Dictionary<DateTime, List<TrackerEvent>> GetEvents()
        {
            List<string> files = ListFiles();
            Dictionary<DateTime, List<TrackerEvent>> events = new Dictionary<DateTime, List<TrackerEvent>>();
            foreach (string file in files)
            {

                DateTime day;
                if (DateTime.TryParse(file, out day))
                {
                    events.Add(day, GetEvents(day));
                }
            }
            return events;
        }

        public List<TrackerEvent> GetEvents(DateTime day)
        {
            if (IsFileExist(day.ToString("yyyy-MM-dd")))
            {
                FileStream file = OpenFile(day.ToString("yyyy-MM-dd"));
                byte[] bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                string json = Encoding.UTF8.GetString(bytes);
                file.Close();
                return JsonSerializer.Deserialize<List<TrackerEvent>>(json) ?? new List<TrackerEvent>();
            }
            return new List<TrackerEvent>();
        }

        public TrackerEvent? GetEvent(string guid)
        {
            Dictionary<DateTime, List<TrackerEvent>> events = GetEvents();
            foreach (var day in events)
            {
                TrackerEvent? trackerEvent = day.Value.FirstOrDefault(e => e.Guid == guid);
                if (trackerEvent != null)
                {
                    return trackerEvent;
                }
            }
            return null;
        }

        public void AddEvent(TrackerEvent trackerEvent)
        {
            string day = trackerEvent.Start.ToString("yyyy-MM-dd");
            List<TrackerEvent> events = GetEvents(trackerEvent.Start);
            events.Add(trackerEvent);
            string json = JsonSerializer.Serialize(events);
            SaveFile(day, json);
        }

        public void UpdateEvent(TrackerEvent trackerEvent)
        {
            string day = trackerEvent.Start.ToString("yyyy-MM-dd");
            List<TrackerEvent> events = GetEvents(trackerEvent.Start);
            TrackerEvent? oldEvent = events.FirstOrDefault(e => e.Guid == trackerEvent.Guid);
            if (oldEvent != null)
            {
                events.Remove(oldEvent);
                events.Add(trackerEvent);
                string json = JsonSerializer.Serialize(events);
                SaveFile(day, json);
            }
        }

        public void DeleteEvent(TrackerEvent trackerEvent)
        {
            string day = trackerEvent.Start.ToString("yyyy-MM-dd");
            List<TrackerEvent> events = GetEvents(trackerEvent.Start);
            TrackerEvent? oldEvent = events.FirstOrDefault(e => e.Guid == trackerEvent.Guid);
            if (oldEvent != null)
            {
                events.Remove(oldEvent);
                string json = JsonSerializer.Serialize(events);
                SaveFile(day, json);
            }
        }

        public void DeleteEvent(DateTime day)
        {
            if (IsFileExist(day.ToString("yyyy-MM-dd")))
            {
                File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TimeLayerObjet", "Events", $"{day.ToString("yyyy-MM-dd")}.json"));
            }
        }

        public TrackerEvent MergeEvents(List<TrackerEvent> events, TrackerEvent model)
        {
            DateTime start = events.Min(e => e.Start);
            DateTime end = events.Max(e => e.End!.Value);
            TrackerEvent trackerEvent = new TrackerEvent
            {
                Guid = Guid.NewGuid().ToString(),
                Name = model.Name,
                Ticket = model.Ticket,
                Project = model.Project,
                Key = model.Key,
                Description = model.Description,
                IsJira = model.IsJira,
                Start = start,
                End = end,
                Sync = false
            };
            foreach (TrackerEvent e in events)
            {
                DeleteEvent(e);
            }
            AddEvent(trackerEvent);
            return trackerEvent;
        }

        public TrackerEvent? GetLastEvent()
        {
            Dictionary<DateTime, List<TrackerEvent>> events = GetEvents();
            var currentDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            if (!events.ContainsKey(currentDay))
            {
                return null;
            }

            List<TrackerEvent> lastEvents = events[currentDay];
            return lastEvents.OrderByDescending(e => e.End).FirstOrDefault();
        }

        public TrackerConfig GetConfig()
        {
            if (IsFileExist("config"))
            {
                FileStream file = OpenFile("config");
                byte[] bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                string json = Encoding.UTF8.GetString(bytes);
                file.Close();
                return JsonSerializer.Deserialize<TrackerConfig>(json) ?? new TrackerConfig();
            }

            return new TrackerConfig();
        }

        public void UpdateConfig(TrackerConfig config)
        {
            string json = JsonSerializer.Serialize(config);
            SaveFile("config", json);
        }

        public void CheckVersion(string version)
        {
            if (CompareVersions(GetVersion(), version) < 0)
            {
                MigrateEvents();
                SetVersion(version);
            }
        }

        public Dictionary<DateTime, List<TrackerEvent>> MigrateEvents()
        {
            var events = GetEvents();
            var config = GetConfig();

            foreach (var dayPair in events)
            {
                var dayEvents = dayPair.Value.Where(e => e.IsJira && string.IsNullOrEmpty(e.Ticket) && !string.IsNullOrEmpty(e.Name));

                foreach (var trackerEvent in dayEvents)
                {
                    if (string.IsNullOrEmpty(trackerEvent.Key))
                    {
                        trackerEvent.Key = trackerEvent.Name;
                        UpdateEvent(trackerEvent);
                    }

                    if (trackerEvent.Name == "Pause")
                    {
                        trackerEvent.IsJira = false;
                        UpdateEvent(trackerEvent);
                        continue;
                    }

                    if (config != null && config.aliases != null)
                    {
                        var alias = config.aliases.FirstOrDefault(a => a.name == trackerEvent.Key);

                        if (alias != null)
                        {
                            trackerEvent.Ticket = alias.value;
                            UpdateEvent(trackerEvent);
                        }
                    }
                }
            }

            return events;
        }

        private string GetVersion()
        {
            InitFolder();
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TimeLayerObjet", "Events", ".version");

            //read ".version" file 
            if (File.Exists(path))
            {
                FileStream file = File.OpenRead(path);
                byte[] bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                string version = Encoding.UTF8.GetString(bytes);
                file.Close();
                return version;
            }

            return string.Empty;
        }

        private string SetVersion(string version)
        {
            InitFolder();
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TimeLayerObjet", "Events", ".version");
            File.WriteAllText(path, version);
            return version;
        }

        private static List<string> ListFiles()
        {
            InitFolder();
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TimeLayerObjet", "Events");
            return Directory.GetFiles(path).Select(f => Path.GetFileNameWithoutExtension(f)).ToList();
        }

        private static bool IsFileExist(string day)
        {
            return ListFiles().Contains(day);
        }

        /// <summary>
        /// Create folder for events (%appdata%/TimeLayerObjet/Events)
        /// </summary>
        private static void InitFolder()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TimeLayerObjet", "Events");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Open json file for events with read access
        /// </summary>
        private static FileStream OpenFile(string day)
        {
            InitFolder();
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TimeLayerObjet", "Events", $"{day}.json");

            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }

            return File.Open(path, FileMode.Open, FileAccess.Read);
        }

        /// <summary>
        /// Save json file for events
        /// </summary>  
        private static void SaveFile(string day, string content)
        {
            InitFolder();
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TimeLayerObjet", "Events", $"{day}.json");
            File.WriteAllText(path, content);
        }

        private int CompareVersions(string a, string b)
        {
            if (string.IsNullOrEmpty(a))
            {
                return -1;
            }
            return (new Version(a)).CompareTo(new Version(b));
        }
    }
}
