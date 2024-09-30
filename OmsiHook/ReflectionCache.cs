using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace OmsiHook;

/// <summary>
/// Stores a cache of all the reflection data needed to marshal a complex data structure.
/// </summary>
internal class ReflectionCache
{
    /// <summary>
    /// The global cache of reflection data.
    /// </summary>
    private static readonly ConcurrentDictionary<(Type InStruct, Type OutStruct), ReflectionCache> globalCache = new();

    public readonly Type nativeType;
    public readonly Type localType;
    public readonly FieldTransform[] fieldMapping;
    //public readonly 

    public struct FieldTransform
    {
        public FieldInfo native;
        public FieldInfo local;
        public Func<object, object> toNative;
        public Func<object, object> toLocal;
    }

    private ReflectionCache() { }
    private ReflectionCache(Type nativeType, Type localType)
    {
        this.fieldMapping = new FieldTransform[nativeType.GetFields().Length];
        this.nativeType = nativeType;
        this.localType = localType;
    }

    /// <summary>
    /// Creates or gets a reflection cache object needed to marshal the given types.
    /// </summary>
    /// <param name="nativeType">The unmanaged data structure type.</param>
    /// <param name="localType">The managed data structure type.</param>
    /// <returns></returns>
    /// <exception cref="FieldAccessException"></exception>
    public static ReflectionCache GetOrBuildReflectionCache(Memory mem, Type nativeType, Type localType)
    {
        if (globalCache.TryGetValue((nativeType, localType), out var ret))
            return ret;

        return BuildReflectionCache(mem, nativeType, localType);
    }

    /*public static ReflectionCache BuildReflectionCacheIL(Memory mem, Type nativeType, Type localType)
    {
        var ret = new ReflectionCache(nativeType, localType);
    }*/

