using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CommonConnection;

namespace Printer
{
    public partial class OptionsImprimante : Form
    {
        private Printer _printer;
        private bool _formValid = false;
        private TcpListener _serverSocket;
        private int port;
        private IClientListener _clientListener;

        public OptionsImprimante(IClientListener clientListener)
        {
            InitializeComponent();
            _clientListener = clientListener;
            _printer = new Printer();
            port = 2000 + new Random().Next(6000);
            IPHostEntry iphostentry = Dns.GetHostEntry(Dns.GetHostName());
            _serverSocket = new TcpListener(iphostentry.AddressList[0], port);
            _serverSocket.Start();
            new Thread(WaitForClientToConnect).Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _printer.setName(textBox1.Text);
            _printer.setSpeed(trackBar1.Value);

            _formValid = true;
            Hide();
        }

        public bool IsPrinterCreated()
        {
            return _formValid;
        }

        public Printer GetPrinter()
        {
            return _printer;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            CheckIfPrinterIsValid();
        }

        private void CheckIfPrinterIsValid()
        {
            bool valid = true;

            String newPrinterName = textBox1.Text;

            if (newPrinterName.Length < 4)
                valid = false;

            button1.Enabled = valid;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            label1.Text = trackBar1.Value + "";
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // créer l'adresse ip du serveur
            IPAddress ipServeurImpression = IPAddress.Parse(textBox5.Text);
            int portServeur = int.Parse(textBox4.Text);
            
            ModuleClient moduleClient = new ModuleClient(_clientListener);
            if (moduleClient.TryConnectWith(ipServeurImpression, portServeur))
            {
                panel2.BackColor = Color.Orange;
                String adresse = IpToString(ipServeurImpression);
                byte[] message = Encoding.ASCII.GetBytes("action=nouvelle imprimante;adresse=" + adresse + ";port=" + portServeur + ";");
                moduleClient.SendDataToServer(message);
            }
            else
            {
                panel2.BackColor = Color.Red;
            }

        }

        private static String IpToString(IPAddress ipServeurImpression)
        {
            byte[] ip = ipServeurImpression.GetAddressBytes();
            String adresse = "";
            foreach (byte octet in ip)
            {
                if (adresse.Length > 0)
                    adresse += ".";
                adresse += octet;
            }
            return adresse;
        }

        private void WaitForClientToConnect()
        {
            while (true)
            {
                TcpClient newClient = _serverSocket.AcceptTcpClient();
                MessageBox.Show("le serveur vient de se connecter!");
                new Thread(() => WaitForClientRequest(newClient)).Start();
            }
        }

        private void WaitForClientRequest(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] messageFromClient = new byte[2048];
            int byteRead;

            while (true)
            {
                byteRead = 0;
                try
                {
                    byteRead = stream.Read(messageFromClient, 0, 2048);
                    if (byteRead == 0)
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    break;
                }

                ASCIIEncoding encoder = new ASCIIEncoding();
                String textFromClient = encoder.GetString(messageFromClient, 0, byteRead);
                Console.WriteLine("imprimante reçoit: '" + textFromClient + "'");

                // traitement des demandes du serveur pour l'imprimante.

                try
                {
                    byte[] response = encoder.GetBytes(Convert.ToString("réponse pour le serveur"));
                    stream.Write(response, 0, response.Length);
                    stream.Flush();
                }
                catch (Exception)
                {
                    Console.WriteLine("problème");
                }
            }
            client.Close();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }
    }
}
