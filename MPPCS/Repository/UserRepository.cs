using CommonDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public interface UserRepository : Repository <User>
    {
        string GetPasswordOfUser (string username);
    }
}
