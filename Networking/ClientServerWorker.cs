using CommonDomain;
using Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Networking
{
    public class ClientServerWorker : ServerObserver
    {
        private Answer answer_ok;
        private MainServiceInterface serverService;
        private TcpClient connection;

        private NetworkStream stream;
        private IFormatter formatter;
        private volatile bool connected;

        public ClientServerWorker(MainServiceInterface serverService, TcpClient connection)
        {
            this.answer_ok = new OKAnswer();
            this.serverService = serverService;
            this.connection = connection;
            try
            {
                stream = connection.GetStream();
                formatter = new BinaryFormatter();
                connected = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public virtual void run()
        {
            while (connected)
            {
                try
                {
                    object request = formatter.Deserialize(stream);
                    object answer = HandleRequest((Request)request);
                    if (answer != null)
                    {
                        SendResponse((Answer)answer);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
                try
                {
                    Thread.Sleep(500);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
            try
            {
                stream.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex);
            }
        }

        private void SendResponse(Answer answer)
        {
            Console.WriteLine("Sending response " + answer);
            lock (stream)
            {
                formatter.Serialize(stream, answer);
                stream.Flush();
            }
        }

        public void updateShowList(List<Show> shows)
        {
            Console.WriteLine("Sending updated show list to client...");
            DynamicFullShowListAnswer answer = new DynamicFullShowListAnswer(shows);
            try
            {
                SendResponse(answer);
                Console.WriteLine("Sent show list to client.");
            }
            catch (Exception ex)
            {
                throw new Exception("Sending error: " + ex);
            }
        }

        public void updateFilteredList(List<Show> shows)
        {
            Console.WriteLine("Sending updated filtered show list to client...");
            DynamicFilteredShowListAnswer answer = new DynamicFilteredShowListAnswer(shows);
            try
            {
                SendResponse(answer);
                Console.WriteLine("Sent filtereds show list to client.");
            }
            catch (Exception ex)
            {
                throw new Exception("Sending error: " + ex);
            }
        }
        /// /////////////////////////////////////////////////////////////////////////////
        private Answer HandleRequest(Request request)
        {
            Answer answer = null;
            if (request is LoginRequest)
            {
                Console.WriteLine("Received LoginRequest...");
                LoginRequest loginRequest = (LoginRequest)request;
                User user = loginRequest.GetUser();
                try
                {
                    lock (serverService)
                    {
                        serverService.Login(user.GetUsername(), user.GetPassword(), this);
                    }
                    return answer_ok;
                }
                catch (Exception ex)
                {
                    connected = false;
                    return new ErrorAnswer(ex.Message);
                }
            }

            if (request is LogoutRequest)
            {
                Console.WriteLine("Received LogoutRequest...");
                LogoutRequest logoutRequest = (LogoutRequest)request;
                string username = logoutRequest.GetUsername();
                try
                {
                    lock (serverService)
                    {
                        serverService.Logout(username, this);
                    }
                    connected = false;
                    return answer_ok;
                }
                catch (Exception ex)
                {
                    return new ErrorAnswer(ex.Message);
                }
            }

            if (request is AddSaleRequest)
            {
                Console.WriteLine("Received AddSaleRequest...");
                AddSaleRequest addSaleRequest = (AddSaleRequest)request;
                Sale sale = addSaleRequest.GetSale();
                try
                {
                    lock (serverService)
                    {
                        serverService.AddSaleToShow(sale.GetName(), sale.GetTicketsBought(), sale.GetShowId());
                    }
                    return answer_ok;
                }
                catch (Exception ex)
                {
                    return new ErrorAnswer(ex.Message);
                }
            }

            if (request is FullShowListRequest)
            {
                Console.WriteLine("Received FullShowListRequest...");
                try
                {
                    List<Show> shows;
                    lock (serverService)
                    {
                        shows = serverService.GetAllShows();
                    }
                    return new FullShowListAnswer(shows);
                }
                catch (Exception ex)
                {
                    return new ErrorAnswer(ex.Message);
                }
            }

            if (request is FilteredShowListRequest)
            {
                Console.WriteLine("Received FullShowListRequest...");
                FilteredShowListRequest filteredShowListRequest = (FilteredShowListRequest)request;
                DateTime date = filteredShowListRequest.GetDate();
                try
                {
                    List<Show> filteredShows;
                    lock (serverService)
                    {
                        filteredShows = serverService.GetShowsByDate(date);
                    }
                    return new FilteredShowListAnswer(filteredShows);
                }
                catch (Exception ex)
                {
                    return new ErrorAnswer(ex.Message);
                }
            }

            return answer;
        }
    }
}
