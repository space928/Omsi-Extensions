using OmsiHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Multiplayer_Server
{
    internal class MessageParser
    {
        readonly static int MAJOR_VERSION = 1;
        readonly static int MINOR_VERSION = 0;
        public static void ParseMessage(byte[] message, Client client, Telepathy.Server server)
        {
            message.AsSpan<byte>(0, message.Length);
            int parse_pos = 0;
            switch ((OMSIMPMessages.Messages) FastBinaryReader.ReadI32(message, ref parse_pos))
            {
                case OMSIMPMessages.Messages.REQUEST_VERSION:
                    {
                        byte[] buff = new byte[12];
                        int out_pos = 0;
                        FastBinaryWriter.Write(buff, ref out_pos, OMSIMPMessages.Messages.REPLY_VERSION);
                        FastBinaryWriter.Write(buff, ref out_pos, new OMSIMPMessages.Version_Reply() { Major = MAJOR_VERSION, Minor = MINOR_VERSION });
                        server.Send(client.ClientId, buff);
                    }
                    break;

                case OMSIMPMessages.Messages.UPDATE_PLAYER_POSITION:
                    {
                        var position = FastBinaryReader.Read<OMSIMPMessages.Player_Position_Update>(message, ref parse_pos);
                        Console.WriteLine($"\u001b[8;0HP:{position.position}/{position.tile}\u001b[9;0HR:{position.rotation}\u001b[10;0HV:{position.velocity}");

                        byte[] buff = new byte[Unsafe.SizeOf<OMSIMPMessages.Vehicle_Position_Update>() + 4];
                        int out_pos = 0;
                        FastBinaryWriter.Write(buff, ref out_pos, OMSIMPMessages.Messages.UPDATE_VEHICLE_POSITION);
                        FastBinaryWriter.Write(buff, ref out_pos, new OMSIMPMessages.Vehicle_Position_Update() { 
                            ID = 0,
                            rotation = position.rotation,
                            tile = position.tile,
                            velocity = position.velocity,
                            position = (new D3DVector(position.position.x + 4, position.position.y, position.position.z)),
                            relmatrix = position.relmatrix,
                            acclocal = position.acclocal,
                        });
                        server.Send(client.ClientId, buff);

                    }
                    break;

                case OMSIMPMessages.Messages.REQUEST_PLAYERS: break;

                case OMSIMPMessages.Messages.PING:
                    {
                        byte[] buff = new byte[8];
                        int out_pos = 0;
                        FastBinaryWriter.Write(buff, ref out_pos, OMSIMPMessages.Messages.PONG);
                        FastBinaryWriter.Write(buff, ref out_pos, FastBinaryReader.ReadI32(message, ref parse_pos));
                        server.Send(client.ClientId, buff);
                    }
                    break;
                default:
                    {
                        Console.WriteLine($"Data received from {client.ClientId}: {message}");
                        break;
                    }
            }
        }
    }
}
