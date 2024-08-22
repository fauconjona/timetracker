using KeyLogger.Interfaces;
using KeyLogger.Services;
using Microsoft.Win32;
using time.layer.objet.Interfaces;
using time.layer.objet.Objets;
using TimeTracker.Interfaces;

namespace TimeTracker.Services
{
    public class TrackerManager : ITrackerManager
    {
        private TrackerEvent? currentEvent;
        private TimeTracker? form;
        private readonly IEventService eventService;
        private readonly IKeyLoggerService keyLoggerService;
        private DateTime? sessionLock = null;
        private List<TrackerConfig.TrackerKey> trackerKeys = new List<TrackerConfig.TrackerKey>();

        private bool IsAutoStart => eventService.GetConfig()?.autoStart ?? false;

        public TrackerManager(IEventService eventService, IKeyLoggerService keyLoggerService)
        {
            this.eventService = eventService;
            this.keyLoggerService = keyLoggerService;

            RefreshKeys();
        }

        public void Start(TimeTracker form)
        {
            this.form = form;
            KeyLoggerService.LogHandler logHandler = (key) =>
            {
                if (string.IsNullOrEmpty(key))
                {
                    return;
                }

                form.Invoke(new Action(() =>
                {
                    Console.WriteLine(key);
                    form.DebugLabel.Text = string.Format("Debug: {0}", key);
                }));

                if (key == GetKey("start"))
                {
                    StartStopEvent();
                }
                else if (key == GetKey("edit"))
                {
                    EditCurrentEvent();
                }
                else if (key == GetKey("cancel"))
                {
                    Cancel(true);
                }
            };

            keyLoggerService.Start(logHandler);

            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SessionSwitch);
            
            form.Shown += (sender, e) =>
            {
                RefreshButtons();
            };
        }

