using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.NetworkInformation;
using CommonConnection;

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