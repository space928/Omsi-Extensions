using OmsiHook;

public static class OMSIMPMessages
{
    public enum Messages : int
    {                                 // Arguments
        REQUEST_VERSION         = 0,  // -
        REPLY_VERSION           = 1,  // Struct Version_Reply
        UPDATE_PLAYER_POSITION  = 2,  // Struct Player_Position_Update
        UPDATE_VEHICLE_POSITION = 3,
        REQUEST_PLAYERS         = 4,
        REPLY_PLAYERS           = 5,
        PING                    = 6,  // Int32 Nonce
        PONG                    = 7,  // Int32 Nonce
    }

    public struct Version_Reply
    {
        public int Major;
        public int Minor;
    }

    public struct Player_Position_Update
    {
        public D3DMatrix abs_position;
        public D3DMatrix abs_position_inv;
        public OmsiPoint tile;
        public D3DXQuaternion rotation;
        public D3DVector velocity;
        public D3DVector position;
    }

    public struct Vehicle_Position_Update
    {
        public int ID;
        public D3DMatrix abs_position;
        public D3DMatrix abs_position_inv;
        public OmsiPoint tile;
        public D3DXQuaternion rotation;
        public D3DVector velocity;
        public D3DVector position;

    }
}
