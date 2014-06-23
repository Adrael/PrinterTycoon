using System;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientWindow;
using CommonConnection;

namespace Server
{
    public partial class Server : Form, IServerListener
    {
        private ModuleServeur _interfaceClients;
        private ModuleServeur _interfaceImprimante;
        private IPAddress _adresse;

        public Server()
        {
            InitializeComponent();
            _adresse = IPAddress.Parse("192.168.1.12");
            _interfaceClients = new ModuleServeur(this);
            _interfaceImprimante = new ModuleServeur(this);
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void Server_Load(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void buttonImprimante_Click(object sender, EventArgs e)
        {
            try
            {
                int portImprimante = Convert.ToInt32(textPortImprimante.Text);
                demarrerInterfaceImprimante(portImprimante);
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void buttonClient_Click(object sender, EventArgs e)
        {
            try
            {
                int portClient = Convert.ToInt32(textPortClient.Text);
                demarrerInterfaceClient(portClient);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void demarrerInterfaceClient(int portClient)
        {
            indicateurClient.BackColor = Color.Orange;
            labelPortClient.Text = portClient.ToString();

            if (_interfaceImprimante.IsRunning())
            {
                _interfaceImprimante.StopModule();
            }

            _interfaceClients = new ModuleServeur(this);
            _interfaceClients.StartServer(_adresse, portClient);
            indicateurClient.BackColor = Color.Green;
        }

        private void demarrerInterfaceImprimante(int portImprimante)
        {
            indicateurImprimante.BackColor = Color.Orange;
            labelPortImprimante.Text = portImprimante.ToString();

            if (_interfaceImprimante.IsRunning())
            {
                _interfaceImprimante.StopModule();
            }

            _interfaceImprimante = new ModuleServeur(this);
            _interfaceImprimante.StartServer(_adresse, portImprimante);
            indicateurImprimante.BackColor = Color.Green;
        }

        public byte[] ProcessDataFromClient(byte[] dataFromClient, int dataSize)
        {
            throw new NotImplementedException();
        }

        public void MessageForListener(string dataFromClient)
        {
            Console.WriteLine("message : " + dataFromClient);
        }

        private void Server_FormClosed(object sender, FormClosedEventArgs e)
        {
            _interfaceClients.StopModule();

            // if (_interfaceImprimante.isRunning())
            //    _interfaceImprimante.StopModule();


        }
    }
}