    /// <summary>
    /// Creates a new reflection cache object needed to marshal the given types.
    /// </summary>
    /// <param name="nativeType">The unmanaged data structure type.</param>
    /// <param name="localType">The managed data structure type.</param>
    /// <returns></returns>
    /// <exception cref="FieldAccessException"></exception>
    // TODO: Rewrite this to use IL generation to avoid unnecessary boxing...
    // https://github.com/space928/Omsi-Extensions/issues/120
    public static ReflectionCache BuildReflectionCache(Memory mem, Type nativeType, Type localType)
    {
        var ret = new ReflectionCache(nativeType, localType);

        FieldInfo[] nativeFields = nativeType.GetFields();
        for (int i = 0; i < nativeFields.Length; i++)
        {
            FieldInfo nativeFld = nativeFields[i];
            ref var map = ref ret.fieldMapping[i];
#if DEBUG
            try
            {
#endif
                map.native = nativeFld;
                // Match fields by name, setting the destination fields to the corresponding source fields
                map.local = localType.GetField(nativeFld.Name);

                foreach (var attr in nativeFld.GetCustomAttributes(false))
                {
                    // Based on which kind of attribute the field has, perform special marshalling operations
                    switch (attr)
                    {
                        case OmsiStructAttribute a:
                            if (a.RequiresExtraMarshalling)
                            {
                                if (a.InternalType == nativeType)
                                    throw new ArgumentException($"Struct of type {nativeType.Name} tried to marshal one of it's fields as {nativeType.Name}, recursive data types are not allowed!");

                                map.toLocal = typeof(Memory).GetMethod(nameof(Memory.MarshalStruct), BindingFlags.NonPublic | BindingFlags.Instance, new[] { typeof(object) })
                                    .MakeGenericMethod(a.ObjType, a.InternalType)
                                    .CreateDelegate<Func<object, object>>(mem);

                                map.toNative = typeof(Memory).GetMethod(nameof(Memory.UnMarshalStruct), BindingFlags.NonPublic | BindingFlags.Instance, new[] { typeof(object) })
                                    .MakeGenericMethod(a.ObjType, a.InternalType)
                                    .CreateDelegate<Func<object, object>>(mem);
                            }
                            break;

                        case OmsiStrPtrAttribute a:
                            map.toLocal = val => mem.ReadMemoryString((int)val, a.Wide, a.Raw, a.Pascal);
                            map.toNative = val => mem.AllocateString((string)val, a.Wide).Result;
                            break;

                        case OmsiPtrAttribute:
                            map.toLocal = val => new IntPtr((int)val);
                            map.toNative = val => ((IntPtr)val).ToInt32();
                            break;

                        case OmsiStructPtrAttribute a:
                            var readStructFunc = typeof(Memory).GetMethod(nameof(Memory.ReadMemory), new Type[] { typeof(int) })
                                .MakeGenericMethod(a.InternalType)
                                .CreateDelegate<Func<object, object>>(mem);
                            var writeStructFunc = typeof(Memory).GetMethod(nameof(Memory.AllocateStruct), BindingFlags.NonPublic | BindingFlags.Instance, new[] { typeof(object) })
                                .MakeGenericMethod(a.InternalType)
                                .CreateDelegate<Func<object, object>>(mem);

                            // Perform extra marshalling if needed
                            if (a.RequiresExtraMarshalling)
                            {
                                if (a.InternalType == nativeType)
                                    throw new ArgumentException($"Struct of type {nativeType.Name} tried to marshal one of it's fields as {nativeType.Name}, recursive data types are not allowed!");

                                var marshalFunc = typeof(Memory).GetMethod(nameof(Memory.MarshalStruct), BindingFlags.NonPublic | BindingFlags.Instance, new[] { typeof(object) })
                                .MakeGenericMethod(a.ObjType, a.InternalType)
                                .CreateDelegate<Func<object, object>>(mem);

                                var unMarshalFunc = typeof(Memory).GetMethod(nameof(Memory.UnMarshalStruct), BindingFlags.NonPublic | BindingFlags.Instance, new[] { typeof(object) })
                                .MakeGenericMethod(a.ObjType, a.InternalType)
                                .CreateDelegate<Func<object, object>>(mem);

                                map.toLocal = val => marshalFunc(readStructFunc(val));
                                map.toNative = val => writeStructFunc(unMarshalFunc(val));
                            }
                            else
                            {
                                map.toLocal = readStructFunc;
                                map.toNative = writeStructFunc;
                            }
                            break;

                        case OmsiObjPtrAttribute a:
                            map.toLocal = val =>
                            {
                                int addr = (int)val;
                                val = Activator.CreateInstance(a.ObjType, true);
                                ((OmsiObject)val).InitObject(mem, addr);
                                return val;
                            };
                            map.toNative = val => ((OmsiObject)val).Address;
                            break;

                        case OmsiStructArrayPtrAttribute a:
                            var readStructsFunc = typeof(Memory).GetMethod(nameof(Memory.ReadMemoryStructArray))
                                .MakeGenericMethod(a.InternalType)
                                .CreateDelegate<Func<int, bool, object>>(mem);
                            var writeStructsFunc = typeof(Memory).GetMethod(nameof(Memory.AllocateAndInitStructArray), BindingFlags.NonPublic | BindingFlags.Instance, new[] { typeof(object), typeof(int), typeof(bool) })
                                .MakeGenericMethod(a.InternalType)
                                .CreateDelegate<Func<object, int, bool, int>>(mem);

                            // Perform extra marshalling if needed
                            if (a.RequiresExtraMarshalling)
                            {
                                if (a.InternalType == nativeType)
                                    throw new ArgumentException($"Struct of type {nativeType.Name} tried to marshal one of it's fields as {nativeType.Name}[], recursive data types are not allowed!");

                                var marshalFunc = typeof(Memory).GetMethod(nameof(Memory.MarshalStructs), BindingFlags.NonPublic | BindingFlags.Instance, new[] { typeof(object) })
                                .MakeGenericMethod(a.ObjType, a.InternalType)
                                .CreateDelegate<Func<object, object>>(mem);

                                var unMarshalFunc = typeof(Memory).GetMethod(nameof(Memory.UnMarshalStructs), BindingFlags.NonPublic | BindingFlags.Instance, new[] { typeof(object) })
                                .MakeGenericMethod(a.InternalType, a.ObjType)
                                .CreateDelegate<Func<object, object>>(mem);

                                map.toLocal = val => marshalFunc(readStructsFunc((int)val, a.Raw));
                                map.toNative = val => writeStructsFunc(unMarshalFunc(val), 1, a.Raw);
                            }
                            else
                            {
                                map.toLocal = val => readStructsFunc((int)val, a.Raw);
                                map.toNative = val => writeStructsFunc(val, 1, a.Raw);
                            }
                            break;

                        case OmsiObjArrayPtrAttribute a:
                            var readObjsFunc = typeof(Memory).GetMethod(nameof(Memory.ReadMemoryObjArray))
                                .MakeGenericMethod(a.ObjType)
                                .CreateDelegate<Func<object, object>>(mem);

                            map.toLocal = readObjsFunc;
                            map.toNative = val => mem.AllocateAndInitStructArray(((OmsiObject[])val).Select(x => x.Address).ToArray()).Result;
                            break;

                        case OmsiStrArrayPtrAttribute a:
                            map.toLocal = val => mem.ReadMemoryStringArray((int)val, a.Wide, a.Raw, a.Pascal);
                            map.toNative = val =>
                            {
                                // TODO: I might add a dedicated method for allocating string arrays
                                // https://github.com/space928/Omsi-Extensions/issues/121
                                var stringTasks = ((string[])val).Select(x => mem.AllocateString(x, a.Wide, 1, a.Raw));
                                var strings = Task.WhenAll(stringTasks);
                                return mem.AllocateAndInitStructArray(strings.Result).Result;
                            };
                            break;

                        case OmsiMarshallerAttribute a:
                            throw new NotImplementedException($"Attribute {attr.GetType().FullName} is not yet supported by the marshaller!");

                        default:
                            break;
                    }
                }
#if DEBUG
            }
            catch (Exception ex)
            {
                throw new FieldAccessException($"Failed to marshal field '{nativeFld.Name}' in '{nativeFld.ReflectedType.FullName}'. \n" +
                    $"Attributes:\n{GetCustomAttributeDebugString(nativeFld)}\n" +
                    $"Failed with internal exception:\n", ex);
            }
#endif
        }

        if (globalCache.TryAdd((nativeType, localType), ret))
            return ret;
        else if (globalCache.TryGetValue((nativeType, localType), out ret))
            return ret;
        return ret;
    }

