using JiraTracker.Interaces;
using time.layer.objet.Interfaces;
using time.layer.objet.Objets;
using TimeTracker.Interfaces;

namespace TimeTracker
{
    public partial class TimeTracker : Form
    {

        private IEventService eventService;
        private IJiraService jiraService;
        private readonly ITrackerManager trackerManager;

        public TimeTracker(ITrackerManager trackerManager, IEventService eventService, IJiraService jiraService)
        {
            this.trackerManager = trackerManager;
            this.eventService = eventService;
            this.jiraService = jiraService;
            InitializeComponent();
        }

        public void AddEvent(TrackerEvent trackerEvent)
        {
            eventService.AddEvent(trackerEvent);
            RefreshEvents();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            trackerManager.UpdateLabel();
            RefreshEvents();

            var config = eventService.GetConfig();
            if (config.autoStart)
            {
                trackerManager.OpenNewEvent();
            }
        }

        private void Form1_Closed(object sender, FormClosedEventArgs e)
        {
            trackerManager.Close();
        }

        public void RefreshEvents()
        {
            List<object> selectedNodes = GetSelectedEvents();
            List<DateTime> expandedNodes = new List<DateTime>();

            foreach (TreeNode node in treeView1.Nodes)
            {
                if (node.IsExpanded)
                {
                    expandedNodes.Add((DateTime)node.Tag);
                }
            }

            treeView1.Nodes.Clear();
            Dictionary<DateTime, List<TrackerEvent>> events = eventService.GetEvents();
            foreach (var day in events)
            {
                var node = treeView1.Nodes.Add(day.Key.ToString("D"));
                node.Tag = day.Key;
                var dayEvents = day.Value.OrderBy(e => e.Start).ToList();
                foreach (var trackerEvent in dayEvents)
                {
                    var nodeEvent = node.Nodes.Add(trackerEvent.ToString());
                    // add guid
                    nodeEvent.Tag = trackerEvent.Guid;
                    nodeEvent.ImageKey = trackerEvent.Sync ? "sync" : "not_sync";
                }
            }

            foreach (TreeNode node in treeView1.Nodes)
            {
                if (expandedNodes.Contains((DateTime)node.Tag))
                {
                    node.Expand();
                }
            }

            foreach (TreeNode node in treeView1.Nodes)
            {
                node.Checked = selectedNodes.Contains(node.Tag);
                foreach (TreeNode subNode in node.Nodes)
                {
                    subNode.Checked = selectedNodes.Contains(subNode.Tag);
                }
            }
        }

        private void deleteEvent_Click(object sender, EventArgs e)
        {
            List<object> events = GetSelectedEvents();
            foreach (var tag in events)
            {
                if (tag != null)
                {
                    DeleteEvent(tag);
                }
            }

            RefreshEvents();
        }

