using OmsiHook.Examples.Multiplayer_Server;
using Telepathy;
using Client = OmsiHook.Examples.Multiplayer_Server.Client;

internal class OMSIServer
{

    static void Main(string[] args)
    {
        Dictionary<int, Client>? Clients = new Dictionary<int, Client>();
        Server server = new Server();
        server.Start(1337);

        Console.WriteLine("Server started. Press Ctrl+C to stop...");

        while (true)
        {
            Message msg;
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
