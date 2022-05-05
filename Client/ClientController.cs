using Networking;
using CommonDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class ClientController : ServerObserver
    {
        public event EventHandler<ClientEventArgs> updateEvent;
        private readonly MainServiceInterface clientService;
        private string username;

        public ClientController(MainServiceInterface clientService)
        {
            this.clientService = clientService;
            username = null;
        }

        public int Login(string username, string password)
        {
            clientService.Login(username, password, this);
            Console.WriteLine("Logged in as " + username);
            this.username = username;
            return 1;
        }

        public void Logout()
        {
            Console.WriteLine("Logging out...");
            clientService.Logout(username, this);
            username= null;
        }

        public void AddSaleToShow(string buyer, int ticketsbought, int showid)
        {
            clientService.AddSaleToShow(buyer, ticketsbought, showid);
            Console.WriteLine("Added sales to show");
        }

        public List<Show> GetAllShows()
        {
            return clientService.GetAllShows();
        }

        public List<Show> GetShowsByDate(DateTime date)
        {
            return clientService.GetShowsByDate(date);
        }

        public void updateShowList(List<Show> shows)
        {
            ClientEventArgs args = new ClientEventArgs(ClientEvent.UpdateShowList, shows);
            Console.WriteLine("Received dynamic list update");
            OnUserEvent(args);
        }

        public void updateFilteredList(List<Show> shows)
        {
            ClientEventArgs args = new ClientEventArgs(ClientEvent.UpdateFilteredList, shows);
            Console.WriteLine("Received dynamic filtered list update");
            OnUserEvent(args);
        }

        protected virtual void OnUserEvent(ClientEventArgs e)
        {
            if (updateEvent == null) return;
            updateEvent(this, e);
            Console.WriteLine("Update event called.");
        }
    }
}
