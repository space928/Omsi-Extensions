using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiplayer_Server
{
    internal class Client
    {
        public int ClientId {  get; private set; }
        public Client(int clientId) { 
            ClientId = clientId;
        }
    }
}