        public void Close()
        {
            try
            {
                keyLoggerService?.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Cancel(bool prompt = false)
        {
            bool cancel = true;
            if (currentEvent != null && prompt)
            {
                var result = MessageBox.Show($"Annuler \"{currentEvent.Name}\"?", "Time Tracker", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                cancel = result == DialogResult.OK;
            }

            if (cancel)
            {
                currentEvent = null;
                UpdateLabel();
                RefreshButtons();
            }
        }

        public void Submit()
        {
            if (currentEvent != null)
            {
                form?.Invoke(new Action(() =>
                {
                    form?.AddEvent(currentEvent);
                }));
                currentEvent = null;
                UpdateLabel();
                RefreshButtons();
            }
        }

        public void Edit(TrackerEvent trackerEvent)
        {
            form?.Invoke(new Action(() =>
            {
                form?.EditEvent(trackerEvent);
            }));
            RefreshButtons();
        }

        private void SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            var config = eventService.GetConfig();

            if (config == null || !config.session)
            {
                return;
            }

            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                if (currentEvent != null)
                {
                    currentEvent.End = DateTime.Now;
                    Submit();
                }
                sessionLock = DateTime.Now;
                UpdateLabel();
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                // wait 2 secondes before starting a new event
                Thread.Sleep(2000);

                if (sessionLock != null && DateTime.Now.DayOfYear == sessionLock?.DayOfYear)
                {
                    InitNewEvent();
                    sessionLock = null;
                }
            }
        }

        public void OpenNewEvent(DateTime? start = null, string name = "New Event")
        {
            if (start == null)
            {
                start = DateTime.Now;
            }
            currentEvent = new TrackerEvent
            {
                Guid = Guid.NewGuid().ToString(),
                Start = start.Value,
                Name = name
            };
            NewEvent newEventForm = new NewEvent(this, currentEvent);
            newEventForm.ShowDialog();
            UpdateLabel();
            RefreshButtons();
        }

        public void UpdateLabel(string message = "")
        {
            form?.Invoke(new Action(() =>
            {
                if (!string.IsNullOrEmpty(message))
                {
                    form.progressLabel.Text = message;
                } 
                else
                {
                    form.progressLabel.Text = currentEvent == null ? "Aucune tâche en cours" : $"En cours: {currentEvent.Name} depuis {currentEvent.Start:t}";
                }
            }));
        }

        public void StartFromLastEvent()
        {
            var lastEvent = eventService.GetLastEvent();
            if (lastEvent != null)
            {
                var start = lastEvent.End;
                OpenNewEvent(start);
            }
        }

        public void StartStopEvent()
        {
            if (currentEvent == null)
            {
                OpenNewEvent();
            }
            else
            {
                currentEvent.End = DateTime.Now;
                // open windows alert box top most
                var result = MessageBox.Show($"{currentEvent.Name}: Début: {currentEvent.Start:t} / Fin: {currentEvent.End:t} / Durée: {(currentEvent.End!.Value - currentEvent.Start).ToString(@"hh\:mm")}", "Time Tracker", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.OK)
                {
                    Submit();
                    if (IsAutoStart)
                    {
                        OpenNewEvent();
                    }
                }
            }
        }

        public void EditCurrentEvent()
        {
            if (currentEvent != null)
            {
                NewEvent newEventForm = new NewEvent(this, currentEvent, false);
                newEventForm.ShowDialog();
            }
        }

        public void RefreshKeys()
        {
            var config = eventService.GetConfig();
            if (config != null)
            {
                trackerKeys = config.keys;
            }

            RefreshButtons();
        }

        #region private methods

        private void InitNewEvent()
        {
            var lastEvent = eventService.GetLastEvent();
            bool isPause = false;

            // Ask if it's a Pause event
            var result = MessageBox.Show("Enregistrer la pause?", "Time Tracker", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            if (result == DialogResult.Yes)
            {
                isPause = true;
                currentEvent = new TrackerEvent
                {
                    Guid = Guid.NewGuid().ToString(),
                    Start = sessionLock!.Value,
                    End = DateTime.Now,
                    Name = "Pause"
                };
                Submit();
            }

            if (lastEvent != null)
            {
                var message = string.Format("Reprendre la dernière tâche \"{0}\"?", lastEvent?.Name);
                result = MessageBox.Show(message, "Time Tracker", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                if (result == DialogResult.Cancel)
                {
                    return;
                }

                if (result == DialogResult.Yes)
                {
                    if (isPause)
                    {
                        currentEvent = new TrackerEvent
                        {
                            Guid = Guid.NewGuid().ToString(),
                            Start = DateTime.Now,
                            Name = lastEvent!.Name
                        };
                    }
                    else
                    {
                        eventService.DeleteEvent(lastEvent!);
                        form?.Invoke(new Action(() =>
                        {
                            form.RefreshEvents();
                        }));
                        currentEvent = lastEvent!;
                        currentEvent.End = null;
                    }
                    UpdateLabel();
                    RefreshButtons();
                }
                else if (result == DialogResult.No)
                {
                    var start = lastEvent!.End;
                    OpenNewEvent(start);
                }
            }
        }

        private string GetKey(string key)
        {
            var trackerKey = trackerKeys.FirstOrDefault(k => k.key == key);
            return trackerKey?.value ?? string.Empty;
        }

        private void RefreshButtons()
        {
            // start/stop button
            form?.Invoke(new Action(() =>
            {
                var key = GetKey("start");
                form.startStopButton.Text = (currentEvent == null ? "Démarrer" : "Arrêter") + (string.IsNullOrEmpty(key) ? "" : $" ({key})");
            }));

            // edit button
            form?.Invoke(new Action(() =>
            {
                var key = GetKey("edit");
                form.editCurrentButton.Text = "Modifier" + (string.IsNullOrEmpty(key) ? "" : $" ({key})");
                form.editCurrentButton.Enabled = currentEvent != null;
            }));

            // cancel button
            form?.Invoke(new Action(() =>
            {
                var key = GetKey("cancel");
                form.cancelButton.Text = "Annuler" + (string.IsNullOrEmpty(key) ? "" : $" ({key})");
                form.cancelButton.Enabled = currentEvent != null;
            }));

            // start from last event button
            form?.Invoke(new Action(() =>
            {
                var lastEvent = eventService.GetLastEvent();
                form.restartButton.Enabled = lastEvent != null;
                form.restartButton.Text = "Reprendre depuis la dernière tâche" + (lastEvent == null ? "" : $" ({lastEvent.End:t})");
            }));
        }

        #endregion
    }
}
