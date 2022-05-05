using CommonDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public interface Request { }

    [Serializable]
    public class LoginRequest : Request
    {
        private User user;

        public LoginRequest(User user)
        {
            this.user = user;
        }
        
        public User GetUser()
        {
            return user;
        }
    }

    [Serializable]
    public class LogoutRequest : Request
    {
        private string username;

        public LogoutRequest(string username)
        {
            this.username = username;
        }
        public string GetUsername()
        {
            return username;
        }
    }

    [Serializable]
    public class FullShowListRequest : Request
    {
        public FullShowListRequest() { }
    }

    [Serializable]
    public class FilteredShowListRequest : Request
    {
        DateTime date;
        public FilteredShowListRequest(DateTime date)
        {
            this.date = date;
        }
        public DateTime GetDate()
        {
            return date;
        }
    }

    [Serializable]
    public class AddSaleRequest : Request
    {
        Sale sale;
        public AddSaleRequest(Sale sale)
        {
            this.sale = sale;
        }
        public Sale GetSale()
        {
            return sale;
        }
    }
}
