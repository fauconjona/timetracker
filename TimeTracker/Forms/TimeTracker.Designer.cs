namespace TimeTracker
{
    partial class TimeTracker
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimeTracker));
            treeView1 = new TreeView();
            icons = new ImageList(components);
            DebugLabel = new Label();
            deleteEvent = new Button();
            editEvent = new Button();
            progressLabel = new Label();
            mergeButton = new Button();
            restartButton = new Button();
            syncButton = new Button();
            groupBox1 = new GroupBox();
            cancelButton = new Button();
            editCurrentButton = new Button();
            startStopButton = new Button();
            groupBox2 = new GroupBox();
            configButton = new Button();
            addButton = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // treeView1
            // 
            treeView1.CheckBoxes = true;
            treeView1.ImageIndex = 0;
            treeView1.ImageList = icons;
            treeView1.Location = new Point(12, 12);
            treeView1.Name = "treeView1";
            treeView1.SelectedImageIndex = 0;
            treeView1.Size = new Size(489, 409);
            treeView1.TabIndex = 0;
            treeView1.BeforeSelect += treeView1_BeforeSelect;
            // 
            // icons
            // 
            icons.ColorDepth = ColorDepth.Depth8Bit;
            icons.ImageStream = (ImageListStreamer)resources.GetObject("icons.ImageStream");
            icons.TransparentColor = Color.Transparent;
            icons.Images.SetKeyName(0, "calendar");
            icons.Images.SetKeyName(1, "not_sync");
            icons.Images.SetKeyName(2, "sync");
            // 
            // DebugLabel
            // 
            DebugLabel.AutoSize = true;
            DebugLabel.Location = new Point(507, 406);
            DebugLabel.Name = "DebugLabel";
            DebugLabel.Size = new Size(48, 15);
            DebugLabel.TabIndex = 1;
            DebugLabel.Text = "Debug: ";
            DebugLabel.Visible = false;
            // 
            // deleteEvent
            // 
            deleteEvent.ForeColor = Color.Black;
            deleteEvent.Location = new Point(107, 22);
            deleteEvent.Name = "deleteEvent";
            deleteEvent.Size = new Size(95, 23);
            deleteEvent.TabIndex = 2;
            deleteEvent.Text = "Supprimer";
            deleteEvent.UseVisualStyleBackColor = true;
            deleteEvent.Click += deleteEvent_Click;
            // 
            // editEvent
            // 
            editEvent.Location = new Point(6, 22);
            editEvent.Name = "editEvent";
            editEvent.Size = new Size(95, 23);
            editEvent.TabIndex = 3;
            editEvent.Text = "Modifier";
            editEvent.UseVisualStyleBackColor = true;
            editEvent.Click += editEvent_Click;
            // 
            // progressLabel
            // 
            progressLabel.AutoSize = true;
            progressLabel.Location = new Point(6, 77);
            progressLabel.MaximumSize = new Size(300, 0);
            progressLabel.Name = "progressLabel";
            progressLabel.Size = new Size(58, 15);
            progressLabel.TabIndex = 4;
            progressLabel.Text = "En cours: ";
            // 
            // mergeButton
            // 
            mergeButton.Location = new Point(208, 22);
            mergeButton.Name = "mergeButton";
            mergeButton.Size = new Size(95, 23);
            mergeButton.TabIndex = 5;
            mergeButton.Text = "Fusionner";
            mergeButton.UseVisualStyleBackColor = true;
            mergeButton.Click += mergeButton_Click;
            // 
            // restartButton
            // 
            restartButton.Location = new Point(6, 51);
            restartButton.Name = "restartButton";
            restartButton.Size = new Size(297, 23);
            restartButton.TabIndex = 6;
            restartButton.Text = "Reprendre depuis la dernière tâche";
            restartButton.UseVisualStyleBackColor = true;
            restartButton.Click += restartButton_Click;
            // 
            // syncButton
            // 
            syncButton.Location = new Point(155, 51);
            syncButton.Name = "syncButton";
            syncButton.Size = new Size(148, 23);
            syncButton.TabIndex = 7;
            syncButton.Text = "Synchroniser avec Jira";
            syncButton.UseVisualStyleBackColor = true;
            syncButton.Click += syncButton_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cancelButton);
            groupBox1.Controls.Add(editCurrentButton);
            groupBox1.Controls.Add(startStopButton);
            groupBox1.Controls.Add(progressLabel);
            groupBox1.Controls.Add(restartButton);
            groupBox1.Location = new Point(508, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(309, 99);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "Tracker";
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(208, 22);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(95, 23);
            cancelButton.TabIndex = 9;
            cancelButton.Text = "Annuler";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // editCurrentButton
            // 
            editCurrentButton.Location = new Point(107, 22);
            editCurrentButton.Name = "editCurrentButton";
            editCurrentButton.Size = new Size(95, 23);
            editCurrentButton.TabIndex = 8;
            editCurrentButton.Text = "Modifier";
            editCurrentButton.UseVisualStyleBackColor = true;
            editCurrentButton.Click += editCurrentButton_Click;
            // 
            // startStopButton
            // 
            startStopButton.Location = new Point(6, 22);
            startStopButton.Name = "startStopButton";
            startStopButton.Size = new Size(95, 23);
            startStopButton.TabIndex = 7;
            startStopButton.Text = "Démarrer";
            startStopButton.UseVisualStyleBackColor = true;
            startStopButton.Click += startStopButton_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(addButton);
            groupBox2.Controls.Add(editEvent);
            groupBox2.Controls.Add(deleteEvent);
            groupBox2.Controls.Add(syncButton);
            groupBox2.Controls.Add(mergeButton);
            groupBox2.Location = new Point(508, 117);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(309, 83);
            groupBox2.TabIndex = 9;
            groupBox2.TabStop = false;
            groupBox2.Text = "Selection";
            // 
            // configButton
            // 
            configButton.Location = new Point(722, 402);
            configButton.Name = "configButton";
            configButton.Size = new Size(95, 23);
            configButton.TabIndex = 10;
            configButton.Text = "Options";
            configButton.UseVisualStyleBackColor = true;
            configButton.Click += configButton_Click;
            // 
            // addButton
            // 
            addButton.Location = new Point(6, 51);
            addButton.Name = "addButton";
            addButton.Size = new Size(148, 23);
            addButton.TabIndex = 8;
            addButton.Text = "Ajouter";
            addButton.UseVisualStyleBackColor = true;
            addButton.Click += addButton_Click;
            // 
            // TimeTracker
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(825, 433);
            Controls.Add(configButton);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(DebugLabel);
            Controls.Add(treeView1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "TimeTracker";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "Time Tracker";
            FormClosed += Form1_Closed;
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TreeView treeView1;
        public Label DebugLabel;
        private Button deleteEvent;
        private Button editEvent;
        public Label progressLabel;
        private Button mergeButton;
        public Button restartButton;
        private Button syncButton;
        private ImageList icons;
        private GroupBox groupBox1;
        public Button editCurrentButton;
        public Button startStopButton;
        private GroupBox groupBox2;
        public Button cancelButton;
        private Button configButton;
        private Button addButton;
    }
}