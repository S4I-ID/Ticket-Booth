using NetworkingProtoV3;
using Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;

namespace NetworkingProtoV3
{
    public class ProtoV3Worker : ServerObserver
    {
        private MainServiceInterface server;
        private TcpClient connection;
        private NetworkStream stream;
        private volatile bool connected;

        private Answer answer_ok = ProtoUtils.CreateOkAnswer();
        public ProtoV3Worker(MainServiceInterface server, TcpClient connection)
        {
            this.server = server;
            this.connection = connection;
            try
            {
                stream = connection.GetStream();
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
                    Request request = Request.Parser.ParseDelimitedFrom(stream);
                    Answer answer = HandleRequest(request);
                    if (answer != null)
                    {
                        SendResponse(answer);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
                try
                {
                    Thread.Sleep(200);
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
            Console.WriteLine("Sending answer " + answer);
            lock (stream)
            {
                answer.WriteDelimitedTo(stream);
                stream.Flush();
            }
        }

        public void updateShowList(List<CommonDomain.Show> shows)
        {
            Console.WriteLine("Sending updated show list to client...");
            Answer answer = ProtoUtils.CreateDynamicUpdateShowListAnswer(shows);
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

        public void updateFilteredList(List<CommonDomain.Show> shows)
        {
            Console.WriteLine("Sending updated filtered show list to client...");
            Answer answer = ProtoUtils.CreateDynamicFilteredShowListAnswer(shows);
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

        // //////////////////////////////////////////////////////////////////////

        private Answer HandleRequest(Request request)
        {
            Answer answer = null;
            Request.Types.Type requestType = request.Type;
            if (requestType==Request.Types.Type.Login)
            {
                Console.WriteLine("Received LoginRequest...");
                CommonDomain.User user = ProtoUtils.GetUserFromLogin(request);
                try
                {
                    lock (server)
                    {
                        server.Login(user.GetUsername(), user.GetPassword(), this);
                    }
                    return answer_ok; 
                }
                catch (Exception ex)
                {
                    connected = false;
                    return ProtoUtils.CreateErrorAnswer(ex.Message);
                }
            }

            if (requestType==Request.Types.Type.Logout)
            {
                Console.WriteLine("Received LogoutRequest...");
                string username = ProtoUtils.GetUsernameFromLogout(request);
                try
                {
                    lock (server)
                    {
                        server.Logout(username, this);
                    }
                    connected = false;
                    return answer_ok;
                }
                catch (Exception ex)
                {
                    return ProtoUtils.CreateErrorAnswer(ex.Message);
                }
            }

            if (requestType==Request.Types.Type.AddSale)
            {
                Console.WriteLine("Received AddSaleRequest...");
                CommonDomain.Sale sale = ProtoUtils.GetSaleFromRequest(request);
                try
                {
                    lock (server)
                    {
                        server.AddSaleToShow(sale.GetName(), sale.GetTicketsBought(), sale.GetShowId());
                    }
                    return answer_ok;
                }
                catch (Exception ex)
                {
                    return ProtoUtils.CreateErrorAnswer(ex.Message);
                }
            }

            if (requestType==Request.Types.Type.FullShowList)
            {
                Console.WriteLine("Received FullShowListRequest...");
                try
                {
                    List<CommonDomain.Show> shows;
                    lock (server)
                    {
                        shows = server.GetAllShows();
                    }
                    return ProtoUtils.CreateFullShowListAnswer(shows);
                }
                catch (Exception ex)
                {
                    return ProtoUtils.CreateErrorAnswer(ex.Message);
                }
            }

            if (requestType==Request.Types.Type.FilteredShowList)
            {
                Console.WriteLine("Received FullShowListRequest...");
                DateTime date = ProtoUtils.GetDateFromFilteredRequest(request);
                try
                {
                    List<CommonDomain.Show> filteredShows;
                    lock (server)
                    {
                        filteredShows = server.GetShowsByDate(date);
                    }
                    return ProtoUtils.CreateFilteredShowListAnswer(filteredShows);
                }
                catch (Exception ex)
                {
                    return ProtoUtils.CreateErrorAnswer(ex.Message);
                }
            }
            return answer;
        }
    }
}
