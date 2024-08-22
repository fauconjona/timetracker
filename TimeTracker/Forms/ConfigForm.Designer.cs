namespace TimeTracker.Forms
{
    partial class ConfigForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigForm));
            groupBox1 = new GroupBox();
            jiraProjectTextBox = new TextBox();
            jiraProjectLabel = new Label();
            jiraTokenTextBox = new TextBox();
            jiraTokenLabel = new Label();
            jiraLoginTextBox = new TextBox();
            jiraLoginLabel = new Label();
            jiraUrlTextBox = new TextBox();
            jiraUrlLabel = new Label();
            groupBox2 = new GroupBox();
            cancelShortcutTextBox = new TextBox();
            cancelShortchutLabel = new Label();
            editShortcutTextBox = new TextBox();
            startShortcutTextBox = new TextBox();
            editShortcutLabel = new Label();
            startShortcutLabel = new Label();
            saveButton = new Button();
            cancelButton = new Button();
            groupBox3 = new GroupBox();
            sessionCheckBox = new CheckBox();
            autoStartCheckBox = new CheckBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(jiraProjectTextBox);
            groupBox1.Controls.Add(jiraProjectLabel);
            groupBox1.Controls.Add(jiraTokenTextBox);
            groupBox1.Controls.Add(jiraTokenLabel);
            groupBox1.Controls.Add(jiraLoginTextBox);
            groupBox1.Controls.Add(jiraLoginLabel);
            groupBox1.Controls.Add(jiraUrlTextBox);
            groupBox1.Controls.Add(jiraUrlLabel);
            groupBox1.Location = new Point(12, 79);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(478, 130);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Jira";
            // 
            // jiraProjectTextBox
            // 
            jiraProjectTextBox.Location = new Point(65, 97);
            jiraProjectTextBox.Name = "jiraProjectTextBox";
            jiraProjectTextBox.Size = new Size(407, 23);
            jiraProjectTextBox.TabIndex = 7;
            // 
            // jiraProjectLabel
            // 
            jiraProjectLabel.AutoSize = true;
            jiraProjectLabel.Location = new Point(6, 105);
            jiraProjectLabel.Name = "jiraProjectLabel";
            jiraProjectLabel.Size = new Size(41, 15);
            jiraProjectLabel.TabIndex = 6;
            jiraProjectLabel.Text = "Projet:";
            // 
            // jiraTokenTextBox
            // 
            jiraTokenTextBox.Location = new Point(65, 68);
            jiraTokenTextBox.Name = "jiraTokenTextBox";
            jiraTokenTextBox.Size = new Size(407, 23);
            jiraTokenTextBox.TabIndex = 5;
            // 
            // jiraTokenLabel
            // 
            jiraTokenLabel.AutoSize = true;
            jiraTokenLabel.Location = new Point(6, 76);
            jiraTokenLabel.Name = "jiraTokenLabel";
            jiraTokenLabel.Size = new Size(41, 15);
            jiraTokenLabel.TabIndex = 4;
            jiraTokenLabel.Text = "Token:";
            // 
            // jiraLoginTextBox
            // 
            jiraLoginTextBox.Location = new Point(65, 39);
            jiraLoginTextBox.Name = "jiraLoginTextBox";
            jiraLoginTextBox.Size = new Size(407, 23);
            jiraLoginTextBox.TabIndex = 3;
            // 
            // jiraLoginLabel
            // 
            jiraLoginLabel.AutoSize = true;
            jiraLoginLabel.Location = new Point(6, 47);
            jiraLoginLabel.Name = "jiraLoginLabel";
            jiraLoginLabel.Size = new Size(40, 15);
            jiraLoginLabel.TabIndex = 2;
            jiraLoginLabel.Text = "Login:";
            // 
            // jiraUrlTextBox
            // 
            jiraUrlTextBox.Location = new Point(65, 11);
            jiraUrlTextBox.Name = "jiraUrlTextBox";
            jiraUrlTextBox.Size = new Size(407, 23);
            jiraUrlTextBox.TabIndex = 1;
            // 
            // jiraUrlLabel
            // 
            jiraUrlLabel.AutoSize = true;
            jiraUrlLabel.Location = new Point(6, 19);
            jiraUrlLabel.Name = "jiraUrlLabel";
            jiraUrlLabel.Size = new Size(25, 15);
            jiraUrlLabel.TabIndex = 0;
            jiraUrlLabel.Text = "Url:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(cancelShortcutTextBox);
            groupBox2.Controls.Add(cancelShortchutLabel);
            groupBox2.Controls.Add(editShortcutTextBox);
            groupBox2.Controls.Add(startShortcutTextBox);
            groupBox2.Controls.Add(editShortcutLabel);
            groupBox2.Controls.Add(startShortcutLabel);
            groupBox2.Location = new Point(496, 79);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(212, 130);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Raccourcis clavier";
            // 
            // cancelShortcutTextBox
            // 
            cancelShortcutTextBox.Location = new Point(98, 68);
            cancelShortcutTextBox.Name = "cancelShortcutTextBox";
            cancelShortcutTextBox.Size = new Size(108, 23);
            cancelShortcutTextBox.TabIndex = 5;
            // 
            // cancelShortchutLabel
            // 
            cancelShortchutLabel.AutoSize = true;
            cancelShortchutLabel.Location = new Point(6, 76);
            cancelShortchutLabel.Name = "cancelShortchutLabel";
            cancelShortchutLabel.Size = new Size(52, 15);
            cancelShortchutLabel.TabIndex = 4;
            cancelShortchutLabel.Text = "Annuler:";
            // 
            // editShortcutTextBox
            // 
            editShortcutTextBox.Location = new Point(98, 39);
            editShortcutTextBox.Name = "editShortcutTextBox";
            editShortcutTextBox.Size = new Size(108, 23);
            editShortcutTextBox.TabIndex = 3;
            // 
            // startShortcutTextBox
            // 
            startShortcutTextBox.Location = new Point(98, 11);
            startShortcutTextBox.Name = "startShortcutTextBox";
            startShortcutTextBox.Size = new Size(108, 23);
            startShortcutTextBox.TabIndex = 2;
            // 
            // editShortcutLabel
            // 
            editShortcutLabel.AutoSize = true;
            editShortcutLabel.Location = new Point(6, 47);
            editShortcutLabel.Name = "editShortcutLabel";
            editShortcutLabel.Size = new Size(55, 15);
            editShortcutLabel.TabIndex = 1;
            editShortcutLabel.Text = "Modifier:";
            // 
            // startShortcutLabel
            // 
            startShortcutLabel.AutoSize = true;
            startShortcutLabel.Location = new Point(6, 19);
            startShortcutLabel.Name = "startShortcutLabel";
            startShortcutLabel.Size = new Size(63, 15);
            startShortcutLabel.TabIndex = 0;
            startShortcutLabel.Text = "Start/Stop:";
            // 
            // saveButton
            // 
            saveButton.Location = new Point(549, 215);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(159, 23);
            saveButton.TabIndex = 2;
            saveButton.Text = "Enregistrer";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(12, 215);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(159, 23);
            cancelButton.TabIndex = 3;
            cancelButton.Text = "Annuler";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(sessionCheckBox);
            groupBox3.Controls.Add(autoStartCheckBox);
            groupBox3.Location = new Point(12, 7);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(690, 53);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
            groupBox3.Text = "Tracker";
            // 
            // sessionCheckBox
            // 
            sessionCheckBox.AutoSize = true;
            sessionCheckBox.Location = new Point(397, 22);
            sessionCheckBox.Name = "sessionCheckBox";
            sessionCheckBox.Size = new Size(287, 19);
            sessionCheckBox.TabIndex = 1;
            sessionCheckBox.Text = "Détecter les pauses avec les fermetures de session";
            sessionCheckBox.UseVisualStyleBackColor = true;
            // 
            // autoStartCheckBox
            // 
            autoStartCheckBox.AutoSize = true;
            autoStartCheckBox.Location = new Point(6, 22);
            autoStartCheckBox.Name = "autoStartCheckBox";
            autoStartCheckBox.Size = new Size(228, 19);
            autoStartCheckBox.TabIndex = 0;
            autoStartCheckBox.Text = "Démarrer les tâches automatiquement";
            autoStartCheckBox.UseVisualStyleBackColor = true;
            // 
            // ConfigForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(720, 250);
            ControlBox = false;
            Controls.Add(groupBox3);
            Controls.Add(cancelButton);
            Controls.Add(saveButton);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "ConfigForm";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "Configuration";
            Load += ConfigForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private TextBox jiraLoginTextBox;
        private Label jiraLoginLabel;
        private TextBox jiraUrlTextBox;
        private Label jiraUrlLabel;
        private Label jiraTokenLabel;
        private TextBox jiraTokenTextBox;
        private TextBox editShortcutTextBox;
        private TextBox startShortcutTextBox;
        private Label editShortcutLabel;
        private Label startShortcutLabel;
        private Button saveButton;
        private TextBox jiraProjectTextBox;
        private Label jiraProjectLabel;
        private Button cancelButton;
        private TextBox cancelShortcutTextBox;
        private Label cancelShortchutLabel;
        private GroupBox groupBox3;
        private CheckBox sessionCheckBox;
        private CheckBox autoStartCheckBox;
    }
}