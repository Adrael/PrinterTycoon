using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonConnection;

namespace ClientWindow
{
    public partial class NetworkOptions : Form
    {
        private TcpClient _clientSocket;
        private bool _validConnection = false;
        private ModuleClient _mc;

        public NetworkOptions(IClientListener icl)
        {
            InitializeComponent();
            this._mc = new ModuleClient(icl);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.BackColor = Color.Orange;

            IPAddress ip;
            if (textBox1.Text != "" && IPAddress.TryParse(textBox1.Text, out ip))
            {
                if (_mc.TryConnectWith(ip, int.Parse(textBox3.Text)))
                {
                    panel1.BackColor = Color.Green;
                    _validConnection = true;
                }
                else
                {
                    panel1.BackColor = Color.Red;
                }

            }
            else
            {
                Console.WriteLine("[Debug] Wrong IP address received: '" + textBox1.Text + "'");
                panel1.BackColor = Color.Red;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public bool IsValidConnection()
        {
            return this._validConnection;
        }

        public ModuleClient GetModuleClient()
        {
            return this._mc;
        }
    }
}
