using System;
using System.Net;
using System.Text;
using System.Windows.Forms;
using CommonConnection;

namespace TestServeur
{
    public partial class Form1 : Form, IServerListener
    {
        private ModuleServeur moduleServeur;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            moduleServeur = new ModuleServeur(this);
            moduleServeur.StartServer(IPAddress.Parse("192.168.1.12"), 8888);

        }

        public byte[] ProcessDataFromClient(byte[] dataFromClient, int dataSize)
        {
            // throw new NotImplementedException();
            Console.WriteLine(Encoding.ASCII.GetString(dataFromClient));
            return dataFromClient;
        }

        public void MessageForListener(string dataFromClient)
        {
            Console.WriteLine(dataFromClient);
        }

        private byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            moduleServeur.StopModule();
        }
    }
}
