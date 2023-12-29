namespace OmsiHook
{
    /// <summary>
    /// Data about a tile's water content - similar to <seealso cref="OmsiFileTerrain">OmsiFileTerrain</seealso>
    /// </summary>
    public class OmsiFileWater : OmsiObject
    {
        public OmsiFileWater() : base() { }

        internal OmsiFileWater(Memory memory, int address) : base(memory, address) { }

        //TODO: OmsiFileWater
    }
}