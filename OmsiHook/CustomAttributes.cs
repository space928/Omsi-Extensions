using System;

namespace OmsiHook
{
    /// <summary>
    /// Marks a field to be converted from an int to a string.<para/>
    /// Used by Memory.MarshalStruct()
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class OmsiStrPtrAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly bool wide;

        // This is a positional argument
        public OmsiStrPtrAttribute(bool wide = false)
        {
            this.wide = wide;
        }

        public bool Wide => wide;
    }

    /// <summary>
    /// Marks a field to be converted from an int to an IntPtr (32-Bit).<para/>
    /// Used by Memory.MarshalStruct()
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class OmsiPtrAttribute : Attribute
    {

    }
}
