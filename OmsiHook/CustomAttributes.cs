using System;

namespace OmsiHook
{
    /// <summary>
    /// Marks a field to be converted from an internal struct to a struct.<para/>
    /// Used by Memory.MarshalStruct()
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class OmsiStructAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly Type objType;
        readonly Type internalType;
        readonly bool requiresExtraMarshalling;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objType">The type of the object to convert to</param>
        /// <param name="internalType">The intermidiate type to convert through 
        /// (in case Marshal.PtrToStruct doesn't support all the fields in objType).
        /// Leave null to default to objType</param>
        public OmsiStructAttribute(Type objType, Type internalType = null)
        {
            if (!objType.IsValueType)
                throw new ArgumentException("OmsiStructPtr must be a pointer to a struct/value!");
            if ((!internalType?.IsValueType) ?? false)
                throw new ArgumentException("OmsiStructPtr must be a pointer to a struct/value!");

            this.objType = objType;
            this.requiresExtraMarshalling = internalType != null;
            this.internalType = internalType ?? objType;
        }

        public Type ObjType => objType;
        public Type InternalType => internalType;
        public bool RequiresExtraMarshalling => requiresExtraMarshalling;
    }

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

    /// <summary>
    /// Marks a field to be converted from an int to a struct.<para/>
    /// Used by Memory.MarshalStruct()
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class OmsiStructPtrAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly Type objType;
        readonly Type internalType;
        readonly bool requiresExtraMarshalling;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objType">The type of the object to convert to</param>
        /// <param name="internalType">The intermidiate type to convert through 
        /// (in case Marshal.PtrToStruct doesn't support all the fields in objType).
        /// Leave null to default to objType</param>
        public OmsiStructPtrAttribute(Type objType, Type internalType = null)
        {
            if (!objType.IsValueType)
                throw new ArgumentException("OmsiStructPtr must be a pointer to a struct/value!");
            if ((!internalType?.IsValueType) ?? false)
                throw new ArgumentException("OmsiStructPtr must be a pointer to a struct/value!");

            this.objType = objType;
            this.requiresExtraMarshalling = internalType != null;
            this.internalType = internalType ?? objType;
        }

        public Type ObjType => objType;
        public Type InternalType => internalType;
        public bool RequiresExtraMarshalling => requiresExtraMarshalling;
    }

    /// <summary>
    /// Marks a field to be converted from an int to an OmsiObject.<para/>
    /// Used by Memory.MarshalStruct()
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class OmsiObjPtrAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly Type objType;

        public OmsiObjPtrAttribute(Type objType)
        {
            if (!objType.IsSubclassOf(typeof(OmsiObject)))
                throw new ArgumentException("OmsiObjPtr must be a pointer to an object deriving from " + nameof(OmsiObject) + "!");

            this.objType = objType;
        }

        public Type ObjType => objType;
    }

    /// <summary>
    /// Marks a field to be converted from an int to an OmsiObject[].<para/>
    /// Used by Memory.MarshalStruct()
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class OmsiObjArrayPtrAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly Type objType;

        public OmsiObjArrayPtrAttribute(Type objType)
        {
            if (objType.IsSubclassOf(typeof(OmsiObject)))
                throw new ArgumentException("OmsiObjArrayPtr must be a pointer to an object deriving from " + nameof(OmsiObject) + "!");

            this.objType = objType;
        }

        public Type ObjType => objType;
    }

    /// <summary>
    /// Marks a field to be converted from an int to a struct[].<para/>
    /// Used by Memory.MarshalStruct()
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class OmsiStructArrayPtrAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly Type objType;
        readonly Type internalType;
        readonly bool requiresExtraMarshalling;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objType">The type of the object to convert to</param>
        /// <param name="internalType">The intermidiate type to convert through 
        /// (in case Marshal.PtrToStruct doesn't support all the fields in objType).
        /// Leave null to default to objType</param>
        public OmsiStructArrayPtrAttribute(Type objType, Type internalType = null)
        {
            if (!objType.IsValueType)
                throw new ArgumentException("OmsiStructArrayPtr must be a pointer to a struct/value!");
            if ((!internalType?.IsValueType) ?? false)
                throw new ArgumentException("OmsiStructArrayPtr must be a pointer to a struct/value!");

            this.objType = objType;
            this.requiresExtraMarshalling = internalType != null;
            this.internalType = internalType ?? objType;
        }

        public Type ObjType => objType;
        public Type InternalType => internalType;
        public bool RequiresExtraMarshalling => requiresExtraMarshalling;
    }

    /// <summary>
    /// Marks a field to be converted from an int to an array of strings.<para/>
    /// Used by Memory.MarshalStruct()
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class OmsiStrArrayPtrAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly bool wide;

        public OmsiStrArrayPtrAttribute(bool wide = false)
        {
            this.wide = wide;
        }

        public bool Wide => wide;
    }
}
