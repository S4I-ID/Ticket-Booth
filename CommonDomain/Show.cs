using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain
{
    [Serializable]
    public class Show : Entity<int>
    {
        private string _artistName;
        private string _showName;
        private string _address;
        private DateTime _time;
        private int _availableSeats;
        private int _soldSeats;

        public Show(string aristName, string showName, string address, DateTime time, int availableSeats, int soldSeats)
        {
            this._artistName = aristName;
            this._showName = showName;
            this._address = address;
            this._time = time;
            this._availableSeats = availableSeats;
            this._soldSeats = soldSeats;
        }

        public string GetArtistName()
        {
            return _artistName;
        }

        public void SetArtistName(string artistName)
        {
            this._artistName = artistName;
        }

        public string GetShowName()
        {
            return _showName;
        }

        public void SetShowName(string showName)
        {
            this._showName = showName;
        }

        public string GetAddress()
        {
            return _address;
        }

        public void SetAddress(string address)
        {
            this._address = address;
        }

        public DateTime GetStartTime()
        {
            return _time;
        }

        public void SetStartTime(DateTime startTime)
        {
            this._time = startTime;
        }

        public int GetAvailableSeats()
        {
            return _availableSeats;
        }

        public void SetAvailableSeats(int availableSeats)
        {
            this._availableSeats = availableSeats;
        }

        public int GetSoldSeats()
        {
            return _soldSeats;
        }

        public void SetSoldSeats(int soldSeats)
        {
            this._soldSeats = soldSeats;
        }
    }
}