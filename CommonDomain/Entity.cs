using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain
{
    [Serializable]
    public class Entity<TE>
    {
        private TE Id;

        protected Entity() { }
        protected Entity(TE newId)
        {
            this.Id = newId;
        }

        public TE GetId()
        {
            return this.Id;
        }

        public void SetId(TE newId)
        {
            this.Id = newId;
        }
    }
}