    private static string GetCustomAttributeDebugString(FieldInfo field)
    {
        return string.Join("\n\t", field.GetCustomAttributes(false)
            .Select(x => string.Format("  {0}: {1}", x.GetType().Name, string.Join(", ", x.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Select(attProp => $"{attProp.Name}={attProp.GetValue(x)}")))));
    }
}

internal static class ReflectionCacheIL<NativeStruct, LocalStruct> where NativeStruct : unmanaged where LocalStruct : struct
{
    private static bool compiled = false;
    private static MarshalStruct marshalStructFunc = null;
    private static UnMarshalStruct unmarshalStructFunc = null;

    public static bool Supported => RuntimeFeature.IsDynamicCodeSupported;
    public static bool Compiled => compiled;
    public static MarshalStruct MarshalStructFunc => marshalStructFunc;
    public static UnMarshalStruct UnmarshalStructFunc => unmarshalStructFunc;

    internal delegate LocalStruct MarshalStruct(NativeStruct obj);
    internal delegate NativeStruct UnMarshalStruct(LocalStruct obj);

    public static void Compile(Memory mem)
    {
        var localType = typeof(LocalStruct);
        var nativeType = typeof(NativeStruct);

        DynamicMethod marshalFunc = new($"Marshal{localType.Name}", localType, new Type[] { nativeType.MakeByRefType() }, localType, true);
        DynamicMethod unmarshalFunc = new($"UnMarshal{localType.Name}", nativeType, new Type[] { localType.MakeByRefType() }, localType, true);
        var mil = marshalFunc.GetILGenerator();
        var uil = unmarshalFunc.GetILGenerator();
        mil.DeclareLocal(localType);
        uil.DeclareLocal(localType);
        //loc0.
        // Create destination objects for the marshalled/unmarshalled data
        mil.Emit(OpCodes.Newobj, localType);
        uil.Emit(OpCodes.Newobj, nativeType);
        mil.Emit(OpCodes.Stloc_0);
        uil.Emit(OpCodes.Stloc_0);
        // Load the object to convert
        mil.Emit(OpCodes.Ldarg_0);
        uil.Emit(OpCodes.Ldarg_0);
        //il.Emit(OpCodes.Ldind_Ref);
        //il.Emit(OpCodes.Ldflda);

        FieldInfo[] nativeFields = nativeType.GetFields();
        for (int i = 0; i < nativeFields.Length; i++)
        {
            FieldInfo nativeFld = nativeFields[i];
#if DEBUG
            try
            {
#endif
                mil.BeginScope();
                uil.BeginScope();

                // Match fields by name, setting the destination fields to the corresponding source fields
                var localFld = localType.GetField(nativeFld.Name);
                // Load the field to convert onto the stack
                mil.Emit(OpCodes.Ldfld, nativeFld);
                uil.Emit(OpCodes.Ldfld, localFld);

                /*foreach (var attr in nativeFld.GetCustomAttributes(false))
                {
                    // Based on which kind of attribute the field has, perform special marshalling operations
                    switch (attr)
                    {
                        case OmsiStructAttribute a:
                            if (a.RequiresExtraMarshalling)
                            {
                                if (a.InternalType == nativeType)
                                    throw new ArgumentException($"Struct of type {nativeType.Name} tried to marshal one of it's fields as {nativeType.Name}, recursive data types are not allowed!");

                                map.toLocal = typeof(Memory).GetMethod(nameof(Memory.MarshalStruct))
                                    .MakeGenericMethod(a.ObjType, a.InternalType)
                                    .CreateDelegate<Func<object, object>>(mem);

                                map.toNative = typeof(Memory).GetMethod(nameof(Memory.UnMarshalStruct))
                                    .MakeGenericMethod(a.ObjType, a.InternalType)
                                    .CreateDelegate<Func<object, object>>(mem);
                            }
                            break;

                        case OmsiStrPtrAttribute a:
                            map.toLocal = val => mem.ReadMemoryString((int)val, a.Wide, a.Raw, a.Pascal);
                            map.toNative = val => mem.AllocateString((string)val, a.Wide).Result;
                            break;

                        case OmsiPtrAttribute:
                            map.toLocal = val => new IntPtr((int)val);
                            map.toNative = val => ((IntPtr)val).ToInt32();
                            break;

                        case OmsiStructPtrAttribute a:
                            var readStructFunc = typeof(Memory).GetMethod(nameof(Memory.ReadMemory), new Type[] { typeof(int) })
                                .MakeGenericMethod(a.InternalType)
                                .CreateDelegate<Func<object, object>>(mem);
                            var writeStructFunc = typeof(Memory).GetMethod(nameof(Memory.AllocateStruct))
                                .MakeGenericMethod(a.InternalType)
                                .CreateDelegate<Func<object, object>>(mem);

                            // Perform extra marshalling if needed
                            if (a.RequiresExtraMarshalling)
                            {
                                if (a.InternalType == nativeType)
                                    throw new ArgumentException($"Struct of type {nativeType.Name} tried to marshal one of it's fields as {nativeType.Name}, recursive data types are not allowed!");

                                var marshalFunc = typeof(Memory).GetMethod(nameof(Memory.MarshalStruct))
                                .MakeGenericMethod(a.ObjType, a.InternalType)
                                .CreateDelegate<Func<object, object>>(mem);

                                var unMarshalFunc = typeof(Memory).GetMethod(nameof(Memory.UnMarshalStruct))
                                .MakeGenericMethod(a.ObjType, a.InternalType)
                                .CreateDelegate<Func<object, object>>(mem);

                                map.toLocal = val => marshalFunc(readStructFunc(val));
                                map.toNative = val => writeStructFunc(unMarshalFunc(val));
                            }
                            else
                            {
                                map.toLocal = readStructFunc;
                                map.toNative = writeStructFunc;
                            }
                            break;

                        case OmsiObjPtrAttribute a:
                            map.toLocal = val =>
                            {
                                int addr = (int)val;
                                val = Activator.CreateInstance(a.ObjType, true);
                                ((OmsiObject)val).InitObject(mem, addr);
                                return val;
                            };
                            map.toNative = val => ((OmsiObject)val).Address;
                            break;

                        case OmsiStructArrayPtrAttribute a:
                            var readStructsFunc = typeof(Memory).GetMethod(nameof(Memory.ReadMemoryStructArray))
                                .MakeGenericMethod(a.InternalType)
                                .CreateDelegate<Func<int, bool, object>>(mem);
                            var writeStructsFunc = typeof(Memory).GetMethod(nameof(Memory.AllocateAndInitStructArray))
                                .MakeGenericMethod(a.InternalType)
                                .CreateDelegate<Func<object, int, bool, object>>(mem);

                            // Perform extra marshalling if needed
                            if (a.RequiresExtraMarshalling)
                            {
                                if (a.InternalType == nativeType)
                                    throw new ArgumentException($"Struct of type {nativeType.Name} tried to marshal one of it's fields as {nativeType.Name}[], recursive data types are not allowed!");

                                var marshalFunc = typeof(Memory).GetMethod(nameof(Memory.MarshalStructs))
                                .MakeGenericMethod(a.ObjType, a.InternalType)
                                .CreateDelegate<Func<object, object>>(mem);

                                var unMarshalFunc = typeof(Memory).GetMethod(nameof(Memory.UnMarshalStructs))
                                .MakeGenericMethod(a.ObjType, a.InternalType)
                                .CreateDelegate<Func<object, object>>(mem);

                                map.toLocal = val => marshalFunc(readStructsFunc((int)val, a.Raw));
                                map.toNative = val => writeStructsFunc(unMarshalFunc(val), 1, a.Raw);
                            }
                            else
                            {
                                map.toLocal = val => readStructsFunc((int)val, a.Raw);
                                map.toNative = val => writeStructsFunc(val, 1, a.Raw);
                            }
                            break;

                        case OmsiObjArrayPtrAttribute a:
                            var readObjsFunc = typeof(Memory).GetMethod(nameof(Memory.ReadMemoryObjArray))
                                .MakeGenericMethod(a.ObjType)
                                .CreateDelegate<Func<object, object>>(mem);

                            map.toLocal = readObjsFunc;
                            map.toNative = val => mem.AllocateAndInitStructArray(((OmsiObject[])val).Select(x => x.Address).ToArray()).Result;
                            break;

                        case OmsiStrArrayPtrAttribute a:
                            map.toLocal = val => mem.ReadMemoryStringArray((int)val, a.Wide, a.Raw, a.Pascal);
                            map.toNative = val =>
                            {
                                // TODO: I might add a dedicated method for allocating string arrays
                                // https://github.com/space928/Omsi-Extensions/issues/121
                                var stringTasks = ((string[])val).Select(x => mem.AllocateString(x, a.Wide, 1, a.Raw));
                                var strings = Task.WhenAll(stringTasks);
                                return mem.AllocateAndInitStructArray(strings.Result).Result;
                            };
                            break;

                        case OmsiMarshallerAttribute a:
                            throw new NotImplementedException($"Attribute {attr.GetType().FullName} is not yet supported by the marshaller!");

                        default:
                            break;
                    }
                }*/

                // Now that the field has been marshalled, write it back to the destination object
                // Swap the field value and the destination object on the stack
                mil.DeclareLocal(localFld.FieldType);
                uil.DeclareLocal(nativeFld.FieldType);
                mil.Emit(OpCodes.Stloc_1);
                uil.Emit(OpCodes.Stloc_1);
                mil.Emit(OpCodes.Ldloc_0);
                uil.Emit(OpCodes.Ldloc_0);
                mil.Emit(OpCodes.Ldloc_1);
                uil.Emit(OpCodes.Ldloc_1);
                mil.Emit(OpCodes.Stfld, localFld);
                uil.Emit(OpCodes.Stfld, nativeFld);

                mil.EndScope();
                uil.EndScope();
#if DEBUG
            }
            catch (Exception ex)
            {
                throw new FieldAccessException($"Failed to marshal field '{nativeFld.Name}' in '{nativeFld.ReflectedType.FullName}'. \n" +
                    $"Attributes:\n{GetCustomAttributeDebugString(nativeFld)}\n" +
                    $"Failed with internal exception:\n", ex);
            }
#endif
        }

        mil.Emit(OpCodes.Ret);
        uil.Emit(OpCodes.Ret);

        marshalStructFunc = marshalFunc.CreateDelegate<MarshalStruct>();
        unmarshalStructFunc = unmarshalFunc.CreateDelegate<UnMarshalStruct>();

        compiled = true;
    }

