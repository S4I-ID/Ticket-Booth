using CommonDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public interface Repository<E> where E : Entity<int>
    {
        E Find(int id);
        List<E> GetAll();
        void Add(E entity);
        E Delete(int id);
        E Update(E entity);
        int Size();
    }
}
