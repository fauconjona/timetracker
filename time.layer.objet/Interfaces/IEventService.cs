using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using time.layer.objet.Objets;

namespace time.layer.objet.Interfaces
{
    public interface IEventService
    {
        /// <summary>
        /// Get all events
        /// </summary>
        /// <returns></returns>
        Dictionary<DateTime, List<TrackerEvent>> GetEvents();

        /// <summary>
        /// Get events for a specific day
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        List<TrackerEvent> GetEvents(DateTime day);

        /// <summary>
        /// Get an event by its guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        TrackerEvent? GetEvent(string guid);

        /// <summary>
        /// Add an event
        /// </summary>
        /// <param name="trackerEvent"></param>
        void AddEvent(TrackerEvent trackerEvent);

        /// <summary>
        /// Update an event
        /// </summary>
        /// <param name="trackerEvent"></param>
        void UpdateEvent(TrackerEvent trackerEvent);

        /// <summary>
        /// Delete an event
        /// </summary>
        /// <param name="trackerEvent"></param>
        void DeleteEvent(TrackerEvent trackerEvent);

        /// <summary>
        /// Delete all events for a specific day
        /// </summary>
        /// <param name="day"></param>
        void DeleteEvent(DateTime day);

        /// <summary>
        /// Merge events into one
        /// </summary>
        /// <param name="events"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        TrackerEvent MergeEvents(List<TrackerEvent> events, string title);

        /// <summary>
        /// Get the last event of the current day
        /// </summary>
        /// <returns></returns>
        TrackerEvent? GetLastEvent();

        /// <summary>
        /// Get config
        /// </summary>
        /// <returns></returns>
        TrackerConfig GetConfig();

        /// <summary>
        /// Update config
        /// </summary>
        /// <param name="config"></param>
        void UpdateConfig(TrackerConfig config);
    }
}
