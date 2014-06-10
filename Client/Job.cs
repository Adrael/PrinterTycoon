using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.NetworkInformation;

namespace ClientWindow
{
    public class Job
    {
        private int jobID;
        private string userName;
        private DateTime initialisationDate;
        private string[] files;
        private int state;

        public Job(int id)
        {
            setID(id);

            Console.WriteLine("job Created " + id);

            IPAddress ipAddress = IPAddress.Parse("192.168.43.53");
            int port = 3003;
            int bufferSize = 1024;

            TcpClient client = new TcpClient();
            NetworkStream netStream;

            // Connect to server
            try
            {
                client.Connect(new IPEndPoint(ipAddress, port));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            netStream = client.GetStream();

            // Read bytes from image
            byte[] data = File.ReadAllBytes("C:\\Users\\Adrael\\Downloads\\Music\\2\\Avicii - Addicted To You.mp3");

            // Build the package
            byte[] dataLength = BitConverter.GetBytes(data.Length);
            byte[] package = new byte[4 + data.Length];
            dataLength.CopyTo(package, 0);
            data.CopyTo(package, 4);

            // Send to server
            int bytesSent = 0;
            int bytesLeft = package.Length;

            while (bytesLeft > 0)
            {

                int nextPacketSize = (bytesLeft > bufferSize) ? bufferSize : bytesLeft;

                netStream.Write(package, bytesSent, nextPacketSize);
                bytesSent += nextPacketSize;
                bytesLeft -= nextPacketSize;

            }

            // Clean up
            netStream.Close();
            client.Close();
        }

        public void setUser(string user)
        {
            userName = user;
        }

        public string getUser()
        {
            return userName;
        }

        public void setID(int id)
        {
            jobID = id;
        }

        public int getID()
        {
            return jobID;
        }

        public void setInitTime(DateTime time)
        {
            initialisationDate = time;
        }

        public DateTime initiatedAt()
        {
            return initialisationDate;
        }

        public void setFiles(string[] filesArray)
        {
            for (int i = 0; i < filesArray.Length; ++i)
            {
                files[i] = filesArray[i];
            }
        }

        public string[] getFiles()
        {
            return files;
        }

        public void setState(int stater)
        {
            state = stater;
        }

        public int getState()
        {
            return state;
        }
    }
}