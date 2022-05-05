using Persistence.Service;
using CommonDomain;
using System;
using System.Collections.Generic;
using Networking;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class ServerService : MainServiceInterface
    {
        private SaleService saleService;
        private ShowService showService;
        private UserService userService;
        private readonly IDictionary<string, ServerObserver> loggedClients;

        private void notifySaleAdded()
        {
            foreach (var client in loggedClients)
            {
                if (client.Value!=null)
                {
                    Console.WriteLine("Notifying [" + client.Key + "]");
                    client.Value.updateShowList(GetAllShows());
                }
            }
        }

        public ServerService (SaleService saleService, ShowService showService, UserService userService)
        {
            this.saleService = saleService;
            this.showService = showService;
            this.userService = userService;
            loggedClients = new Dictionary<string, ServerObserver>();
        }

        public void AddSaleToShow(string buyerName, int ticketsBought, int showId)
        {
            Show show = showService.FindShow(showId);
            Show updatedShow = show;
            updatedShow.SetSoldSeats(show.GetSoldSeats()+ticketsBought);
            showService.UpdateShow(updatedShow);
            try
            {
                saleService.AddSale(buyerName,ticketsBought,showId);
                notifySaleAdded();
            }
            catch (Exception ex)
            {
                showService.UpdateShow(show);
                throw new Exception(ex.Message);
            }
        }

        public List<Show> GetAllShows()
        {
            return showService.GetAllShows();
        }

        public List<Show> GetShowsByDate(DateTime date)
        {
            return showService.GetShowsByDate(date);
        }

        public void Login(string username, string password, ServerObserver client)
        {
            int login = userService.Login(new User(username, password));
            if (login == 1)
            {
                if (loggedClients.ContainsKey(username) == true)
                    throw new Exception("User " + username + " already logged in.");
                loggedClients.Add(username, client);
            }
        }

        public void Logout(string username, ServerObserver client)
        {
            ServerObserver localClient;
            if (!loggedClients.TryGetValue(username, out localClient))
                throw new Exception("User " + username + " is not logged in.");
            loggedClients.Remove(username);
            if (localClient==null)
                throw new Exception("User " + username + " is not logged in.");
        }
    }
}
