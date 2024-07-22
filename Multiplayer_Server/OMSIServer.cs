using Multiplayer_Server;
using System;
using System.Linq;
using Telepathy;
using Client = Multiplayer_Server.Client;

internal class OMSIServer
{

    static void Main(string[] args)
    {
        Dictionary<int, Client>? Clients = new Dictionary<int, Client>();
        Telepathy.Server server = new Telepathy.Server();
        server.Start(1337);

        Console.WriteLine("Server started. Press Ctrl+C to stop...");

        while (true)
        {
            Telepathy.Message msg;
            while (server.GetNextMessage(out msg))
            {
                switch (msg.eventType)
                {
                    case EventType.Connected:
                        Clients.Add(msg.connectionId, new Client(msg.connectionId));
                        Console.WriteLine($"Client connected: {server.GetClientAddress(msg.connectionId)}");
                        break;
                    case EventType.Data:
                        MessageParser.ParseMessage(msg.data, Clients[msg.connectionId], server);
                        break;
                    case EventType.Disconnected:
                        Console.WriteLine($"Client disconnected: {Clients[msg.connectionId].ClientId}");
                        Clients.Remove(msg.connectionId);
                        break;
                }
            }
            System.Threading.Thread.Sleep(10);
        }

        server.Stop();
    }
}
