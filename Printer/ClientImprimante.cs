using System;
using System.Drawing;
using System.Windows.Forms;
using CommonConnection;

namespace Printer
{
    public partial class ClientImprimante : Form, IClientListener, IServerListener
    {
        private bool _online;
        private Printer _printer;
        private ModuleClient _moduleClient;

        public ClientImprimante()
        {
            InitializeComponent();
            _moduleClient = new ModuleClient(this);
            _printer = new Printer();
            ChangePrinterPanel();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _online = !_online;
            ChangePrinterPanel();
        }

        private void ChangePrinterPanel()
        {
            if (_online)
            {
                panel1.BackColor = checkBox1.Checked ? Color.Orange : Color.LawnGreen;
                button1.Text = "Eteindre";
            }
            else
            {
                panel1.BackColor = Color.Red;
                button1.Text = "Allumer";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ChangePrinterPanel();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OptionsImprimante options = new OptionsImprimante(this);
            
            options.ShowDialog();
            if (options.IsPrinterCreated())
            {
                _printer = options.GetPrinter();
            }
        }

        public byte[] ProcessDataFromServer(byte[] responseFromServer, int dataSize)
        {
            throw new NotImplementedException();
        }

        public byte[] ProcessDataFromClient(byte[] dataFromClient, int dataSize)
        {
            throw new NotImplementedException();
        }

        public void MessageForListener(string dataFromClient)
        {
            throw new NotImplementedException();
        }
    }
}
