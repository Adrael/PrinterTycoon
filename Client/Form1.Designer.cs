namespace ClientWindow
{
    partial class Form1
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
            this.addButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.filesList = new System.Windows.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderProgression = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.printButton = new System.Windows.Forms.Button();
            this.networkOptionsLabel = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Location = new System.Drawing.Point(381, 12);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(170, 35);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "Ajouter";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Location = new System.Drawing.Point(381, 53);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(170, 35);
            this.deleteButton.TabIndex = 2;
            this.deleteButton.Text = "Supprimer";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(381, 449);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(170, 35);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Annuler";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // filesList
            // 
            this.filesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderProgression});
            this.filesList.Location = new System.Drawing.Point(12, 12);
            this.filesList.Name = "filesList";
            this.filesList.Size = new System.Drawing.Size(350, 473);
            this.filesList.TabIndex = 4;
            this.filesList.UseCompatibleStateImageBehavior = false;
            this.filesList.View = System.Windows.Forms.View.Details;
            this.filesList.SelectedIndexChanged += new System.EventHandler(this.filesList_SelectedIndexChanged);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 182;
            // 
            // columnHeaderProgression
            // 
            this.columnHeaderProgression.Text = "Progression";
            this.columnHeaderProgression.Width = 133;
            // 
            // printButton
            // 
            this.printButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.printButton.Enabled = false;
            this.printButton.Location = new System.Drawing.Point(381, 94);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(170, 35);
            this.printButton.TabIndex = 3;
            this.printButton.Text = "Imprimer";
            this.printButton.UseVisualStyleBackColor = true;
            this.printButton.Click += new System.EventHandler(this.printButton_Click);
            // 
            // networkOptionsLabel
            // 
            this.networkOptionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.networkOptionsLabel.AutoSize = true;
            this.networkOptionsLabel.Location = new System.Drawing.Point(382, 136);
            this.networkOptionsLabel.Name = "networkOptionsLabel";
            this.networkOptionsLabel.Size = new System.Drawing.Size(165, 17);
            this.networkOptionsLabel.TabIndex = 4;
            this.networkOptionsLabel.TabStop = true;
            this.networkOptionsLabel.Text = "Manage network settings";
            this.networkOptionsLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.networkOptionsLabel_LinkClicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 497);
            this.Controls.Add(this.networkOptionsLabel);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.filesList);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.addButton);
            this.MinimumSize = new System.Drawing.Size(537, 544);
            this.Name = "Form1";
            this.Text = "PrinterTycoon Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ListView filesList;
        private System.Windows.Forms.Button printButton;
        private System.Windows.Forms.LinkLabel networkOptionsLabel;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderProgression;
    }
}

