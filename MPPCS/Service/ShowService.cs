using CommonDomain;
using Persistence.Repository;
using Persistence.Service.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Service
{
    public class ShowService
    {
        private ShowRepository repository;
        private ShowValidator validator;

        public ShowService(ShowRepository repository)
        {
            this.repository = repository;
            this.validator = new ShowValidator();
        }

        public List<Show> GetAllShows()
        {
            return repository.GetAll();
        }

        public List<Show> GetShowsByDate(DateTime time)
        {
            List<Show> showsOnDate = (List<Show>)repository.GetAll().Where
                (show => show.GetStartTime().Year == time.Year &&
                         show.GetStartTime().Month == time.Month &&
                         show.GetStartTime().Day == time.Day
                ).ToList<Show>();

            return showsOnDate;
        }

        public void AddShow(Show show)
        {
            validator.validate(show);
            repository.Add(show);
        }

        public Show FindShow(int showId)
        {
            return repository.Find(showId);
        }

        public Show UpdateShow(Show newShow)
        {
            validator.validate(newShow);
            return repository.Update(newShow);
        }
    }
}