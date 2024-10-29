using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeTracker.Forms
{
    public partial class AliasForm : Form
    {
        private readonly ConfigForm ConfigForm;
        private readonly string AliasName;
        private readonly string AliasValue;

        public AliasForm(ConfigForm configForm, string name = "", string value = "")
        {
            this.ConfigForm = configForm;
            this.AliasName = name;
            this.AliasValue = value;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(AliasForm));
            nameLabel = new Label();
            valueLabel = new Label();
            nameTextBox = new TextBox();
            valueTextBox = new TextBox();
            saveButton = new Button();
            cancelButton = new Button();
            SuspendLayout();
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new Point(12, 9);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new Size(37, 15);
            nameLabel.TabIndex = 0;
            nameLabel.Text = "Nom:";
            // 
            // valueLabel
            // 
            valueLabel.AutoSize = true;
            valueLabel.Location = new Point(12, 36);
            valueLabel.Name = "valueLabel";
            valueLabel.Size = new Size(42, 15);
            valueLabel.TabIndex = 1;
            valueLabel.Text = "Valeur:";
            // 
            // nameTextBox
            // 
            nameTextBox.Location = new Point(72, 6);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(172, 23);
            nameTextBox.TabIndex = 2;
            // 
            // valueTextBox
            // 
            valueTextBox.Location = new Point(72, 33);
            valueTextBox.Name = "valueTextBox";
            valueTextBox.Size = new Size(172, 23);
            valueTextBox.TabIndex = 3;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(130, 62);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(114, 23);
            saveButton.TabIndex = 4;
            saveButton.Text = "Valider";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(12, 62);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(112, 23);
            cancelButton.TabIndex = 5;
            cancelButton.Text = "Annuler";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // AliasForm
            // 
            ClientSize = new Size(252, 94);
            ControlBox = false;
            Controls.Add(cancelButton);
            Controls.Add(saveButton);
            Controls.Add(valueTextBox);
            Controls.Add(nameTextBox);
            Controls.Add(valueLabel);
            Controls.Add(nameLabel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "AliasForm";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "Alias";
            Load += AliasForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            ConfigForm.Invoke(() =>
            {
                ConfigForm.AddAlias(nameTextBox.Text, valueTextBox.Text);
            });

            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private Label nameLabel;
        private Label valueLabel;
        private TextBox nameTextBox;
        private TextBox valueTextBox;
        private Button cancelButton;
        private Button saveButton;

        private void AliasForm_Load(object sender, EventArgs e)
        {
            this.nameTextBox.Text = this.AliasName;
            this.valueTextBox.Text = this.AliasValue;
        }
    }
}
