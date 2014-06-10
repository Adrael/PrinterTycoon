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

namespace ClientWindow
{
    public partial class NetworkOptions : Form
    {
        private TcpClient _clientSocket;
        private bool _validConnection = false;

        public NetworkOptions()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.BackColor = Color.Green;

            IPAddress ip;
            if (textBox1.Text != "" && IPAddress.TryParse(textBox1.Text, out ip))
            {
                //valid ip
                _clientSocket = new TcpClient();

                try
                {
                    _clientSocket.Connect(ip, Convert.ToInt32(this.textBox3.Text));
                }
                catch (Exception exception)
                {
                    panel1.BackColor = Color.Red;
                    return;
                }

                _validConnection = true;
                Close();
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

        public TcpClient GetClientSocket()
        {
            return this._clientSocket;
        }
    }
}
