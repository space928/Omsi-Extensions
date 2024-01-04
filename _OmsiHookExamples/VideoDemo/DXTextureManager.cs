using FFMediaToolkit.Decoding;
using OmsiHook;

namespace VideoDemo
{
    public class DXTextureManager
    {
        private readonly ManualResetEventSlim d3dGotContext = new(false);
        private OmsiHook.OmsiHook? omsi;

        // Width and height of the texture
        private uint texWidth;
        private uint texHeight;

        private D3DTexture? texture = null;

        public bool IsReady => texture != null && texture.IsValid;

        // Initialize the DXTextureManager with an OmsiHook instance
        public void Init(OmsiHook.OmsiHook omsi)
        {
            texture = omsi.CreateTextureObject();
            this.omsi = omsi;

            // Subscribe to the OnMapLoaded event
            omsi.OnMapLoaded += Omsi_OnMapLoaded;
        }

        // Method to create a Direct3D texture based on video stream information
        public bool CreateTexture(VideoStreamInfo info, int scriptTextureIndex)
        {
            if (omsi == null || !omsi.IsD3DReady || texture == null)
                return false;

            // Set the texture width and height based on the video stream information
            texWidth = (uint)info.FrameSize.Width;
            texHeight = (uint)info.FrameSize.Height;

            try
            {
                texture.CreateD3DTexture(texWidth, texHeight);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            // Access the script textures of the player vehicle in OmsiHook
            var scriptTexes = omsi.Globals.PlayerVehicle.ComplObjInst.ScriptTextures;

            // Check if the provided script_texture_index is within bounds
            if (scriptTextureIndex >= scriptTexes.Count)
                return false;

            // Update the script texture at the specified index with the new Direct3D texture information
            var old = scriptTexes[scriptTextureIndex];
            scriptTexes[scriptTextureIndex] = new()
            {
                TexPn = old.TexPn,
                color = old.color,
                tex = unchecked((IntPtr)texture.TextureAddress)
            };
            return true;
        }

        // Method to update the contents of the Direct3D texture with new video frame data
        public void UpdateTexture(Memory<byte> data)
        {
            texture?.UpdateTexture(data);
        }

        // Event handler for the OnMapLoaded event, signals when the Direct3D context is obtained
        private void Omsi_OnMapLoaded(object? sender, bool e)
        {
            d3dGotContext.Set();
        }
    }
}