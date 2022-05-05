using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public enum ClientEvent
    {
        UpdateShowList,UpdateFilteredList
    }

    public class ClientEventArgs : EventArgs
    {
        private readonly ClientEvent clientEvent;
        private readonly Object data;

        public ClientEventArgs(ClientEvent clientEvent, Object data)
        {
            this.clientEvent = clientEvent;
            this.data = data;
        }

        public ClientEvent GetEventType()
        {
            return clientEvent;
        }

        public object GetData()
        {
            return data;
        }
    }
}
