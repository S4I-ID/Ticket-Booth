using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Service.Validator
{
    public interface Validator<T>
    {
        void validate(T entity);
    }
}
