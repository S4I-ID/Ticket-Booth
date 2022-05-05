using CommonDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Service.Validator
{
    internal class ShowValidator : Validator<Show>
    {
        public void validate(Show entity)
        {
            string errors = "";

            if (entity.GetAvailableSeats() < 0)
                errors += "Seats are invalid!\n";
            if (entity.GetArtistName() == "")
                errors += "Artist name is null!\n";
            if (entity.GetShowName() == "")
                errors += "Show name is null!\n";
            if (entity.GetSoldSeats() < 0)
                errors += "Sold seats are invalid!\n";
            if (entity.GetAddress() == "")
                errors += "Address is invalid!\n";
            if (entity.GetAvailableSeats() < entity.GetSoldSeats())
                errors += "Not enough seats left!\n";

            if (!errors.Equals(""))
                throw new Exception("Invalid show!\n" + errors);
        }
    }
}
