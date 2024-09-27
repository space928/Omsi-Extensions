using Multiplayer_Client;
using System;
using System.Text;
using Telepathy;

class OMSIClient
{
    static void Main(string[] args)
    {
        Client client = new Client();
        GameClient gameClient = new GameClient();
        client.Connect("127.0.0.1", 1337);


        while (true)
        {
            Telepathy.Message msg;
            while (client.GetNextMessage(out msg))
            {
                switch (msg.eventType)
                {
                    case EventType.Connected:
                        {
                            Console.WriteLine($"Client connected: {msg.connectionId}");
                            byte[] buff = new byte[4];
                            int out_pos = 0;
                            FastBinaryWriter.Write(buff, ref out_pos, OMSIMPMessages.Messages.REQUEST_VERSION);
                            client.Send(buff);
                            byte[] buff2 = new byte[8];
                            out_pos = 0;
                            gameClient.LastPing = new Tuple<int, long, long>(787, DateTime.Now.Ticks, 0);
                            FastBinaryWriter.Write(buff2, ref out_pos, OMSIMPMessages.Messages.PING);
                            FastBinaryWriter.Write(buff2, ref out_pos, 787);
                            client.Send(buff2);
                        }
                        break;
                    case EventType.Data:
                        MessageParser.ParseMessage(msg.data, client, gameClient);
                        break;
                    case EventType.Disconnected:
                        Console.WriteLine($"Client disconnected: {msg.connectionId}");
                        break;
                }
            }
            gameClient.Tick(client);
            System.Threading.Thread.Sleep(33);
        }
        client.Disconnect();
    }
}
