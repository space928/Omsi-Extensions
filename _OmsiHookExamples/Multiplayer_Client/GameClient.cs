using OmsiHook;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace OmsiHook.Examples.Multiplayer_Client
{
    internal class GameClient
    {
        public Tuple<int, long, long> LastPing;
        private OmsiHook omsi;
        private Dictionary<int, OmsiRoadVehicleInst> Vehicles = new Dictionary<int, OmsiRoadVehicleInst>();
        private int frameCounter = 0;
        public GameClient()
        {
            omsi = new OmsiHook();
            omsi.AttachToOMSI().Wait();
            var OMSIRM = omsi.RemoteMethods;
            OMSIRM.OmsiSetCriticalSectionLock(omsi.Globals.ProgamManager.CS_MakeVehiclePtr).ContinueWith((_) =>
            {
                OMSIRM.MakeVehicle(@"Vehicles\GPM_MAN_LionsCity_M\MAN_A47.bus", __copyToMainList: true).ContinueWith((id) =>
                {
                    Console.WriteLine($"Spawned Vehicle ID: {id.Result}");
                    Vehicles[0] = omsi.Globals.RoadVehicles.FList[1];
                    OMSIRM.OmsiReleaseCriticalSectionLock(omsi.Globals.ProgamManager.CS_MakeVehiclePtr).ContinueWith((_) => Console.WriteLine($"Unlock"));
                });
            });
        }

        public void UpdateVehicles(OMSIMPMessages.Vehicle_Position_Update update)
        {
            if (Vehicles.TryGetValue(update.ID, out var vehicle))
            {
                if (frameCounter % 20 == 0)
                    vehicle.Position = update.position;
                vehicle.Rotation = update.rotation;
                vehicle.Velocity = update.velocity;
                vehicle.MyKachelPnt = update.tile;
                vehicle.RelMatrix = update.relmatrix;
                vehicle.Acc_Local = update.acclocal;

                var posMat = Matrix4x4.CreateFromQuaternion(update.rotation);
                posMat.Translation = update.position;
                var absPosMat = Matrix4x4.Multiply(posMat, Matrix4x4.Identity/*update.relmatrix*/);
                Matrix4x4.Invert(absPosMat, out var absPosMatInv);

                vehicle.Pos_Mat = posMat;
                vehicle.AbsPosition = absPosMat;
                vehicle.AbsPosition_Inv = absPosMatInv;
                vehicle.Used_RelVec = ((Matrix4x4)update.relmatrix).Translation;
                vehicle.AI_Blinker_L = 1;
                vehicle.AI_Blinker_R = 1;
                vehicle.AI_var = 1;
                frameCounter++;
            }
            else
            {

            }
        }

        public void Tick(Telepathy.Client client)
        {
            if (omsi.Globals.PlayerVehicle.IsNull || !client.Connected)
                return;

            var vehicle = omsi.Globals.PlayerVehicle;
            Console.WriteLine($"\x1b[8;0HP:{vehicle.Position}/{vehicle.MyKachelPnt}\x1b[9;0HR:{vehicle.Rotation}\x1b[10;0HV:{vehicle.Velocity}\x1b[11;0HB:{vehicle.Acc_Local} / {((Vehicles.ContainsKey(0)) ? (Vehicles[0].Acc_Local.ToString()) : "-")}");
            byte[] buff = new byte[Unsafe.SizeOf<OMSIMPMessages.Player_Position_Update>() + 4];
            int out_pos = 0;
            FastBinaryWriter.Write(buff, ref out_pos, OMSIMPMessages.Messages.UPDATE_PLAYER_POSITION);
            FastBinaryWriter.Write(buff, ref out_pos, new OMSIMPMessages.Player_Position_Update()
            {
                position = vehicle.Position,
                tile = vehicle.MyKachelPnt,
                rotation = vehicle.Rotation,
                velocity = vehicle.Velocity,
                relmatrix = vehicle.RelMatrix,
                acclocal = vehicle.Acc_Local
                //vehicle = vehicle.RoadVehicle.MyPath

            }); ;
            client.Send(buff);
        }
    }
}
