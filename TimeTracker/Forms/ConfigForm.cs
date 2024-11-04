using JiraTracker.Interaces;
using Newtonsoft.Json.Linq;
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
using static time.layer.objet.Objets.TrackerConfig;

namespace TimeTracker.Forms
{
    public partial class ConfigForm : Form
    {
        private readonly IEventService eventService;
        private readonly IJiraService jiraService;
        private readonly ITrackerManager trackerManager;
        private List<TrackerConfig.Alias> Aliases = new List<TrackerConfig.Alias>();
        private bool aliasesChanged = false;

        public ConfigForm(IEventService eventService, IJiraService jiraService, ITrackerManager trackerManager)
        {
            this.eventService = eventService;
            this.jiraService = jiraService;
            this.trackerManager = trackerManager;
            InitializeComponent();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            dayDurationTimePicker.Format = DateTimePickerFormat.Custom;
            dayDurationTimePicker.ShowUpDown = true;
            dayDurationTimePicker.Value = DateTime.Today.Add(new TimeSpan(7, 45, 0));

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

                if (config!.dayDuration != TimeSpan.Zero)
                {
                    dayDurationTimePicker.Value = DateTime.Today.Add(config.dayDuration);
                }

                RefreshAliases(config!);
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
                dayDuration = dayDurationTimePicker.Value.TimeOfDay,
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
                },
                aliases = this.Aliases
            };

            eventService.UpdateConfig(config);
            jiraService.Initialize(config.url, config.login, config.token, config.project);
            trackerManager.RefreshKeys();
            if (aliasesChanged)
            {
                trackerManager.MigrateEvents(true);
            }
            else
            {
                trackerManager.RefreshEvents();
            }
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addAliasButton_Click(object sender, EventArgs e)
        {
            var aliasForm = new AliasForm(this);
            aliasForm.ShowDialog();
        }

        public void RefreshAliases(TrackerConfig? config = null)
        {
            if (config != null && config.aliases != null)
            {
                this.Aliases = config.aliases;
            }

            this.aliasesTreeView.Nodes.Clear();

            foreach (var alias in this.Aliases)
            {
                TreeNode node = new(alias.ToString())
                {
                    Tag = alias.id
                };
                this.aliasesTreeView.Nodes.Add(node);
            }
        }

        public void AddAlias(int? id, string name, string value)
        {
            if (id is null)
            {
                var maxId = Aliases.Count > 0 ? Aliases.Max(a => a.id) : 0;
                Aliases.Add(
                    new TrackerConfig.Alias
                    {
                        id = maxId + 1,
                        name = name,
                        value = value
                    }
                );
            }
            else
            {
                var index = Aliases.FindIndex(a => a.id == id);
                if (index >= 0)
                {
                    Aliases[index].name = name;
                    Aliases[index].value = value;
                }
            }

            aliasesChanged = true;

            RefreshAliases();
        }

        public void RemoveAlias(string name)
        {
            var index = Aliases.FindIndex(a => a.name == name);

            if (index >= 0)
            {
                Aliases.RemoveAt(index);
            }

            aliasesChanged = true;

            RefreshAliases();
        }

        private void deleteAliasButton_Click(object sender, EventArgs e)
        {
            var id = (int)this.aliasesTreeView.SelectedNode.Tag;
            var alias = Aliases.FirstOrDefault(a => a.id == id);
            if (alias != null)
            {
                RemoveAlias(alias.name);
            }
            else
            {
                MessageBox.Show("L'élément sélectionné est invalide", "Time Tracker", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void aliasesTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var id = (int)e.Node.Tag;
            var alias = Aliases.FirstOrDefault(a => a.id == id);
            if (alias != null) {
                var aliasForm = new AliasForm(this, alias.id, alias.name, alias.value);
                aliasForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("L'élément sélectionné est invalide", "Time Tracker", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }
    }
}
