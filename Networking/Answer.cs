using CommonDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public interface Answer { }
    public interface UpdateAnswer : Answer{ }

    [Serializable]
    public class OKAnswer : Answer { }

    [Serializable]
    public class ErrorAnswer : Answer
    {
        private string message;
        public ErrorAnswer(string message)
        {
            this.message = message;
        }
        public string GetMessage()
        {
            return message;
        }
    }

    [Serializable]
    public class FullShowListAnswer : Answer
    {
        private List<Show> shows;
        public FullShowListAnswer(List<Show> shows)
        {
            this.shows = shows;
        }
        public List<Show> GetShows()
        {
            return shows;
        }
    }

    [Serializable]
    public class DynamicFullShowListAnswer : UpdateAnswer
    {
        private List<Show> shows;
        public DynamicFullShowListAnswer(List<Show> shows)
        {
            this.shows = shows;
        }
        public List<Show> GetShows()
        {
            return shows;
        }
    }

    [Serializable]
    public class FilteredShowListAnswer : Answer
    {
        private List<Show> shows;
        public FilteredShowListAnswer(List<Show> shows)
        {
            this.shows = shows;
        }
        public List<Show> GetShows()
        {
            return shows;
        }
    }

    public class DynamicFilteredShowListAnswer : UpdateAnswer
    {
        private List<Show> shows;
        public DynamicFilteredShowListAnswer(List<Show> shows)
        {
            this.shows = shows;
        }
        public List<Show> GetShows()
        {
            return shows;
        }
    }
}
