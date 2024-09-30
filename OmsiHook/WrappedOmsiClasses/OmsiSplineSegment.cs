namespace OmsiHook
{
    /// <summary>
    /// Segment of spline in a tile - splines are a type of mesh that has geometry, in comparison to <seealso cref="OmsiPathSegment"/>
    /// which defines a track that a vehicle or human can follow, a spline or object can have several paths.
    /// </summary>
    public class OmsiSplineSegment : D3DMeshObject
    {
        public OmsiSplineSegment() { }

        internal OmsiSplineSegment(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        //TODO: OmsiSplineSegment
        //https://github.com/space928/Omsi-Extensions/issues/142
    }
}
