using Microsoft.VisualBasic;
using OmsiHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telepathy;

namespace Multiplayer_Client
{
    internal class GameClient
    {
        public Tuple<int, long, long> LastPing;
        private OmsiHook.OmsiHook omsi;
        private Dictionary<int, OmsiRoadVehicleInst> Vehciles = new Dictionary<int, OmsiRoadVehicleInst>();
        public GameClient() {
            omsi = new OmsiHook.OmsiHook();
            omsi.AttachToOMSI().Wait();
            var OMSIRM = omsi.RemoteMethods;
            OMSIRM.OmsiSetCriticalSectionLock(omsi.Globals.ProgamManager.CS_MakeVehiclePtr).ContinueWith((_) =>
            {
                OMSIRM.MakeVehicle(@"Vehicles\GPM_MAN_LionsCity_M\MAN_A47.bus", __copyToMainList: true).ContinueWith((id) =>
                {
                    Console.WriteLine($"Spawned Vehicle ID: {id.Result}");
                    Vehciles[0] = omsi.Globals.RoadVehicles.FList[1];
                    OMSIRM.OmsiReleaseCriticalSectionLock(omsi.Globals.ProgamManager.CS_MakeVehiclePtr).ContinueWith((_) => Console.WriteLine($"Unlock"));
                });
            });
        }

        public void updateVehicles(OMSIMPMessages.Vehicle_Position_Update update)
        {
            if (Vehciles.TryGetValue(update.ID, out var vehicle))
            {
                vehicle.AbsPosition = update.abs_position;
                vehicle.Position = update.position;
                vehicle.Rotation = update.rotation;
                vehicle.Velocity = update.velocity;
                vehicle.MyKachelPnt = update.tile;
                vehicle.AbsPosition_Inv = update.abs_position_inv;
            } else
            {
                
            }
        }

        public void Tick(Telepathy.Client client)
        {
            if (omsi.Globals.PlayerVehicle.IsNull || !client.Connected)
                return;

            var vehicle = omsi.Globals.PlayerVehicle;
            Console.WriteLine($"\x1b[8;0HP:{vehicle.Position}/{vehicle.MyKachelPnt}\x1b[9;0HR:{vehicle.Rotation}\x1b[10;0HV:{vehicle.Velocity}");
            byte[] buff = new byte[180];
            int out_pos = 0;
            FastBinaryWriter.Write(buff, ref out_pos, OMSIMPMessages.Messages.UPDATE_PLAYER_POSITION);
            FastBinaryWriter.Write(buff, ref out_pos, new OMSIMPMessages.Player_Position_Update()
            {
                position = vehicle.Position,
                abs_position = vehicle.AbsPosition,
                abs_position_inv = vehicle.AbsPosition_Inv,
                tile     = vehicle.MyKachelPnt,
                rotation = vehicle.Rotation,
                velocity = vehicle.Velocity,

            });
            client.Send(buff);
        }
    }
}
