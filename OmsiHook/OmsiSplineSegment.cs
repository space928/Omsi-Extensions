namespace OmsiHook
{
    /// <summary>
    /// Segment of spline in a tile
    /// </summary>
    public class OmsiSplineSegment : D3DMeshObject
    {
        public OmsiSplineSegment() { }

        internal OmsiSplineSegment(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        //TODO: OmsiSplineSegment
    }
}