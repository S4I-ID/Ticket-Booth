using CommonDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public interface ServerObserver
    {
        void updateShowList(List<Show> shows);
        void updateFilteredList(List<Show> shows);
    }
}
