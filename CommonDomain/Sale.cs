using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain
{
    [Serializable]
    public class Sale : Entity<int>
    {
        private string _buyerName;
        private int _ticketsBought;
        private int _showId;

        public Sale(string name, int tickets, int showid)
        {
            this._buyerName = name;
            this._ticketsBought = tickets;
            this._showId = showid;
        }

        public string GetName()
        {
            return this._buyerName;
        }

        public int GetTicketsBought()
        {
            return this._ticketsBought;
        }

        public int GetShowId()
        {
            return this._showId;
        }

        public void SetName(string newName)
        {
            this._buyerName = newName;
        }

        public void SetTicketsBought(int newTicketsBought)
        {
            this._ticketsBought = newTicketsBought;
        }

        public void SetShowId(int newShowId)
        {
            this._showId = newShowId;
        }

    }
}