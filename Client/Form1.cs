using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using CommonConnection;

namespace ClientWindow
{
    public partial class Form1 : Form, IClientListener
    {
        private bool _isPrinting = false;
        private ColumnHeader columnHeaderSize;
        private ModuleClient _mc;
        private ColumnHeader columnHeaderId;

        private List<Thread> threads;

        private delegate void launchPrintDelegate();
        private delegate void cancelPrintDelegate();
        private delegate void progressPrintDelegate();

        public Form1()
        {
            InitializeComponent();

            var progressTimer = new System.Timers.Timer(1000);
            progressTimer.Elapsed += progressTimedEvent;
            progressTimer.Enabled = true;

            addButton.Enabled = false;

            threads = new List<Thread>();
        }

        private void progressTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (_isPrinting)
            {
                createThread("progressPrintThread", progressPrint);
            }
        }
        
        private void addButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog {Multiselect = true};

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.FileName != null)
                {
                    var array = new String[3];
                    foreach (var name in dialog.FileNames)
                    {
                        var infos = new FileInfo(name);
                        var fileName = infos.Name;
                        var fileSize = infos.Length / 1024;

                        array[0] = fileName;
                        array[1] = fileSize.ToString();
                        array[2] = "0 %";

                        var item = new ListViewItem(array);
                        filesList.Items.Add(item);
                    }

                    printButton.Enabled = true;
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (_isPrinting)
            {
                createThread("cancelPrintThread", cancelPrint);
            }

            closeAllThreads();

            Close();
        }

        private void closeAllThreads()
        {
            foreach (var thread in threads)
            {
                thread.Join();
//                thread.Abort();
            }

            Console.WriteLine("Closed threads");
        }

        private void filesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            deleteButton.Enabled = true;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {

            foreach (ListViewItem i in filesList.SelectedItems)
            {
                filesList.Items.Remove(i);
            }

            if (filesList.Items.Count <= 0)
            {
                printButton.Enabled = false;
            }

            deleteButton.Enabled = false;
        }

        private void startPrint()
        {
            Invoke((launchPrintDelegate) launchPrint);
        }

        private void cancelPrint()
        {
            Invoke((cancelPrintDelegate) cancelPrinting);
        }

        private void progressPrint()
        {
            Invoke((progressPrintDelegate) progressPrinting);
        }

        private void progressPrinting()
        {
            processAction("progress", true);
        }

        private void cancelPrinting()
        {
            processAction("cancel", true);
        }

        private void processAction(String action, bool byIDOrSize)
        {
            var files = "action=" + action + "&";

            foreach (ListViewItem item in filesList.Items)
            {
                files += item.SubItems[0].Text + "=" + (byIDOrSize ? item.SubItems[3].Text : item.SubItems[1].Text) + "&";
            }

            Console.WriteLine("Send for " + action + ": " + files);

            _mc.SendDataToServer(GetBytes(files));
        }

        private void launchPrint()
        {
            processAction("print", false);
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            deleteButton.Enabled = false;
            printButton.Enabled = false;
            addButton.Enabled = false;
            _isPrinting = true;

            createThread("startPrintThread", startPrint);
        }

        private void createThread(String name, ThreadStart function)
        {
            Console.WriteLine("Creating thread " + name);
            var thread = new Thread(function) { Name = name };
            threads.Add(thread);
            thread.IsBackground = false;
            thread.Start();
        }

        private byte[] GetBytes(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        private string GetString(byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }

        private void networkOptionsLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var networkOptions = new NetworkOptions(this);
            networkOptions.ShowDialog();
            if (networkOptions.IsValidConnection())
            {
                this._mc = networkOptions.GetModuleClient();
                addButton.Enabled = true;
            }
        }

        public void ProcessDataFromServer(byte[] responseFromServer, int dataSize)
        {
            var response = GetString(responseFromServer);
            Console.WriteLine("Response from server: " + response);

            var split = response.Split('&');
            var action = split[0].Split('=')[1];

            for (var i = 1; i < split.Length; ++i)
            {
                var value = split[i].Split('=');
                Console.WriteLine("value = " + value);

                foreach (ListViewItem item in filesList.Items)
                {
                    if (action == "progress")
                    {
                        if (setUpProgression(item, value[0], value[1]))
                        {
                            break;
                        }
                    }

                    else if (action == "print")
                    {
                        Console.WriteLine("value = " + value);
                        if (setUpId(item, value[0], value[1]))
                        {
                            break;
                        }
                    }
                }
            }

        }

        private bool setUpId(ListViewItem item, String name, String value)
        {
            if (item.SubItems[0].Text == name && item.SubItems[3].Text == "")
            {
                item.SubItems[3].Text = value;
                return true;
            }

            return false;
        }

        private bool setUpProgression(ListViewItem item, String id, String value)
        {
            if (item.SubItems[3].Text == id)
            {
                item.SubItems[2].Text = value;
                return true;
            }

            return false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            closeAllThreads();
        }

        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ListView filesList;
        private System.Windows.Forms.Button printButton;
        private System.Windows.Forms.LinkLabel networkOptionsLabel;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderProgression;

        private void InitializeComponent()
        {
            this.addButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.filesList = new System.Windows.Forms.ListView();
            this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderSize = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderProgression = new System.Windows.Forms.ColumnHeader();
            this.printButton = new System.Windows.Forms.Button();
            this.networkOptionsLabel = new System.Windows.Forms.LinkLabel();
            this.columnHeaderId = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Location = new System.Drawing.Point(441, 10);
            this.addButton.Margin = new System.Windows.Forms.Padding(2);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(128, 28);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "Ajouter";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Location = new System.Drawing.Point(441, 43);
            this.deleteButton.Margin = new System.Windows.Forms.Padding(2);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(128, 28);
            this.deleteButton.TabIndex = 2;
            this.deleteButton.Text = "Supprimer";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(441, 425);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(2);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(128, 28);
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
            this.columnHeaderSize,
            this.columnHeaderProgression,
            this.columnHeaderId});
            this.filesList.Location = new System.Drawing.Point(9, 10);
            this.filesList.Margin = new System.Windows.Forms.Padding(2);
            this.filesList.Name = "filesList";
            this.filesList.Size = new System.Drawing.Size(419, 445);
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
            // columnHeaderSize
            // 
            this.columnHeaderSize.Text = "Size (Ko)";
            this.columnHeaderSize.Width = 83;
            // 
            // columnHeaderProgression
            // 
            this.columnHeaderProgression.Text = "Progression";
            this.columnHeaderProgression.Width = 87;
            // 
            // printButton
            // 
            this.printButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.printButton.Enabled = false;
            this.printButton.Location = new System.Drawing.Point(441, 76);
            this.printButton.Margin = new System.Windows.Forms.Padding(2);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(128, 28);
            this.printButton.TabIndex = 3;
            this.printButton.Text = "Imprimer";
            this.printButton.UseVisualStyleBackColor = true;
            this.printButton.Click += new System.EventHandler(this.printButton_Click);
            // 
            // networkOptionsLabel
            // 
            this.networkOptionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.networkOptionsLabel.AutoSize = true;
            this.networkOptionsLabel.Location = new System.Drawing.Point(441, 110);
            this.networkOptionsLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.networkOptionsLabel.Name = "networkOptionsLabel";
            this.networkOptionsLabel.Size = new System.Drawing.Size(126, 13);
            this.networkOptionsLabel.TabIndex = 4;
            this.networkOptionsLabel.TabStop = true;
            this.networkOptionsLabel.Text = "Manage network settings";
            this.networkOptionsLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.networkOptionsLabel_LinkClicked);
            // 
            // columnHeaderId
            // 
            this.columnHeaderId.Text = "#";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 471);
            this.Controls.Add(this.networkOptionsLabel);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.filesList);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.addButton);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(407, 449);
            this.Name = "Form1";
            this.Text = "PrinterTycoon Client";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        
    }
}