        private void editEvent_Click(object sender, EventArgs e)
        {
            List<object> events = GetSelectedEvents();

            if (events.Count == 0)
            {
                MessageBox.Show("Aucune tâche sélectionnée.", "Time Tracker", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return;
            }

            if (events.Count > 1)
            {
                MessageBox.Show("Sélectionnez une seule tâche.", "Time Tracker", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return;
            }

            var currentEvent = events[0];
            if (currentEvent != null)
            {
                if (currentEvent is string && !string.IsNullOrEmpty(currentEvent as string))
                {
                    TrackerEvent? trackerEvent = eventService.GetEvent((string)currentEvent);
                    if (trackerEvent != null)
                    {
                        NewEvent newEvent = new NewEvent(trackerManager, trackerEvent, false, true);
                        newEvent.ShowDialog();
                    }
                }
            }
        }

        public void EditEvent(TrackerEvent trackerEvent)
        {
            eventService.UpdateEvent(trackerEvent);
            RefreshEvents();
        }

        // C'est pas beau mais c'est pour éviter le select de base
        private bool _firstCheck = true;
        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (!_firstCheck)
            {
                var node = e.Node;
                if (node != null)
                {
                    node.Checked = !node.Checked;
                }
            }
            _firstCheck = false;
            e.Cancel = true;
        }

        private List<object> GetSelectedEvents()
        {
            List<object> selectedEvents = new List<object>();
            foreach (TreeNode node in treeView1.Nodes)
            {
                if (node.Checked)
                {
                    selectedEvents.Add(node.Tag);
                }
                foreach (TreeNode subNode in node.Nodes)
                {
                    if (subNode.Checked)
                    {
                        selectedEvents.Add(subNode.Tag);
                    }
                }
            }
            return selectedEvents;
        }

        private void DeleteEvent(object tag)
        {
            if (tag is DateTime)
            {
                var result = MessageBox.Show($"Supprimer \"{tag as DateTime?}\"?", "Time Tracker", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.Yes)
                {
                    eventService.DeleteEvent((DateTime)tag);
                }
            }
            else if (tag is string && !string.IsNullOrEmpty(tag as string))
            {
                TrackerEvent? trackerEvent = eventService.GetEvent((string)tag);
                if (trackerEvent != null)
                {
                    var result = MessageBox.Show($"Supprimer \"{trackerEvent.Name}\"?", "Time Tracker", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    if (result == DialogResult.Yes)
                    {
                        eventService.DeleteEvent(trackerEvent);
                    }
                }
            }
        }

        private void mergeButton_Click(object sender, EventArgs e)
        {
            List<object> events = GetSelectedEvents();

            //check if there are at least 2 events
            if (events.Count < 2)
            {
                MessageBox.Show("Sélectionnez au moins 2 tâches.", "Time Tracker", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return;
            }

            //check if all events are strings
            foreach (var tag in events)
            {
                if (tag is DateTime)
                {
                    MessageBox.Show("Sélectionnez des tâches à fusionner.", "Time Tracker", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    return;
                }
            }

            //check if all events are in the same day
            DateTime? day = null;
            List<TrackerEvent> eventsList = new List<TrackerEvent>();
            foreach (var tag in events)
            {
                TrackerEvent? trackerEvent = eventService.GetEvent((string)tag);
                if (trackerEvent != null)
                {
                    if (day == null)
                    {
                        day = trackerEvent.Start.Date;
                    }
                    else if (day != trackerEvent.Start.Date)
                    {
                        MessageBox.Show("Sélectionnez des tâches du même jour.", "Time Tracker", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        return;
                    }
                    eventsList.Add(trackerEvent);
                }
            }

            // confirm merge
            string eventsNames = string.Join(", ", eventsList.Select(e => e.Name));

            var result = MessageBox.Show($"Fusionner les tâches suivants: {eventsNames}?", "Time Tracker", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            if (result != DialogResult.Yes)
            {
                return;
            }

            //merge events
            eventService.MergeEvents(eventsList, eventsList[0].Name);
            RefreshEvents();
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            trackerManager.StartFromLastEvent();
        }

        private void syncButton_Click(object sender, EventArgs e)
        {
            DoSync();
        }

        private async Task DoSync()
        {
            var confirm = MessageBox.Show("Synchroniser les tâches sélectionnées avec Jira?", "Time Tracker", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            List<object> events = GetSelectedEvents().ToList();

            trackerManager.UpdateLabel("Synchronisation en cours...");

            foreach (var tag in events)
            {
                try
                {
                    if (tag is string && !string.IsNullOrEmpty(tag as string))
                    {
                        TrackerEvent? trackerEvent = eventService.GetEvent((string)tag);
                        if (trackerEvent != null && !trackerEvent.Sync)
                        {
                            Console.WriteLine($"Syncing {trackerEvent.Name}");
                            trackerEvent.Sync = await jiraService.AddWorklog(trackerEvent.Name, trackerEvent.Start, trackerEvent.End ?? DateTime.Now);
                            eventService.UpdateEvent(trackerEvent);
                        }
                    }

                    if (tag is DateTime)
                    {
                        List<TrackerEvent> dayEvents = eventService.GetEvents((DateTime)tag);
                        foreach (var trackerEvent in dayEvents)
                        {
                            if (!trackerEvent.Sync)
                            {
                                Console.WriteLine($"Syncing {trackerEvent.Name}");
                                trackerEvent.Sync = await jiraService.AddWorklog(trackerEvent.Name, trackerEvent.Start, trackerEvent.End ?? DateTime.Now);
                                eventService.UpdateEvent(trackerEvent);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la synchronisation: {ex.Message}", "Time Tracker", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
            }

            MessageBox.Show("Synchronisation terminée.", "Time Tracker", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            trackerManager.UpdateLabel();
            Invoke(new Action(() =>
            {
                RefreshEvents();
            }));
        }

        private void startStopButton_Click(object sender, EventArgs e)
        {
            trackerManager.StartStopEvent();
        }

        private void editCurrentButton_Click(object sender, EventArgs e)
        {
            trackerManager.EditCurrentEvent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            trackerManager.Cancel(true);
        }

        private void configButton_Click(object sender, EventArgs e)
        {
            Program.OpenConfig();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            List<object> events = GetSelectedEvents().ToList();

            if (events.Count == 1 && events[0] is DateTime)
            {
                trackerManager.AddNewEvent((DateTime)events[0]);
            }
            else if (events.Count == 1 && events[0] is string && !string.IsNullOrEmpty(events[0] as string))
            {
                TrackerEvent? trackerEvent = eventService.GetEvent((string)events[0]);
                trackerManager.AddNewEvent(trackerEvent?.End);
            }
            else
            {
                trackerManager.AddNewEvent();
            }
        }
    }
}