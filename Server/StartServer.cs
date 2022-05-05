using Networking;
using System;
using System.Net.Sockets;
using System.Threading;
using Persistence.Repository;
using Persistence.Service;
using NetworkingProtoV3;

namespace Server
{
    class StartServer
    {
        static void Main(string[] args)
        {
            SaleRepository saleRepository = new SaleDatabaseRepository(Properties.Settings.Default.DbPath);
            ShowRepository showRepository = new ShowDatabaseRepository(Properties.Settings.Default.DbPath);
            UserRepository userRepository = new UserORMRepository(Properties.Settings.Default.DbPath);
            SaleService saleService = new SaleService(saleRepository);
            ShowService showService = new ShowService(showRepository);
            UserService userService = new UserService(userRepository);

            MainServiceInterface serverService = new ServerService(saleService, showService, userService);
            string serverHost = Properties.Settings.Default.ServerIP;
            int serverPort = Properties.Settings.Default.ServerPort;

            //Server server = new Server(serverService, serverHost, serverPort);
            ProtoV3Server server = new ProtoV3Server(serverService, serverHost, serverPort);
            server.Start();
        }
    }
    
    public class Server : ConcurrentServer
    {
        private MainServiceInterface server;
        private ClientServerWorker worker;
        public Server(MainServiceInterface server, string host, int port) : base(host, port)
        {
            this.server = server;
            Console.WriteLine("Booting Server...");
        }
        protected override Thread createWorker(TcpClient client)
        {
            worker = new ClientServerWorker(server, client);
            return new Thread(new ThreadStart(worker.run));
        }
    }

    public class ProtoV3Server : ConcurrentServer
    {
        private MainServiceInterface server;
        private ProtoV3Worker worker;
        public ProtoV3Server(MainServiceInterface server, string host, int port) : base(host,port)
        {
            this.server = server;
            Console.WriteLine("Booting ProtoV3Server...");
        }

        protected override Thread createWorker(TcpClient client)
        {
            worker = new ProtoV3Worker(server, client);
            return new Thread(new ThreadStart(worker.run));
        }
    }
}