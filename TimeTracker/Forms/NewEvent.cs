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
                    typeComboBox.Items.Add(alias.ToString());
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

                if (trackerEvent.AliasId is not null)
                {
                    var alias = aliases.FirstOrDefault(a => a.id == trackerEvent.AliasId);

                    if (alias is not null)
                    {
                        typeComboBox.SelectedItem = alias.ToString();
                        HandleTypeChange(alias.id);
                    }
                    else
                    {
                        typeComboBox.SelectedItem = "Ticket";
                    }
                }
                else if (!trackerEvent.IsJira)
                {
                    typeComboBox.SelectedItem = "Pause";
                }
                else
                {
                    typeComboBox.SelectedItem = "Ticket";
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
                trackerEvent.AliasId = null;
                trackerEvent.Name = eventName.Text;
                trackerEvent.IsJira = true;
            }
            else if (selectedType == "Pause")
            {
                trackerEvent.AliasId = null;
                trackerEvent.IsJira = false;
                trackerEvent.Ticket = string.Empty;
                trackerEvent.Name = "Pause";
            }
            else if (!string.IsNullOrEmpty(selectedType))
            {
                var alias = aliases.FirstOrDefault(a => a.ToString() == selectedType);
                trackerEvent.AliasId = alias?.id ?? null;
                trackerEvent.Ticket = alias?.value ?? string.Empty;
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
            var aliasSelected = typeComboBox.SelectedItem.ToString();
            if (aliasSelected != "Ticket" && aliasSelected != "Pause")
            {
                var alias = aliases.FirstOrDefault(a => a.ToString() == aliasSelected);
                if (alias is not null)
                {
                    HandleTypeChange(alias.id);
                }
            }
            else if (aliasSelected == "Ticket")
            {
                ticketTextBox.Enabled = true;
                ticketTextBox.Text = string.Empty;
                eventName.Enabled = true;
                eventName.Text = string.Empty;
            }
            else if (aliasSelected == "Pause")
            {
                ticketTextBox.Enabled = false;
                ticketTextBox.Text = string.Empty;
                eventName.Enabled = false;
                eventName.Text = "Pause";
            }
        }

        private void HandleTypeChange(int aliasId)
        {
            var alias = aliases.FirstOrDefault(a => a.id == aliasId);

            if (alias != null)
            {
                ticketTextBox.Text = alias.value;
                ticketTextBox.Enabled = false;
                eventName.Text = alias.name;
                eventName.Enabled = false;
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
