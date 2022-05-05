using Networking;
using CommonDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string host = Properties.Settings.Default.ServerHost;
            int port = Properties.Settings.Default.ServerPort;

            MainServiceInterface clientService = new ClientMainService(host,port);
            ClientController ctrl = new ClientController(clientService);
            Form1 loginWindow = new Form1(ctrl);
            Application.Run(loginWindow);
        }
    }
}
