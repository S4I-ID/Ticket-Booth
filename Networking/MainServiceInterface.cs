using CommonDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public interface MainServiceInterface
    {
        void AddSaleToShow(string buyerName, int ticketsBought, int showId);
        List<Show> GetAllShows();
        List<Show> GetShowsByDate(DateTime date);
        void Login(string username, string password, ServerObserver client);
        void Logout(string username, ServerObserver client);
    }
}