    private static string GetCustomAttributeDebugString(FieldInfo field)
    {
        return string.Join("\n\t", field.GetCustomAttributes(false)
            .Select(x => string.Format("  {0}: {1}", x.GetType().Name, string.Join(", ", x.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Select(attProp => $"{attProp.Name}={attProp.GetValue(x)}")))));
    }
}

internal static class ReflectionCacheExpression<NativeStruct, LocalStruct> where NativeStruct : unmanaged where LocalStruct : struct
{
    private static bool compiled = false;
    private static MarshalStruct marshalStructFunc = null;
    private static UnMarshalStruct unmarshalStructFunc = null;

    public static bool Supported => RuntimeFeature.IsDynamicCodeSupported;
    public static bool Compiled => compiled;
    public static MarshalStruct MarshalStructFunc => marshalStructFunc;
    public static UnMarshalStruct UnmarshalStructFunc => unmarshalStructFunc;

    internal delegate LocalStruct MarshalStruct(NativeStruct obj);
    internal delegate NativeStruct UnMarshalStruct(LocalStruct obj);

    public static void Compile(Memory mem)
    {
        var localType = typeof(LocalStruct);
        var nativeType = typeof(NativeStruct);

        var mobj = Expression.Parameter(nativeType, "inVal");
        var uobj = Expression.Parameter(localType, "inVal");
        var mret = Expression.Variable(localType, "mret");
        var mretInit = Expression.Assign(mret, Expression.New(localType));
        var uret = Expression.Variable(nativeType, "uret");
        var uretInit = Expression.Assign(uret, Expression.New(nativeType));

        var mblocks = new List<Expression>();
        var ublocks = new List<Expression>();

        var memInst = Expression.Constant(mem);

        mblocks.Add(mretInit);
        ublocks.Add(uretInit);

        FieldInfo[] nativeFields = nativeType.GetFields();
        for (int i = 0; i < nativeFields.Length; i++)
        {
            FieldInfo nativeFld = nativeFields[i];
#if DEBUG
            try
            {
#endif
                // Match fields by name, setting the destination fields to the corresponding source fields
                var localFld = localType.GetField(nativeFld.Name);
                // Load the field to convert onto the stack
                Expression mvalSrc = Expression.Field(mobj, nativeFld);
                Expression uvalSrc = Expression.Field(uobj, localFld);
                var mvalDst = Expression.Field(mret, localFld);
                var uvalDst = Expression.Field(uret, nativeFld);

                foreach (var attr in nativeFld.GetCustomAttributes(false))
                {
                    // Based on which kind of attribute the field has, perform special marshalling operations
                    switch (attr)
                    {
                        case OmsiStructAttribute a:
                            {
                                if (a.RequiresExtraMarshalling)
                                {
                                    if (a.InternalType == nativeType)
                                        throw new ArgumentException($"Struct of type {nativeType.Name} tried to marshal one of it's fields as {nativeType.Name}, recursive data types are not allowed!");

                                    var toLocal = typeof(Memory).GetMethod(nameof(Memory.MarshalStruct))
                                        .MakeGenericMethod(a.ObjType, a.InternalType);

                                    var toNative = typeof(Memory).GetMethod(nameof(Memory.UnMarshalStruct))
                                        .MakeGenericMethod(a.ObjType, a.InternalType);

                                    mvalSrc = Expression.Call(memInst, toLocal, mvalSrc);
                                    uvalSrc = Expression.Call(memInst, toNative, uvalSrc);
                                }
                                break;
                            }
                        case OmsiStrPtrAttribute a:
                            {
                                //Expression<Func<int, string>> toLocal = val => mem.ReadMemoryString(val, a.Wide, a.Raw, a.Pascal);
                                //Expression<Func<string, int>> toNative = val => mem.AllocateString(val, a.Wide, 1, a.Raw).Result;
                                var toLocal = typeof(Memory).GetMethod(nameof(Memory.ReadMemoryString), new[] { typeof(int), typeof(bool), typeof(bool), typeof(bool) });
                                var toNative = typeof(Memory).GetMethod(nameof(Memory.AllocateString), new[] { typeof(string), typeof(bool), typeof(int), typeof(bool) });

                                mvalSrc = Expression.Call(memInst, toLocal, mvalSrc, Expression.Constant(a.Wide), Expression.Constant(a.Raw), Expression.Constant(a.Pascal));
                                uvalSrc = Expression.Call(memInst, toNative, uvalSrc, Expression.Constant(a.Wide), Expression.Constant(1), Expression.Constant(a.Raw));
                                uvalSrc = Expression.Property(uvalSrc, nameof(Task<int>.Result));
                                break;
                            }
                        case OmsiPtrAttribute:
                            {
                                mvalSrc = Expression.New(typeof(IntPtr).GetConstructor(new[] { typeof(int) }), mvalSrc);
                                uvalSrc = Expression.Call(uvalSrc, typeof(IntPtr).GetMethod(nameof(IntPtr.ToInt32)));

                                break;
                            }
                        case OmsiStructPtrAttribute a:
                            {
                                var readStructFunc = typeof(Memory).GetMethod(nameof(Memory.ReadMemory), new Type[] { typeof(int) })
                                    .MakeGenericMethod(a.InternalType);
                                var writeStructFunc = typeof(Memory).GetMethod(nameof(Memory.AllocateStruct))
                                    .MakeGenericMethod(a.InternalType);

                                // Perform extra marshalling if needed
                                if (a.RequiresExtraMarshalling)
                                {
                                    if (a.InternalType == nativeType)
                                        throw new ArgumentException($"Struct of type {nativeType.Name} tried to marshal one of it's fields as {nativeType.Name}, recursive data types are not allowed!");

                                    var marshalFunc = typeof(Memory).GetMethod(nameof(Memory.MarshalStruct))
                                        .MakeGenericMethod(a.ObjType, a.InternalType);

                                    var unMarshalFunc = typeof(Memory).GetMethod(nameof(Memory.UnMarshalStruct))
                                        .MakeGenericMethod(a.ObjType, a.InternalType);

                                    mvalSrc = Expression.Call(memInst, readStructFunc, mvalSrc);
                                    mvalSrc = Expression.Call(memInst, marshalFunc, mvalSrc);

                                    uvalSrc = Expression.Call(memInst, unMarshalFunc, uvalSrc);
                                    uvalSrc = Expression.Call(memInst, writeStructFunc, uvalSrc);
                                    uvalSrc = Expression.Property(uvalSrc, nameof(Task<int>.Result));
                                }
                                else
                                {
                                    mvalSrc = Expression.Call(memInst, readStructFunc, mvalSrc);
                                    uvalSrc = Expression.Call(memInst, writeStructFunc, uvalSrc);
                                    uvalSrc = Expression.Property(uvalSrc, nameof(Task<int>.Result));
                                }
                                break;
                            }
                        case OmsiObjPtrAttribute a:
                            {
                                var ctor = a.ObjType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, new[] { typeof(Memory), typeof(int) });
                                mvalSrc = Expression.New(ctor, memInst, mvalSrc);
                                uvalSrc = Expression.Property(uvalSrc, nameof(OmsiObject.Address));
                                break;
                            }
                        case OmsiStructArrayPtrAttribute a:
                            {
                                var readStructsFunc = typeof(Memory).GetMethod(nameof(Memory.ReadMemoryStructArray))
                                    .MakeGenericMethod(a.InternalType);
                                var writeStructsFunc = typeof(Memory).GetMethod(nameof(Memory.AllocateAndInitStructArray))
                                    .MakeGenericMethod(a.InternalType);

                                // Perform extra marshalling if needed
                                if (a.RequiresExtraMarshalling)
                                {
                                    if (a.InternalType == nativeType)
                                        throw new ArgumentException($"Struct of type {nativeType.Name} tried to marshal one of it's fields as {nativeType.Name}[], recursive data types are not allowed!");

                                    var marshalFunc = typeof(Memory).GetMethod(nameof(Memory.MarshalStructs))
                                        .MakeGenericMethod(a.ObjType, a.InternalType);

                                    var unMarshalFunc = typeof(Memory).GetMethod(nameof(Memory.UnMarshalStructs))
                                        .MakeGenericMethod(a.InternalType, a.ObjType);

                                    mvalSrc = Expression.Call(memInst, readStructsFunc, mvalSrc, Expression.Constant(a.Raw));
                                    mvalSrc = Expression.Call(memInst, marshalFunc, mvalSrc);

                                    uvalSrc = Expression.Call(memInst, unMarshalFunc, uvalSrc);
                                    uvalSrc = Expression.Call(memInst, writeStructsFunc, uvalSrc, Expression.Constant(1), Expression.Constant(a.Raw));
                                    uvalSrc = Expression.Property(uvalSrc, nameof(Task<int>.Result));
                                }
                                else
                                {
                                    mvalSrc = Expression.Call(memInst, readStructsFunc, mvalSrc, Expression.Constant(a.Raw));
                                    uvalSrc = Expression.Call(memInst, writeStructsFunc, uvalSrc, Expression.Constant(1), Expression.Constant(a.Raw));
                                    uvalSrc = Expression.Property(uvalSrc, nameof(Task<int>.Result));
                                }
                                break;
                            }
                        case OmsiObjArrayPtrAttribute a:
                            {
                                var readObjsFunc = typeof(Memory).GetMethod(nameof(Memory.ReadMemoryObjArray))
                                    .MakeGenericMethod(a.ObjType);
                                var writeObjsFunc = typeof(Memory).GetMethod(nameof(Memory.AllocateAndInitStructArray))
                                    .MakeGenericMethod(typeof(int));
                                var selectAddressesFunc = typeof(ReflectionCacheExpression<NativeStruct, LocalStruct>).GetMethod(nameof(GetObjectAddresses), BindingFlags.Static | BindingFlags.NonPublic);

                                mvalSrc = Expression.Call(memInst, readObjsFunc, mvalSrc);
                                uvalSrc = Expression.Call(null, selectAddressesFunc, uvalSrc);
                                uvalSrc = Expression.Property(Expression.Call(memInst, writeObjsFunc, uvalSrc), nameof(Task<int>.Result));
                                break;
                            }
                        case OmsiStrArrayPtrAttribute a:
                            {
                                var toLocal = typeof(Memory).GetMethod(nameof(Memory.ReadMemoryStringArray));
                                var toNative = typeof(ReflectionCacheExpression<NativeStruct, LocalStruct>).GetMethod(nameof(AllocateStrings), BindingFlags.Static | BindingFlags.NonPublic);

                                mvalSrc = Expression.Call(memInst, toLocal, mvalSrc, Expression.Constant(a.Wide), Expression.Constant(a.Raw), Expression.Constant(a.Pascal));
                                uvalSrc = Expression.Call(null, toNative, memInst, uvalSrc, Expression.Constant(a.Wide), Expression.Constant(a.Raw));
                                break;
                            }
                        case OmsiMarshallerAttribute a:
                            throw new NotImplementedException($"Attribute {attr.GetType().FullName} is not yet supported by the marshaller!");

                        default:
                            break;
                    }
                }

                // Now that the field has been marshalled, write it back to the destination object
                //try
                //{
                var massgn = Expression.Assign(mvalDst, mvalSrc);
                var uassgn = Expression.Assign(uvalDst, uvalSrc);
                mblocks.Add(massgn);
                ublocks.Add(uassgn);
                /*}
                catch (Exception)
                {
                    var massgn = Expression.Assign(mvalDst, Expression.Default(localFld.FieldType));
                    var uassgn = Expression.Assign(uvalDst, Expression.Default(nativeFld.FieldType));
                    mblocks.Add(massgn);
                    ublocks.Add(uassgn);
                }*/
#if DEBUG
            }
            catch (Exception ex)
            {
                throw new FieldAccessException($"Failed to marshal field '{nativeFld.Name}' in '{nativeFld.ReflectedType.FullName}'. \n" +
                    $"Attributes:\n{GetCustomAttributeDebugString(nativeFld)}\n" +
                    $"Failed with internal exception:\n", ex);
            }
#endif
        }

