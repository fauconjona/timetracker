using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using time.layer.objet.Objets;

namespace TimeTracker.Interfaces
{
    public interface ITrackerManager
    {
        /// <summary>
        /// Start the tracker manager
        /// </summary>
        /// <param name="form"></param>
        void Start(TimeTracker form);

        /// <summary>
        /// Close the tracker manager
        /// </summary>
        void Close();

        /// <summary>
        /// Cancel the current event
        /// </summary>
        /// <param name="prompt">Prompt to confirm cancel</param>
        void Cancel(bool prompt = false);

        /// <summary>
        /// Edit the event
        /// </summary>
        /// <param name="trackerEvent"></param>
        void Edit(TrackerEvent trackerEvent);

        /// <summary>
        /// Create new event
        /// </summary>
        /// <param name="trackerEvent"></param>
        void Create(TrackerEvent trackerEvent);

        /// <summary>
        /// Open a new event dialog
        /// </summary>
        /// <param name="start"></param>
        /// <param name="name"></param>
        void OpenNewEvent(DateTime? start = null, string name = "New Event");

        /// <summary>
        /// Open new event dialog to create a custom event
        /// </summary>
        /// <param name="start"></param>
        /// <param name="name"></param>
        void AddNewEvent(DateTime? start = null, string name = "New Event");

        /// <summary>
        /// Start the tracker from the last event of the day
        /// </summary>
        void StartFromLastEvent();

        /// <summary>
        /// Update the label with the current event or message
        /// </summary>
        /// <param name="message">Message to display</param>
        void UpdateLabel(string message = "");

        /// <summary>
        /// Start or stop the current event
        /// </summary>
        void StartStopEvent();

        /// <summary>
        /// Open the edit dialog for the current event
        /// </summary>
        void EditCurrentEvent();

        /// <summary>
        /// Refresh the config keys
        /// </summary>
        void RefreshKeys();
    }
}
