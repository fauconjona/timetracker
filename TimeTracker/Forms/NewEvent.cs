using Microsoft.Extensions.DependencyInjection;
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

namespace TimeTracker
{
    public partial class NewEvent : Form
    {
        private readonly ITrackerManager trackerManager;
        private TrackerEvent trackerEvent;
        private bool newEvent;
        private bool isEdit;
        private readonly List<TrackerConfig.Alias> aliases = new List<TrackerConfig.Alias>();

        public NewEvent(ITrackerManager trackerManager, TrackerEvent trackerEvent, bool newEvent = true, bool isEdit = false)
        {
            this.trackerManager = trackerManager;
            this.trackerEvent = trackerEvent;
            this.newEvent = newEvent;
            this.isEdit = isEdit;
            TopMost = true;
            InitializeComponent();

            typeComboBox.Items.Add("Ticket");
            typeComboBox.Items.Add("Pause");

            IEventService eventService = Program.ServiceProvider.GetRequiredService<IEventService>();
            var config = eventService.GetConfig();

            if (config is not null && config.aliases is not null)
            {
                aliases = config.aliases.ToList();
                foreach (var alias in aliases)
                {
                    typeComboBox.Items.Add(alias.name);
                }
            }

            typeComboBox.SelectedIndex = 0;

            dateTimeStart.Format = DateTimePickerFormat.Custom;
            dateTimeEnd.Format = DateTimePickerFormat.Custom;

            if (newEvent && isEdit)
            {
                dateTimeStart.CustomFormat = "dd/MM/yyyy HH:mm";
                dateTimeStart.ShowUpDown = false;
                dateTimeEnd.CustomFormat = "dd/MM/yyyy HH:mm";
                dateTimeEnd.ShowUpDown = false;
            }
            else
            {
                dateTimeStart.CustomFormat = "HH:mm";
                dateTimeStart.ShowUpDown = true;
                dateTimeEnd.CustomFormat = "HH:mm";
                dateTimeEnd.ShowUpDown = true;
            }



            if (trackerEvent != null)
            {
                dateTimeStart.Value = trackerEvent.Start;
                eventName.Text = trackerEvent.Name;
                ticketTextBox.Text = trackerEvent.Ticket;
                descriptionTextBox.Text = trackerEvent.Description;

                if (aliases.Any(a => a.name == trackerEvent.Key))
                {
                    typeComboBox.SelectedItem = trackerEvent.Key;
                    HandleTypeChange(trackerEvent.Key);
                }

                if (isEdit)
                {
                    dateTimeEnd.Visible = true;
                    endLabel.Visible = true;
                    dateTimeEnd.Value = (DateTime)trackerEvent.End;
                }
                else
                {
                    //hide end date
                    dateTimeEnd.Visible = false;
                    endLabel.Visible = false;
                }
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            if (newEvent)
            {
                trackerManager.Cancel();
            }
            Close();
        }

        private void submit_Click(object sender, EventArgs e)
        {
            trackerEvent.Start = dateTimeStart.Value;
            if (isEdit)
            {
                trackerEvent.End = dateTimeEnd.Value;
            }

            var selectedType = typeComboBox.SelectedItem.ToString();

            if (selectedType == "Ticket")
            {
                string ticket = ticketTextBox.Text;
                if (string.IsNullOrEmpty(ticket))
                {
                    MessageBox.Show("Le ticket est obligatoire", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                trackerEvent.Ticket = ticket;
                trackerEvent.Key = ticket;
                trackerEvent.Name = eventName.Text;
                trackerEvent.IsJira = true;
            }
            else if (selectedType == "Pause")
            {
                trackerEvent.Key = "Pause";
                trackerEvent.IsJira = false;
                trackerEvent.Ticket = string.Empty;
                trackerEvent.Name = "Pause";
            }
            else if (!string.IsNullOrEmpty(selectedType))
            {
                var alias = aliases.FirstOrDefault(a => a.name == selectedType);
                trackerEvent.Key = alias is not null ? alias.name : string.Empty;
                trackerEvent.Ticket = alias is not null ? alias.value : string.Empty;
                trackerEvent.Name = eventName.Text;
                trackerEvent.IsJira = true;
            }

            trackerEvent.Description = descriptionTextBox.Text;

            if (isEdit && !newEvent)
            {
                trackerManager.Edit(trackerEvent);
            }
            else if (isEdit && newEvent)
            {
                trackerManager.Create(trackerEvent);
            }
            Close();
        }

        private void NewEvent_Load(object sender, EventArgs e)
        {
            if (!newEvent)
            {
                this.Text = "Modifier l'événement";
            }
            else
            {
                this.Text = "Nouvel événement";
            }
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.ActiveControl = eventName;
        }

        private void NewEvent_FormClosed(object sender, FormClosedEventArgs e)
        {
            trackerManager.UpdateLabel();
        }

        private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleTypeChange(typeComboBox.SelectedItem.ToString());
        }

        private void HandleTypeChange(string key)
        {
            if (key == "Ticket")
            {
                ticketTextBox.Enabled = true;
                eventName.Enabled = true;
            }
            else if (key == "Pause")
            {
                ticketTextBox.Text = string.Empty;
                ticketTextBox.Enabled = false;
                eventName.Text = "Pause";
                eventName.Enabled = false;
            }
            else
            {
                var alias = aliases.FirstOrDefault(a => a.name == key);
                if (alias != null)
                {
                    ticketTextBox.Text = alias.value;
                    ticketTextBox.Enabled = false;
                    eventName.Text = alias.name;
                    eventName.Enabled = true;
                }
                else
                {
                    // Ticket
                    ticketTextBox.Enabled = true;
                    eventName.Enabled = true;
                }
            }
        }
    }
}
