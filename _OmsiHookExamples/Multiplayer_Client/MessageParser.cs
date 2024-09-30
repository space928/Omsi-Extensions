namespace OmsiHook.Examples.Multiplayer_Client
{
    internal class MessageParser
    {
        readonly static int MAJOR_VERSION = 1;
        readonly static int MINOR_VERSION = 0;
        public static void ParseMessage(byte[] message, Telepathy.Client client, GameClient gameClient)
        {
            message.AsSpan<byte>(0, message.Length);
            int parse_pos = 0;
            switch ((OMSIMPMessages.Messages)FastBinaryReader.ReadI32(message, ref parse_pos))
            {
                case OMSIMPMessages.Messages.PING:
                    {
                        byte[] buff = new byte[8];
                        int out_pos = 0;
                        FastBinaryWriter.Write(buff, ref out_pos, OMSIMPMessages.Messages.PONG);
                        FastBinaryWriter.Write(buff, ref out_pos, FastBinaryReader.ReadI32(message, ref parse_pos));
                        client.Send(buff);
                    }
                    break;
                case OMSIMPMessages.Messages.PONG:
                    {
                        if (gameClient.LastPing.Item1 == FastBinaryReader.ReadI32(message, ref parse_pos))
                        {
                            gameClient.LastPing = new Tuple<int, long, long>(gameClient.LastPing.Item1, gameClient.LastPing.Item2, DateTime.Now.Ticks);
                            Console.WriteLine($"Last Ping time: {((gameClient.LastPing.Item3 - gameClient.LastPing.Item2) / 10000):f3}ms");
                        }
                    }
                    break;
                case OMSIMPMessages.Messages.REPLY_VERSION:
                    {
                        Console.WriteLine($"Server Version: {FastBinaryReader.ReadI32(message, ref parse_pos)}.{FastBinaryReader.ReadI32(message, ref parse_pos):D2}");
                    }
                    break;
                case OMSIMPMessages.Messages.UPDATE_VEHICLE_POSITION:
                    {
                        gameClient.UpdateVehicles(FastBinaryReader.Read<OMSIMPMessages.Vehicle_Position_Update>(message, ref parse_pos));
                    }
                    break;
                default:
                    {
                        Console.WriteLine($"Data received {message}");
                        break;
                    }
            }
        }
    }
}
