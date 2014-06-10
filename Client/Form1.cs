using System;
using System.Net.Sockets;
using System.Windows.Forms;

namespace ClientWindow
{
    public partial class Form1 : Form
    {
        private TcpClient _clientSocket;

        public Form1()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog {Multiselect = true};

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.FileName != "")
                {
                    foreach (var name in dialog.FileNames)
                    {
                        filesList.Items.Add(System.IO.Path.GetFileName(name));
                    }

                    printButton.Enabled = true;
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
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

        private void printButton_Click(object sender, EventArgs e)
        {
            var job = new Job(42);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var networkOptions = new NetworkOptions();
            networkOptions.ShowDialog();
            if (networkOptions.IsValidConnection())
            {
                this._clientSocket = networkOptions.GetClientSocket();
            }
        }
    }
}
