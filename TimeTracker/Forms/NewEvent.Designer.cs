namespace TimeTracker
{
    partial class NewEvent
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewEvent));
            eventName = new TextBox();
            submit = new Button();
            cancel = new Button();
            dateTimeStart = new DateTimePicker();
            dateTimeEnd = new DateTimePicker();
            startLabel = new Label();
            endLabel = new Label();
            SuspendLayout();
            // 
            // eventName
            // 
            eventName.Location = new Point(12, 44);
            eventName.Name = "eventName";
            eventName.Size = new Size(575, 23);
            eventName.TabIndex = 1;
            // 
            // submit
            // 
            submit.Location = new Point(390, 70);
            submit.Name = "submit";
            submit.Size = new Size(197, 26);
            submit.TabIndex = 2;
            submit.Text = "Valider";
            submit.UseVisualStyleBackColor = true;
            submit.Click += submit_Click;
            // 
            // cancel
            // 
            cancel.Location = new Point(12, 73);
            cancel.Name = "cancel";
            cancel.Size = new Size(197, 26);
            cancel.TabIndex = 3;
            cancel.Text = "Annuler";
            cancel.UseVisualStyleBackColor = true;
            cancel.Click += cancel_Click;
            // 
            // dateTimeStart
            // 
            dateTimeStart.CustomFormat = "HH:mm";
            dateTimeStart.Location = new Point(61, 12);
            dateTimeStart.Name = "dateTimeStart";
            dateTimeStart.Size = new Size(200, 23);
            dateTimeStart.TabIndex = 4;
            // 
            // dateTimeEnd
            // 
            dateTimeEnd.Location = new Point(387, 12);
            dateTimeEnd.Name = "dateTimeEnd";
            dateTimeEnd.Size = new Size(200, 23);
            dateTimeEnd.TabIndex = 5;
            // 
            // startLabel
            // 
            startLabel.AutoSize = true;
            startLabel.Location = new Point(12, 18);
            startLabel.Name = "startLabel";
            startLabel.Size = new Size(42, 15);
            startLabel.TabIndex = 6;
            startLabel.Text = "Début:";
            // 
            // endLabel
            // 
            endLabel.AutoSize = true;
            endLabel.Location = new Point(352, 18);
            endLabel.Name = "endLabel";
            endLabel.Size = new Size(29, 15);
            endLabel.TabIndex = 7;
            endLabel.Text = "Fin: ";
            // 
            // NewEvent
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoValidate = AutoValidate.EnablePreventFocusChange;
            ClientSize = new Size(600, 108);
            ControlBox = false;
            Controls.Add(endLabel);
            Controls.Add(startLabel);
            Controls.Add(dateTimeEnd);
            Controls.Add(dateTimeStart);
            Controls.Add(cancel);
            Controls.Add(submit);
            Controls.Add(eventName);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "NewEvent";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Commencer une tache";
            FormClosed += NewEvent_FormClosed;
            Load += NewEvent_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox eventName;
        private Button submit;
        private Button cancel;
        private DateTimePicker dateTimeStart;
        private DateTimePicker dateTimeEnd;
        private Label startLabel;
        private Label endLabel;
    }
}