using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        public NewEvent(ITrackerManager trackerManager, TrackerEvent trackerEvent, bool newEvent = true, bool isEdit = false)
        {
            this.trackerManager = trackerManager;
            this.trackerEvent = trackerEvent;
            this.newEvent = newEvent;
            this.isEdit = isEdit;
            TopMost = true;
            InitializeComponent();

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
            trackerEvent.Name = eventName.Text;
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
    }
}