        //mblocks.Add(Expression.Call(null, typeof(Debugger).GetMethod(nameof(Debugger.Break))));
        mblocks.Add(mret);
        ublocks.Add(uret);
        var mblock = Expression.Block(localType, new[] { mret }, mblocks);
        var ublock = Expression.Block(nativeType, new[] { uret }, ublocks);

        var mfunc = Expression.Lambda<MarshalStruct>(mblock, mobj);
        var ufunc = Expression.Lambda<UnMarshalStruct>(ublock, uobj);

        marshalStructFunc = mfunc.Compile();
        unmarshalStructFunc = ufunc.Compile();

        compiled = true;
    }

    private static string GetCustomAttributeDebugString(FieldInfo field)
    {
        return string.Join("\n\t", field.GetCustomAttributes(false)
            .Select(x => string.Format("  {0}: {1}", x.GetType().Name, string.Join(", ", x.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Select(attProp => $"{attProp.Name}={attProp.GetValue(x)}")))));
    }

    private static int[] GetObjectAddresses(OmsiObject[] objs) => objs.Select(x => x.Address).ToArray();

    private static int AllocateStrings(Memory mem, string[] val, bool wide, bool raw)
    {
        // TODO: I might add a dedicated method for allocating string arrays
        // https://github.com/space928/Omsi-Extensions/issues/121
        var stringTasks = val.Select(x => mem.AllocateString(x, wide, 1, raw));
        var strings = Task.WhenAll(stringTasks);
        return mem.AllocateAndInitStructArray(strings.Result).Result;
    }
}
