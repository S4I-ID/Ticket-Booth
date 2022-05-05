using CommonDomain;
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
    public class ClientMainService : MainServiceInterface
    {
        private string host;
        private int port;

        private ServerObserver client;
        private NetworkStream stream;

        private IFormatter formatter;
        private TcpClient connection;

        private Queue<Answer> answers;
        private volatile bool finished;
        private EventWaitHandle _waitHandle;

        public ClientMainService(string host, int port)
        {
            this.host = host;
            this.port = port;
            answers = new Queue<Answer>();
        }

        private void InitializeConnection()
        {
            try
            {
                connection = new TcpClient(host, port);
                stream = connection.GetStream();
                formatter = new BinaryFormatter();
                finished = false;
                _waitHandle = new AutoResetEvent(false);
                StartReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void CloseConnection()
        {
            finished = true;
            try
            {
                stream.Close();
                connection.Close();
                _waitHandle.Set();
                client = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void SendRequest(Request request)
        {
            try
            {
                formatter.Serialize(stream, request);
                stream.Flush();
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending object " + ex);
            }
        }

        private void StartReader()
        {
            Thread tw = new Thread(run);
            tw.Start();
        }

        private Answer ReadAnswer()
        {
            Answer answer = null;
            try
            {
                _waitHandle.WaitOne();
                lock (answers)
                {
                    answer = answers.Dequeue();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return answer;
        }

        public virtual void run()
        {
            while(!finished)
            {
                try
                {
                    object answer = formatter.Deserialize(stream);
                    Console.WriteLine("Answer received: " + answer);
                    if (answer is UpdateAnswer)
                    {
                        HandleUpdate((UpdateAnswer)answer);
                    }
                    else
                    {
                        lock (this.answers)
                        {
                            answers.Enqueue((Answer)answer);
                        }
                        _waitHandle.Set();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Reading error " + ex);
                }
                try
                {
                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        private void HandleUpdate(UpdateAnswer update)
        {
            if (update is DynamicFullShowListAnswer)
            {
                DynamicFullShowListAnswer answer = (DynamicFullShowListAnswer)update;
                Console.WriteLine("Received answer: dynamic full show list: " + answer.GetShows());
                try
                {
                    client.updateShowList(answer.GetShows());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }

            if (update is DynamicFilteredShowListAnswer)
            {
                DynamicFilteredShowListAnswer answer = (DynamicFilteredShowListAnswer)update;
                Console.WriteLine("Received answer: dynamic filtered show list " + answer.GetShows());
                try
                {
                    client.updateFilteredList(answer.GetShows());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }
        // /////////////////////////////////////////////////////////////////////
        public void AddSaleToShow(string buyerName, int ticketsBought, int showId)
        {
            SendRequest(new AddSaleRequest(new Sale(buyerName, ticketsBought, showId)));
            Answer answer = ReadAnswer();
            if (answer is OKAnswer)
            {
                Console.WriteLine("Adding show OK");
            }
            else if (answer is ErrorAnswer)
            {
                ErrorAnswer error = (ErrorAnswer)answer;
                throw new Exception(error.GetMessage());
            }
            else
            {
                throw new Exception("Error reading answer for add sale request");
            }
        }

        public List<Show> GetAllShows()
        {
            SendRequest(new FullShowListRequest());
            Answer answer = ReadAnswer();
            if (answer is FullShowListAnswer)
            {
                return ((FullShowListAnswer)answer).GetShows();
            }
            else if (answer is ErrorAnswer)
            {
                ErrorAnswer error = (ErrorAnswer)answer;
                throw new Exception(error.GetMessage());
            }
            else
            {
                throw new Exception("Error reading answer for full show list request");
            }
        }

        public List<Show> GetShowsByDate(DateTime date)
        {
            SendRequest(new FilteredShowListRequest(date));
            Answer answer = ReadAnswer();
            if (answer is FilteredShowListAnswer)
            {
                return ((FilteredShowListAnswer)answer).GetShows();
            }
            else if (answer is ErrorAnswer)
            {
                ErrorAnswer error = (ErrorAnswer)answer;
                throw new Exception(error.GetMessage());
            }
            else
            {
                throw new Exception("Error reading answer for filtered show list request");
            }
        }

        public void Login(string username, string password, ServerObserver client)
        {
            InitializeConnection();
            User user = new User(username, password);
            SendRequest(new LoginRequest(user));
            Answer answer = ReadAnswer();
            if (answer is OKAnswer)
            {
                this.client = client;
                return;
            }
            if (answer is ErrorAnswer)
            {
                ErrorAnswer error = (ErrorAnswer)answer;
                CloseConnection();
                throw new Exception(error.GetMessage());
            }
        }

        public void Logout(string username, ServerObserver client)
        {
            SendRequest(new LogoutRequest(username));
            Answer answer = ReadAnswer();
            CloseConnection();
            if (answer is ErrorAnswer)
            {
                ErrorAnswer error = (ErrorAnswer)answer;
                throw new Exception(error.GetMessage());
            }
        }
    }
}
