using JiraTracker.Interaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using time.layer.objet.Interfaces;
using time.layer.objet.Objets;
using TimeTracker.Interfaces;

namespace TimeTracker.Forms
{
    public partial class ConfigForm : Form
    {
        private readonly IEventService eventService;
        private readonly IJiraService jiraService;
        private readonly ITrackerManager trackerManager;

        public ConfigForm(IEventService eventService, IJiraService jiraService, ITrackerManager trackerManager)
        {
            this.eventService = eventService;
            this.jiraService = jiraService;
            this.trackerManager = trackerManager;
            InitializeComponent();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            var config = eventService.GetConfig();

            if (config != null)
            {
                jiraUrlTextBox.Text = config.url;
                jiraLoginTextBox.Text = config.login;
                jiraTokenTextBox.Text = config.token;
                jiraProjectTextBox.Text = config.project;
                autoStartCheckBox.Checked = config.autoStart;
                sessionCheckBox.Checked = config.session;

                var startKey = config?.keys?.FirstOrDefault(k => k.key == "start");
                var editKey = config?.keys?.FirstOrDefault(k => k.key == "edit");
                var cancelKey = config?.keys?.FirstOrDefault(k => k.key == "cancel");

                if (startKey != null)
                {
                    startShortcutTextBox.Text = startKey.value;
                }

                if (editKey != null)
                {
                    editShortcutTextBox.Text = editKey.value;
                }

                if (cancelKey != null)
                {
                    cancelShortcutTextBox.Text = cancelKey.value;
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var config = new TrackerConfig
            {
                url = jiraUrlTextBox.Text,
                login = jiraLoginTextBox.Text,
                token = jiraTokenTextBox.Text,
                project = jiraProjectTextBox.Text,
                autoStart = autoStartCheckBox.Checked,
                session = sessionCheckBox.Checked,
                keys = new List<TrackerConfig.TrackerKey>
                {
                    new TrackerConfig.TrackerKey
                    {
                        key = "start",
                        value = startShortcutTextBox.Text
                    },
                    new TrackerConfig.TrackerKey
                    {
                        key = "edit",
                        value = editShortcutTextBox.Text
                    },
                    new TrackerConfig.TrackerKey
                    {
                        key = "cancel",
                        value = cancelShortcutTextBox.Text
                    }
                }
            };

            eventService.UpdateConfig(config);
            jiraService.Initialize(config.url, config.login, config.token, config.project);
            trackerManager.RefreshKeys();
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
