﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// Memory management class for accessing and marshaling OMSI's memory
    /// </summary>
#if DEBUG
    public class Memory : IDisposable
#else
    internal class Memory : IDisposable
#endif
    {
        private Process omsiProcess;
        private IntPtr omsiProcessHandle;
        private bool isLocalPlugin;
        private OmsiRemoteMethods remoteMethods;

        public Process OmsiProcess => omsiProcess;
        public IntPtr OmsiProcessHandle => omsiProcessHandle;
        public OmsiRemoteMethods RemoteMethods
        {
            get => remoteMethods;
            internal set => remoteMethods = value;
        }

        /// <summary>
        /// Buffer to read remote memory into. 4k should be large enough for any structs we encounter as they
        /// have to fit on the stack anyway.
        /// </summary>
        private readonly ThreadLocal<byte[]> readBuffer = new(() => new byte[4096], true);
        /// <summary>
        /// Buffer to stage remote memory writes in. 4k should be large enough for any structs we encounter as
        /// they have to fit on the stack anyway.
        /// </summary>
        private readonly ThreadLocal<byte[]> writeBuffer = new(() => new byte[4096], false);

        /// <summary>
        /// Attempts to attach to a given process as a debugger.
        /// </summary>
        /// <param name="procName">The name of the process to attach to eg: "OMSI.exe"</param>
        /// <returns>Whether the process attached successfully</returns>
        public bool Attach(string procName)
        {
            if (Process.GetProcessesByName(procName).Length <= 0)
                return false;

            omsiProcess = Process.GetProcessesByName(procName)[0];
            try
            {
                Process.EnterDebugMode();
            }
#if DEBUG
            catch (Exception)
            {

                Console.WriteLine("The plugin couldn't grant itself debug privileges, it may fail to attach to the game.");
                //Console.WriteLine(e);
            }
#else
            catch { }
#endif
            isLocalPlugin = Process.GetCurrentProcess().ProcessName == omsiProcess.ProcessName;
            if (!isLocalPlugin)
                omsiProcessHandle =
                    Imports.OpenProcess(
                        (int)(Imports.OpenProcessFlags.PROCESS_VM_OPERATION |
                               Imports.OpenProcessFlags.PROCESS_VM_READ | Imports.OpenProcessFlags.PROCESS_VM_WRITE),
                        false, omsiProcess.Id);
#if DEBUG
            if (!isLocalPlugin && omsiProcessHandle == IntPtr.Zero)
                Console.WriteLine("Couldn't open a handle to the process! Some memory operations may fail!");
#endif

            return true;
        }

        #region Memory Writing Methods
        /// <summary>
        /// Sets the value of a struct at a given address.
        /// </summary>
        /// <typeparam name="T">The managed type of the struct to set. 
        /// This must have a one to one mapping with the unmanaged data.</typeparam>
        /// <param name="address">The address of the data to set</param>
        /// <param name="value">The new value to set; this must be of the 
        /// correct data type to avoid memory corruption</param>
        /// <exception cref="MemoryAccessException"/>
        public void WriteMemory<T>(int address, T value) where T : unmanaged
        {
            if (isLocalPlugin)
            {
                unsafe
                {
                    if (address == 0)
#if DEBUG && SILENCE_ACCESS_VIOLATION
                    {
                        Debug.WriteLine($"Couldn't write {byteSize} bytes of process memory @ 0x{address:X8}!\n{new System.Diagnostics.StackTrace(true)}");
                        return;
                    }
#else

                        throw new MemoryAccessException($"Couldn't write {sizeof(T)} bytes of process memory @ 0x{address:X8}!", address);
#endif
                    Unsafe.Copy((void*)address, ref value);
                    return;
                }
            }

            var size = StructureToByteArray(value, writeBuffer.Value, 0);

            if (!Imports.WriteProcessMemory((int)omsiProcessHandle, address, writeBuffer.Value, size, out _))
                throw new MemoryAccessException($"Couldn't write {Unsafe.SizeOf<T>()} bytes of process memory @ 0x{address:X8}!", address);
        }

        /// <inheritdoc cref="WriteMemory{T}(int, T)"/>
        public void WriteMemory<T>(uint address, T value) where T : unmanaged
        {
            if (isLocalPlugin)
            {
                unsafe
                {
                    if (address == 0)
#if DEBUG && SILENCE_ACCESS_VIOLATION
                    {
                        Debug.WriteLine($"Couldn't write {byteSize} bytes of process memory @ 0x{address:X8}!\n{new System.Diagnostics.StackTrace(true)}");
                        return;
                    }
#else

                        throw new MemoryAccessException($"Couldn't write {sizeof(T)} bytes of process memory @ 0x{address:X8}!", address);
#endif

                    Unsafe.Copy((void*)address, ref value);
                    return;
                }
            }

            var size = StructureToByteArray(value, writeBuffer.Value, 0);

            if (!Imports.WriteProcessMemory((int)omsiProcessHandle, unchecked((int)address), writeBuffer.Value, size, out _))
                throw new MemoryAccessException($"Couldn't write {Unsafe.SizeOf<T>()} bytes of process memory @ 0x{address:X8}!", address);
        }

        /// <summary>
        /// Sets the value of an array of structs starting at a given address.
        /// </summary>
        /// <remarks>
        /// This methods just copies a block of data across without regard to the structure or metadata
        /// of a dynamic array. If you want to copy data to an array use a <seealso cref="MemArray{InternalStruct, Struct}"/>. 
        /// Only use this method if you know what you're doing.
        /// </remarks>
        /// <typeparam name="T">The managed type of the struct to set. 
        /// This must have a one to one mapping with the unmanaged data.</typeparam>
        /// <param name="address">The address of the data to set</param>
        /// <param name="values">The array of new values to set; this must be of the 
        /// correct data type to avoid memory corruption</param>
        /// <exception cref="MemoryAccessException"/>
        public void WriteMemory<T>(int address, T[] values) where T : unmanaged
        {
            WriteMemory(address, values.AsSpan());
        }

        /// <inheritdoc cref="WriteMemory{T}(int, T[])"/>
        public void WriteMemory<T>(uint address, T[] values) where T : unmanaged
        {
            WriteMemory(unchecked((int)address), values);
        }

        /// <inheritdoc cref="WriteMemory{T}(int, T[])"/>
        public void WriteMemory<T>(int address, Span<T> values) where T : unmanaged
        {
            if (values.IsEmpty)
                return;

            if (isLocalPlugin)
            {
                unsafe
                {
                    if (address == 0)
#if DEBUG && SILENCE_ACCESS_VIOLATION
                    {
                        Debug.WriteLine($"Couldn't write {byteSize} bytes of process memory @ 0x{address:X8}!\n{new System.Diagnostics.StackTrace(true)}");
                        return;
                    }
#else

                        throw new MemoryAccessException($"Couldn't write {sizeof(T) * values.Length} bytes of process memory @ 0x{address:X8}!", address);
#endif

                    fixed (T* valuesPtr = values)
                    {
                        int size = sizeof(T) * values.Length;
                        Buffer.MemoryCopy(valuesPtr, (void*)address, size, size);
                    }
                    return;
                }
            }

            var bytes = MemoryMarshal.AsBytes(values);
            unsafe
            {
                fixed (byte* bytesPtr = bytes)
                {
                    if (!Imports.WriteProcessMemory((int)omsiProcessHandle, address, ref Unsafe.AsRef<byte>(bytesPtr), bytes.Length, out _))
                        throw new MemoryAccessException($"Couldn't write {bytes.Length} bytes of process memory @ 0x{address:X8}!", address);
                }
            }
            //Imports.WriteProcessMemory((int)omsiProcessHandle, address, ref MemoryMarshal.GetReference(bytes), bytes.Length, out _);
        }

        /// <inheritdoc cref="WriteMemory{T}(int, T[])"/>
        public void WriteMemory<T>(uint address, Span<T> values) where T : unmanaged
        {
            WriteMemory(unchecked((int)address), values);
        }

        /// <summary>
        /// Mildly quickly copies a block of data from one place to another in remote memory. <para/>
        /// Copies the data to a temp buffer before copying to the destination so it's not very fast.
        /// </summary>
        /// <param name="src">Address to copy from</param>
        /// <param name="dst">Address to copy to</param>
        /// <param name="length">Number of bytes to copy</param>
        /// <exception cref="MemoryAccessException"/>
        internal void CopyMemory(int src, int dst, int length)
        {
            if (isLocalPlugin)
            {
                unsafe
                {
                    if (src == 0 || dst == 0)
                        throw new MemoryAccessException($"Couldn't copy {length} bytes of process memory from 0x{src:X8} to 0x{dst:X8}!");

                    Buffer.MemoryCopy((void*)src, (void*)dst, length, length);
                    return;
                }
            }

            byte[] buffer = new byte[length];
            if (!Imports.ReadProcessMemory((int)omsiProcessHandle, src, buffer, length, ref length))
                throw new MemoryAccessException($"Couldn't read {length} bytes of process memory @ 0x{src:X8}!", src);
            if (!Imports.WriteProcessMemory((int)omsiProcessHandle, dst, buffer, length, out _))
                throw new MemoryAccessException($"Couldn't write {length} bytes to process memory @ 0x{dst:X8}!", dst);
        }

        /// <inheritdoc cref="CopyMemory(int, int, int)"/>
        internal void CopyMemory(uint src, uint dst, int length)
        {
            if (isLocalPlugin)
            {
                unsafe
                {
                    if (src == 0 || dst == 0)
                        throw new MemoryAccessException($"Couldn't copy {length} bytes of process memory from 0x{src:X8} to 0x{dst:X8}!");

                    Buffer.MemoryCopy((void*)src, (void*)dst, length, length);
                    return;
                }
            }

            byte[] buffer = new byte[length];
            if (!Imports.ReadProcessMemory((int)omsiProcessHandle, unchecked((int)src), buffer, length, ref length))
                throw new MemoryAccessException($"Couldn't read {length} bytes of process memory @ 0x{src:X8}!", src);
            if (!Imports.WriteProcessMemory((int)omsiProcessHandle, unchecked((int)dst), buffer, length, out _))
                throw new MemoryAccessException($"Couldn't write {length} bytes to process memory @ 0x{dst:X8}!", dst);
        }

        /// <summary>
        /// Writes the value of a struct to an array of structs at a given address. <para/>
        /// Performs bounds checking.
        /// </summary>
        /// <typeparam name="T">The type of the struct to write</typeparam>
        /// <param name="address">The address of the array to write to</param>
        /// <param name="value">The value of the new element</param>
        /// <param name="index">The index of the element to write to</param>
        /// <param name="itemPointer">Whether to write to the item pointed to by array (ie the array is an array of pointers)</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="MemoryAccessException"/>
        public void WriteMemoryArrayItemSafe<T>(int address, T value, int index, bool itemPointer = false) where T : unmanaged
        {
            int arr = ReadMemory<int>(address);
            int len = ReadMemory<int>(arr - 4);
            if (index < 0 || index >= len)
                throw new ArgumentOutOfRangeException($"Tried to write to item {index} of an {len} element array!");
            if (itemPointer)
            {
                int addr = ReadMemory<int>(arr + index * 4);
                WriteMemory(addr, value);
            }
            else
            {
                WriteMemory(arr + index * (Unsafe.SizeOf<T>()), value);
            }
        }

        /// <summary>
        /// Writes the value of a struct to an array of structs at a given address. <para/>
        /// Does NOT perform bounds checking!
        /// </summary>
        /// <typeparam name="T">The type of the struct to write</typeparam>
        /// <param name="address">The address of the array to write to</param>
        /// <param name="value">The value of the new element</param>
        /// <param name="index">The index of the element to write to</param>
        /// <param name="itemPointer">Whether to write to the item pointed to by array (ie the array is an array of pointers)</param>
        /// <exception cref="MemoryAccessException"/>
        public void WriteMemoryArrayItem<T>(int address, T value, int index, bool itemPointer = false) where T : unmanaged
        {
            int arr = ReadMemory<int>(address);
            if (itemPointer)
            {
                int addr = ReadMemory<int>(arr + index * 4);
                WriteMemory(addr, value);
            }
            else
            {
                WriteMemory(arr + index * (Unsafe.SizeOf<T>()), value);
            }
        }

        /// <summary>
        /// Allocates shared memory for an array of structs. <para/>
        /// Does not zero the allocated memory!
        /// </summary>
        /// <remarks>
        /// Memory allocation behaviour is not well defined, memory corruption and leaks are possible!
        /// </remarks>
        /// <typeparam name="T">The type of struct to make an array of</typeparam>
        /// <param name="data">The items to copy into the newly allocated array</param>
        /// <param name="references">Allows the number of references to the array to be specified. 
        /// Used when overwriting existing arrays to prevent the GC from clearing a array referenced 
        /// by multiple objects when one is destroyed.</param>
        /// <param name="raw">If <see langword="true"/>, treat the <c>address</c> as the pointer to the first element 
        /// of the array instead of as a pointer to the array.</param>
        /// <returns>The pointer to the first item of the newly allocated array</returns>
        /// <exception cref="MemoryAccessException"/>
        public async Task<int> AllocateAndInitStructArray<T>(T[] data, int references = 1, bool raw = false) where T : unmanaged
        {
            if (data == null)
                return 0;

            int ptr = await AllocateStructArray<T>(data.Length, references, raw);
            WriteMemory(ptr, data);

            return ptr;
        }

        // Used via reflection by MarshalStruct in ReflectionCache.cs
        internal int AllocateAndInitStructArray<T>(object data, int references = 1, bool raw = false) where T : unmanaged
        {
            return AllocateAndInitStructArray<T>(data as T[], references, raw).Result;
        }

        /// <summary>
        /// Allocates shared memory for an array of structs. <para/>
        /// Does not zero the allocated memory!
        /// </summary>
        /// <remarks>
        /// Memory allocation behaviour is not well defined, memory corruption and leaks are possible!
        /// </remarks>
        /// <typeparam name="T">The type of struct to make an array of</typeparam>
        /// <param name="capacity">The number of items to allocate space for</param>
        /// <param name="references">Allows the number of references to the array to be specified. 
        /// Used when overwriting existing arrays to prevent the GC from clearing a array referenced 
        /// by multiple objects when one is destroyed.</param>
        /// <param name="raw">If <see langword="true"/>, treat the <c>address</c> as the pointer to the first element 
        /// of the array instead of as a pointer to the array.</param>
        /// <returns>The pointer to the first item of the newly allocated array</returns>
        /// <exception cref="MemoryAccessException"/>
        /// <exception cref="NotImplementedException">Writing to raw struct arrays is not yet supported.</exception>
        public async Task<int> AllocateStructArray<T>(int capacity, int references = 1, bool raw = false) where T : unmanaged
        {
            /*
             * DynArray struct layout:
             * 0 - / NReferences (int)
             * 1 - |
             * 2 - |
             * 3 - \
             * 4 - / Length (int)
             * 5 - |
             * 6 - |
             * 7 - \
             * 8 - / Item1 (T)
             * ...
             * 8+sizeof(T) - Item2
             * ...
             */

            if (raw)
                throw new NotImplementedException("Writing to raw struct arrays is not yet supported.");

            int itemSize = Unsafe.SizeOf<T>();
            //var ptr = Marshal.AllocCoTaskMem(capacity * itemSize + 8).ToInt32();
            int ptr = await AllocRemoteMemory(capacity * itemSize + 8);
            // Write the array metadata
            WriteMemory(ptr, references);
            WriteMemory(ptr + 0x4, capacity);

            return ptr + 8;
        }

        /// <summary>
        /// Allocates shared memory for a struct. <para/>
        /// Does not zero the allocated memory! <para/>
        /// Make sure to <seealso cref="UnMarshalStruct{OutStruct, InStruct}(InStruct)"/>
        /// any structs that require it before passing them into this method.
        /// </summary>
        /// <remarks>
        /// Memory allocation behaviour is not well defined, memory corruption and leaks are possible!
        /// </remarks>
        /// <typeparam name="T">Marshallable struct type to allocate memory for</typeparam>
        /// <param name="value">Struct to copy into newly allocated memory</param>
        /// <param name="references">Allows the number of references to the array to be specified. 
        /// Used when overwriting existing arrays to prevent the GC from clearing a array referenced 
        /// by multiple objects when one is destroyed.</param>
        /// <returns>The pointer to the start of the newly allocated struct</returns>
        /// <exception cref="MemoryAccessException"/>
        public async Task<int> AllocateStruct<T>(T value, int references = 1) where T : unmanaged
        {
            /*
             * Managed struct layout:
             * 0 - / NReferences (int)
             * 1 - |
             * 2 - |
             * 3 - \
             * 8 - / StructData (T)
             * ...
             */

            int structSize = Unsafe.SizeOf<T>();
            int ptr = await AllocRemoteMemory(structSize + 4);
            // Write the array metadata
            WriteMemory(ptr, references);
            WriteMemory(ptr + 0x4, value);

            return ptr + 4;
        }

        // Used via reflection by MarshalStruct in ReflectionCache.cs
        internal int AllocateStruct<T>(object value, int references = 1) where T : unmanaged
        {
            return AllocateStruct<T>((T)value, references).Result;
        }

        /// <summary>
        /// Allocates shared memory for a string and copies it accross.
        /// </summary>
        /// <remarks>
        /// Memory allocation behaviour is not well defined, memory corruption and leaks are possible!
        /// </remarks>
        /// <param name="value">The string to copy</param>
        /// <param name="wide">Chooses which encoding to convert the string to</param>
        /// <param name="references">Allows the number of references to the string to be specified. 
        /// Used when overwriting existing strings to prevent the GC from clearing a string referenced 
        /// by multiple objects when one is destroyed.</param>
        /// <param name="raw">Treat the address as a pointer to the first character 
        /// (<c>char *</c>) rather than a pointer to a pointer.</param>
        /// <returns>The pointer to the newly allocated string</returns>
        /// <exception cref="MemoryAccessException"/>
        /// <exception cref="NotImplementedException">Writing to raw strings is not yet supported.</exception>
        public async Task<int> AllocateString(string value, bool wide = false, int references = 1, bool raw = false)
        {
            /*
             * AnsiString/UnicodeString struct layout:
             * 0 - / Code page (short)
             * 1 - \ 
             * 2 - / Maximum character size (short)
             * 3 - \
             * 4 - / NReferences (int)
             * 5 - |
             * 6 - |
             * 7 - \
             * 8 - / StrLength (bytes) (int)
             * 9 - |
             * a - |
             * b - \
             * 0 - / String
             * ... |
             * ... \
             * ... - Null
             */

            if (raw)
                throw new NotImplementedException("Writing to raw strings is not yet supported.");

            byte[] buffer;
            if (wide)
                buffer = Encoding.Unicode.GetBytes(value);
            else
                buffer = Encoding.ASCII.GetBytes(value);

            var ptr = await AllocRemoteMemory(buffer.Length + 13);
            int strStart = ptr + 12;
            WriteMemory(strStart, buffer);
            //Marshal.Copy(buffer, 0, strStart, buffer.Length);
            // Write the string metadata
            WriteMemory(ptr, (short)1252);
            WriteMemory(ptr + 0x2, (short)(wide ? 2 : 1));
            WriteMemory(ptr + 0x4, references);
            WriteMemory(ptr + 0x8, value.Length);
            // Write null terminator
            WriteMemory(strStart + buffer.Length, (byte)0);

            return strStart;
        }

        /// <summary>
        /// Sets the value of a string at a given address.
        /// </summary>
        /// <param name="address">The address of the data to set</param>
        /// <param name="value">The new value of the string to set; this must be of the 
        /// correct encoding to avoid memory corruption</param>
        /// <param name="wide">Chooses which encoding to convert the string to</param>
        /// <param name="copyReferences">Copy the number of references from the string 
        /// at the address given. Used when overwriting existing strings to prevent the 
        /// GC from clearing a string referenced by multiple objects when one is destroyed.</param>
        /// <exception cref="MemoryAccessException"/>
        /// <exception cref="NotImplementedException">Writing to raw strings is not yet supported.</exception>
        public void WriteMemory(int address, string value, bool wide = false, bool copyReferences = true)
        {
            int refs = 1;
            if (copyReferences)
                refs = ReadMemory<int>(ReadMemory<int>(address) - 0x8);

            WriteMemory(address, AllocateString(value, wide, refs).Result);
        }

        /// <summary>
        /// Sets the value of a string at a given address.
        /// </summary>
        /// <param name="address">The address of the data to set</param>
        /// <param name="value">The new value of the string to set; this must be of the 
        /// correct encoding to avoid memory corruption</param>
        /// <param name="strType">The type of string to copy to</param>
        /// <exception cref="MemoryAccessException"/>
        /// <exception cref="NotImplementedException">Writing to raw strings is not yet supported.</exception>
        public void WriteMemory(int address, string value, StrPtrType strType)
        {
            WriteMemory(address, value,
                (strType & StrPtrType.Wide) != 0);
        }

        /// <inheritdoc cref="WriteMemory"/>
        public async Task WriteMemoryAsync(int address, string value, bool wide = false, bool copyReferences = true)
        {
            int refs = 1;
            if (copyReferences)
                refs = ReadMemory<int>(ReadMemory<int>(address) - 0x8);

            WriteMemory(address, await AllocateString(value, wide, refs));
        }
        #endregion

        #region Memory Reading Methods
        /// <summary>
        /// Reads a struct/value from unmanaged memory and returns it.
        /// </summary>
        /// <typeparam name="T">The type of the value/struct to read</typeparam>
        /// <param name="address">The address to read from</param>
        /// <returns>The value of the struct/value at the given address.</returns>
        /// <exception cref="MemoryAccessException"/>
        public T ReadMemory<T>(int address) where T : unmanaged
        {
            if (isLocalPlugin)
            {
                unsafe
                {
                    if (address == 0)
#if DEBUG && SILENCE_ACCESS_VIOLATION
                    {
                        Debug.WriteLine($"Couldn't read {byteSize} bytes of process memory @ 0x{address:X8}!\n{new System.Diagnostics.StackTrace(true)}");
                        return new T();
                    }
#else

                        throw new MemoryAccessException($"Couldn't read {sizeof(T)} bytes of process memory @ 0x{address:X8}!", address);
#endif
                    return Unsafe.ReadUnaligned<T>((void*)address);
                }
            }

            int byteSize = Unsafe.SizeOf<T>();
            if (byteSize > readBuffer.Value.Length)
                throw new ArgumentException($"Couldn't read memory for object of type {typeof(T).Name} @ 0x{address:X8}; it wouldn't fit in the read buffer!");
            int bytesRead = -1;

            if (!Imports.ReadProcessMemory((int)omsiProcessHandle, address, readBuffer.Value, byteSize, ref bytesRead))
#if DEBUG && SILENCE_ACCESS_VIOLATION
            {
                Debug.WriteLine($"Couldn't read {byteSize} bytes of process memory @ 0x{address:X8}!\n{new System.Diagnostics.StackTrace(true)}");
                return new T();
            }
#else
                throw new MemoryAccessException($"Couldn't read {byteSize} bytes of process memory @ 0x{address:X8}!", address);
#endif

            return ByteArrayToStructure<T>(readBuffer.Value);
        }

        /// <inheritdoc cref="ReadMemory{T}(int)"/>
        public T ReadMemory<T>(IntPtr address) where T : unmanaged
        {
            return ReadMemory<T>(address.ToInt32());
        }

        /// <inheritdoc cref="ReadMemory{T}(int)"/>
        public T ReadMemory<T>(uint address) where T : unmanaged
        {
            return ReadMemory<T>(unchecked((int)address));
        }

        /// <summary>
        /// Reads and constructs an OmsiObject from unmanaged memory.
        /// </summary>
        /// <typeparam name="T">The type of OmsiObject to construct</typeparam>
        /// <param name="address">The address of the pointer to the OmsiObject</param>
        /// <returns>A new OmsiObject.</returns>
        /// <exception cref="MemoryAccessException"/>
        public T ReadMemoryObject<T>(int address) where T : OmsiObject, new()
        {
            if (address == 0)
                return null;
            var addr = ReadMemory<int>(address);
            if (addr == 0)
                return null;
            var obj = new T();
            obj.InitObject(this, addr);
            return obj;
        }

        /// <summary>
        /// Reads and constructs an OmsiObject from unmanaged memory.
        /// </summary>
        /// <remarks>
        /// This version of the method performs a null-check on the base address before the offset is applied 
        /// to prevent attempting to dereference an objject belonging to a null-parent.
        /// </remarks>
        /// <typeparam name="T">The type of OmsiObject to construct</typeparam>
        /// <param name="address">The address of the object this field belongs to.</param>
        /// <param name="offset">The offset from the base address of the object.</param>
        /// <param name="raw">When false, dereferences the object pointer again before construcing the new object.</param>
        /// <returns>A new OmsiObject.</returns>
        /// <exception cref="MemoryAccessException"/>
        public T ReadMemoryObject<T>(int address, int offset, bool raw = true) where T : OmsiObject, new()
        {
            if (address == 0)
                return null;
            var addr = ReadMemory<int>(address + offset);
            if (addr == 0)
                return null;
            if (raw)
            {
                addr = ReadMemory<int>(addr);
                if (addr == 0)
                    return null;
            }
            var obj = new T();
            obj.InitObject(this, addr);
            return obj;
        }

        /// <summary>
        /// Returns the value of a null terminated/length prefixed string at a given address.
        /// </summary>
        /// <param name="address">The address to read from</param>
        /// <param name="strType">Flags specifying how to decode the string.</param>
        /// <returns>The value of the string at the given address.</returns>
        /// <exception cref="MemoryAccessException"/>
        public string ReadMemoryString(int address, StrPtrType strType)
        {
            return ReadMemoryString(address,
                (strType & StrPtrType.Wide) != 0,
                (strType & StrPtrType.Raw) != 0,
                (strType & StrPtrType.Pascal) != 0);
        }

        /// <summary>
        /// Returns the value of a null terminated/length prefixed string at a given address.
        /// </summary>
        /// <param name="address">The address to read from</param>
        /// <param name="raw">Treat the address as a pointer to the first character 
        /// (<c>char *</c>) rather than a pointer to a pointer.</param>
        /// <param name="pascalString">Whether the string can be treated as a length prefixed (pascal) 
        /// string, which is much faster to read</param>
        /// <returns>The value of the string at the given address.</returns>
        /// <exception cref="MemoryAccessException">Only in cases where an unusually long string is 
        /// attempted to be read. For null pointers, this method returns null</exception>
        public string ReadMemoryString(int address, bool wide = false, bool raw = false, bool pascalString = true)
        {
            string ret;
            int i = address;
            if (!raw)
                i = ReadMemory<int>(address);

            if (i == 0)
                return null;

            if (pascalString)
            {
                uint strLen = ReadMemory<uint>(i - 4);
                if (strLen > 4096)
                    throw new MemoryAccessException($"Tried reading a very long string ({strLen} > 4096 characters long). This is probably not a valid string", i);
                if (wide)
                    strLen *= 2;
                var bytes = ReadMemory(i, (int)strLen, readBuffer.Value);
                ret = wide ? new string(MemoryMarshal.Cast<byte, char>(bytes)) : Encoding.ASCII.GetString(bytes);
            }
            else
            {
                var sb = new StringBuilder();
                try
                {
                    // Cache the read buffer to save a few checks (that the compiler would probably have hoisted out anyway)
                    var readBuff = readBuffer.Value;
                    //int readSize = 16;
                    // TODO: Rewrite this to read chunks of memory all at once instead of one char at a time...
                    while (true)
                    {
                        var bytes = ReadMemory(i, wide ? 2 : 1, readBuff);
                        if (bytes.Count == 0 || (wide ? (bytes[0] | bytes[1]) : bytes[0]) == 0)
                            break;

                        //sb.Append(wide ? Encoding.Unicode.GetString(bytes) : Encoding.ASCII.GetString(bytes));
                        sb.Append(wide ? MemoryMarshal.Cast<byte, char>(bytes)[0] : (char)bytes[0]);
                        i++;
                        if (wide)
                            i++;
                    }
                }
                catch (MemoryAccessException) { return null; }
                ret = sb.ToString();
            }

            return ret;
        }

        /// <summary>
        /// Reads raw bytes from unmanaged memory at a given address.
        /// </summary>
        /// <param name="offset">The address to start reading from</param>
        /// <param name="size">The number of bytes to read</param>
        /// <param name="buffer">The buffer to read into</param>
        /// <exception cref="MemoryAccessException"/>
        /// <exception cref="ArgumentException"/>
        public ArraySegment<byte> ReadMemory(int offset, int size, byte[] buffer)
        {
            int bytesRead = 0;
            if (size == 0)
                return new(buffer, 0, 0);
            if (size > buffer.Length)
                throw new ArgumentException($"Couldn't read memory for object of type byte[] @ {offset}; it wouldn't fit in the read buffer (tried reading {size} bytes into {buffer.Length})!");
            if (isLocalPlugin)
            {
                if (offset == 0)
                    throw new MemoryAccessException($"Couldn't read {size} bytes of process memory @ 0x{offset:X8}!", offset);

                Marshal.Copy((nint)offset, buffer, 0, size);
                bytesRead = size;
            }
            else
            {
                if (!Imports.ReadProcessMemory((int)omsiProcessHandle, offset, buffer, size, ref bytesRead))
                    throw new MemoryAccessException($"Couldn't read {size} bytes of process memory @ 0x{offset:X8}!", offset);
            }

            return new(buffer, 0, bytesRead);
        }
        #endregion

        #region Memory Array Item Reading Methods
        /// <summary>
        /// Reads the value of an <seealso cref="OmsiObject"/> from an array at a given address. <para/>
        /// Performs bounds checking for people who don't like corrupting data.
        /// </summary>
        /// <typeparam name="T">The type of the <seealso cref="OmsiObject"/> to read</typeparam>
        /// <param name="address">The address of the array to read from</param>
        /// <param name="index">The index of the element to read from</param>
        /// <exception cref="MemoryAccessException"/>
        public T ReadMemoryArrayItemObjSafe<T>(int address, int index) where T : OmsiObject, new()
        {
            int arr = ReadMemory<int>(address);
            int len = ReadMemory<int>(arr - 4);
            if (index < 0 || index >= len)
                throw new IndexOutOfRangeException($"Tried to access element {index} in an array of {len} elements!");
            return ReadMemoryArrayItemObj<T>(address, index);
        }

        /// <summary>
        /// Reads the value of a <seealso cref="OmsiObject"/> from an array at a given address. <para/>
        /// Does not perform any bounds checking!
        /// </summary>
        /// <typeparam name="T">The type of the <seealso cref="OmsiObject"/> to read</typeparam>
        /// <param name="address">The address of the array to read from</param>
        /// <param name="index">The index of the element to read from</param>
        /// <exception cref="MemoryAccessException"/>
        public T ReadMemoryArrayItemObj<T>(int address, int index) where T : OmsiObject, new()
        {
            int arr = ReadMemory<int>(address);
            var n = new T();
            n.InitObject(this, ReadMemory<int>(arr + index * 4));
            return n;
        }

        /// <summary>
        /// Reads the value of a struct from an array of structs at a given address. <para/>
        /// Performs bounds checking for people who don't like corrupting data.
        /// </summary>
        /// <typeparam name="T">The type of the struct to read</typeparam>
        /// <param name="address">The address of the array to read from</param>
        /// <param name="index">The index of the element to read from</param>
        /// <param name="itemPointer">Whether the item in the array is a pointer to the desired item 
        /// (ie and array of pointers to <typeparamref name="T"/>)</param>
        /// <exception cref="MemoryAccessException"/>
        /// <exception cref="IndexOutOfRangeException"/>
        public T ReadMemoryArrayItemSafe<T>(int address, int index, bool itemPointer = false) where T : unmanaged
        {
            int arr = ReadMemory<int>(address);
            int len = ReadMemory<int>(arr - 4);
            if (index < 0 || index >= len)
                throw new IndexOutOfRangeException($"Tried to access element {index} in an array of {len} elements!");
            return ReadMemoryArrayItem<T>(address, index, itemPointer);
        }

        /// <summary>
        /// Reads the value of a struct from an array of structs at a given address. <para/>
        /// Does not perform any bounds checking!
        /// </summary>
        /// <typeparam name="T">The type of the struct to read</typeparam>
        /// <param name="address">The address of the array to read from</param>
        /// <param name="index">The index of the element to read from</param>
        /// <param name="itemPointer">Whether the item in the array is a pointer to the desired item 
        /// (ie and array of pointers to <typeparamref name="T"/>)</param>
        /// <exception cref="MemoryAccessException"/>
        public T ReadMemoryArrayItem<T>(int address, int index, bool itemPointer = false) where T : unmanaged
        {
            int arr = ReadMemory<int>(address);
            if (itemPointer)
                return ReadMemory<T>(ReadMemory<int>(arr + index * 4));
            else
                return ReadMemory<T>(arr + index * Unsafe.SizeOf<T>());
        }

        /// <summary>
        /// Reads the value of a <seealso cref="string"/> from an array of strings at a given address. <para/>
        /// Performs bounds checking for people who don't like corrupting data.
        /// </summary>
        /// <typeparam name="T">The type of the string to read</typeparam>
        /// <param name="address">The address of the array to read from</param>
        /// <param name="index">The index of the element to read from</param>
        /// <exception cref="MemoryAccessException"/>
        /// <exception cref="IndexOutOfRangeException"/>
        public string ReadMemoryArrayItemStringSafe(int address, int index, bool wide = false)
        {
            int arr = ReadMemory<int>(address);
            int len = ReadMemory<int>(arr - 4);
            if (index < 0 || index >= len)
                throw new IndexOutOfRangeException($"Tried to access element {index} in an array of {len} elements!");
            return ReadMemoryArrayItemString(address, index, wide);
        }

        /// <summary>
        /// Reads the value of a <seealso cref="string"/> from an array of strings at a given address. <para/>
        /// Does not perform any bounds checking!
        /// </summary>
        /// <typeparam name="T">The type of the string to read</typeparam>
        /// <param name="address">The address of the array to read from</param>
        /// <param name="index">The index of the element to read from</param>
        /// <exception cref="MemoryAccessException"/>
        public string ReadMemoryArrayItemString(int address, int index, bool wide = false)
        {
            int arr = ReadMemory<int>(address);
            return ReadMemoryString(arr + index * 4, wide);
        }
        #endregion

        #region Memory Array Reading Methods
        /// <summary>
        /// Reads an array of OmsiObjects from unmanaged memory at a given address.
        /// </summary>
        /// <typeparam name="T">The type of the OmsiObject to return</typeparam>
        /// <param name="address">The address of the array to read from</param>
        /// <returns>The parsed array of OmsiObjects.</returns>
        /// <exception cref="MemoryAccessException"/>
        public T[] ReadMemoryObjArray<T>(int address) where T : OmsiObject, new()
        {
            int arr = ReadMemory<int>(address);
            if (arr == 0)
                return Array.Empty<T>();
            int len = ReadMemory<int>(arr - 4);
            T[] ret = new T[len];
            for (int i = 0; i < len; i++)
            {
                var objAddr = ReadMemory<int>(arr + i * 4);
                if (objAddr == 0)
                {
                    ret[i] = null;
                    continue;
                }

                var n = new T();
                n.InitObject(this, objAddr);
                ret[i] = n;
            }

            return ret;
        }

        /// <summary>
        /// Reads an array of structs from unmanaged memory at a given address.
        /// </summary>
        /// <remarks>
        /// This method shouldn't be used in property getters, rather an instance of MemArray should 
        /// be constructed and returned instead to maintain editability of the array.
        /// </remarks>
        /// <typeparam name="T">The type of the struct to return</typeparam>
        /// <param name="address">The address of the array to read from</param>
        /// <param name="raw">If <see langword="true"/>, treat the <c>address</c> as the pointer to the first element 
        /// of the array instead of as a pointer to the array.</param>
        /// <returns>The parsed array of structs.</returns>
        /// <exception cref="MemoryAccessException"/>
        public T[] ReadMemoryStructArray<T>(int address, bool raw = false) where T : unmanaged
        {
            int arr = address;
            if (!raw && address != 0)
                arr = ReadMemory<int>(arr);
            if (arr == 0)
                return null;
            int len = ReadMemory<int>(arr - 4);
            T[] ret = new T[len];
            for (int i = 0; i < len; i++)
                ret[i] = ReadMemory<T>(arr + i * Unsafe.SizeOf<T>());

            return ret;
        }

        /// <summary>
        /// Reads an array of pointers to structs from unmanaged memory at a given address.
        /// </summary>
        /// <typeparam name="T">The type of the struct to return</typeparam>
        /// <param name="address">The address of the array to read from</param>
        /// <returns>The parsed array of structs.</returns>
        /// <exception cref="MemoryAccessException"/>
        public T[] ReadMemoryStructPtrArray<T>(int address) where T : unmanaged
        {
            int arr = ReadMemory<int>(address);
            if (arr == 0 || address == 0)
                return null;
            int len = ReadMemory<int>(arr - 4);
            T[] ret = new T[len];
            for (int i = 0; i < len; i++)
                ret[i] = ReadMemory<T>(ReadMemory<int>(arr + i * Unsafe.SizeOf<T>()));

            return ret;
        }

        /// <summary>
        /// Reads an array of strings from unmanaged memory at a given address.
        /// </summary>
        /// <param name="address">The address of the array to read from</param>
        /// <param name="raw">If <see langword="true"/>, treat the <c>address</c> as the pointer to the first element 
        /// of the array instead of as a pointer to the array.</param>
        /// <param name="pascal">Whether the string can be treated as a length prefixed (pascal) 
        /// string, which is much faster to read</param>
        /// <returns>The parsed array of strings.</returns>
        /// <exception cref="MemoryAccessException"/>
        public string[] ReadMemoryStringArray(int address, bool wide = false, bool raw = false, bool pascal = true)
        {
            int arr = address;
            if (!raw && address != 0)
                arr = ReadMemory<int>(arr);
            if (arr == 0)
                return null;
            int len = ReadMemory<int>(arr - 4);
            string[] ret = new string[len];
            for (int i = 0; i < len; i++)
                ret[i] = ReadMemoryString(arr + i * 4, wide, raw: false, pascal);

            return ret;
        }
        #endregion

        #region Conversion

        /// <summary>
        /// Marshals any data in a struct which couldn't be automatically marshalled by Marshal.PtrToStruct.
        /// This method uses reflection and as such isn't very fast.
        /// </summary>
        /// <typeparam name="OutStruct">The type of the struct to output; MUST have corresponding 
        /// field names to InStruct</typeparam>
        /// <typeparam name="InStruct">The type of the struct to marshal</typeparam>
        /// <param name="obj">The structs as marshaled by Marshal.PtrToStruct</param>
        /// <returns>The fully marshalled structs.</returns>
        public OutStruct[] MarshalStructs<OutStruct, InStruct>(InStruct[] obj)
           where OutStruct : struct
           where InStruct : unmanaged
        {
            if (obj == null) return null;
            OutStruct[] ret = new OutStruct[obj.Length];
            for (int i = 0; i < obj.Length; i++)
                ret[i] = MarshalStruct<OutStruct, InStruct>(obj[i]);

            return ret;
        }

        // Used via reflection by MarshalStruct in ReflectionCache.cs
        internal object MarshalStructs<OutStruct, InStruct>(object obj)
            where OutStruct : struct
           where InStruct : unmanaged
        {
            return MarshalStructs<OutStruct, InStruct>(obj as InStruct[]);
        }

        /// <summary>
        /// Unmarshals any data in a struct which can't be automatically marshalled by Marshal.StructToPtr.
        /// This method uses reflection and as such isn't very fast.
        /// </summary>
        /// <typeparam name="OutStruct">The type of the struct to output; MUST have corresponding 
        /// field names to InStruct</typeparam>
        /// <typeparam name="InStruct">The type of the struct to marshal</typeparam>
        /// <param name="obj">The structs to be marshaled by Marshal.StructToPtr</param>
        /// <returns>The fully unmarshalled structs.</returns>
        public OutStruct[] UnMarshalStructs<OutStruct, InStruct>(InStruct[] obj)
           where OutStruct : unmanaged
           where InStruct : struct
        {
            OutStruct[] ret = new OutStruct[obj.Length];
            for (int i = 0; i < obj.Length; i++)
                ret[i] = UnMarshalStruct<OutStruct, InStruct>(obj[i]);

            return ret;
        }

        // Used via reflection by MarshalStruct in ReflectionCache.cs
        internal object UnMarshalStructs<OutStruct, InStruct>(object obj)
           where OutStruct : unmanaged
           where InStruct : struct
        {
            return UnMarshalStructs<OutStruct, InStruct>(obj as InStruct[]);
        }

        // Used via reflection by MarshalStruct in ReflectionCache.cs
        internal object MarshalStruct<OutStruct, InStruct>(object obj)
            where OutStruct : struct
            where InStruct : unmanaged
        {
            return MarshalStruct<OutStruct, InStruct>((InStruct)obj);
        }

        // Used via reflection by MarshalStruct in ReflectionCache.cs
        internal object UnMarshalStruct<OutStruct, InStruct>(object obj)
            where OutStruct : unmanaged
            where InStruct : struct
        {
            return UnMarshalStruct<OutStruct, InStruct>((InStruct)obj);
        }

        /// <summary>
        /// Marshals any data in a struct which couldn't be automatically marshalled by Marshal.PtrToStruct.
        /// This method uses reflection and as such isn't very fast.
        /// </summary>
        /// <typeparam name="OutStruct">The type of the struct to output; MUST have corresponding 
        /// field names to InStruct</typeparam>
        /// <typeparam name="InStruct">The type of the struct to marshal</typeparam>
        /// <param name="obj">The struct as marshaled by Marshal.PtrToStruct</param>
        /// <returns>The fully marshalled struct.</returns>
        public OutStruct MarshalStruct<OutStruct, InStruct>(InStruct obj)
            where OutStruct : struct
            where InStruct : unmanaged
        {
            if (typeof(InStruct) == typeof(OutStruct))
                return Unsafe.As<InStruct, OutStruct>(ref obj);

            if (ReflectionCacheExpression<InStruct, OutStruct>.Supported)
            {
                if (!ReflectionCacheExpression<InStruct, OutStruct>.Compiled)
                    ReflectionCacheExpression<InStruct, OutStruct>.Compile(this);

                var ret1 = ReflectionCacheExpression<InStruct, OutStruct>.MarshalStructFunc(obj);
                return ret1;
            }
            else
            {
                var refl = ReflectionCache.GetOrBuildReflectionCache(this, typeof(InStruct), typeof(OutStruct));
                var ret1 = new OutStruct();
                var objRef = __makeref(obj);
                var retRef = __makeref(ret1);
                foreach (var f in refl.fieldMapping)
                {
#if DEBUG
                    try
                    {
#endif
                        var native = f.native.GetValueDirect(objRef);
                        if (f.toLocal != null)
                            f.local.SetValueDirect(retRef, f.toLocal(native));
                        else
                            f.local.SetValueDirect(retRef, native);
#if DEBUG
                    }
                    catch (Exception ex)
                    {
                        throw new FieldAccessException($"Failed to marshal field '{f.native.Name}' in '{f.native.DeclaringType.Name}'. \n" +
                            $"Attributes:\n{GetCustomAttributeDebugString(f.native)}\n" +
                            $"Failed with internal exception:", ex);
                    }
#endif
                }
                return ret1;
            }
        }

        private static string GetCustomAttributeDebugString(FieldInfo field)
        {
            return string.Join("\n\t", field.GetCustomAttributes(false)
                .Select(x => string.Format("  {0}: {1}", x.GetType().Name, string.Join(", ", x.GetType()
                                              .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                                              .Select(attProp => $"{attProp.Name}={attProp.GetValue(x)}")))));
        }

        /// <summary>
        /// Unmarshals any data in a struct which can't be automatically marshalled by Marshal.StructToPtr.
        /// This method uses reflection and as such isn't very fast.
        /// </summary>
        /// <typeparam name="OutStruct">The type of the struct to output; MUST have corresponding 
        /// field names to InStruct</typeparam>
        /// <typeparam name="InStruct">The type of the struct to marshal</typeparam>
        /// <param name="obj">The complex struct to be unmarshalled</param>
        /// <returns>The fully unmarshalled struct.</returns>
        public OutStruct UnMarshalStruct<OutStruct, InStruct>(InStruct obj)
            where OutStruct : unmanaged
            where InStruct : struct
        {
            if (typeof(InStruct) == typeof(OutStruct))
                return Unsafe.As<InStruct, OutStruct>(ref obj);

            if (ReflectionCacheExpression<OutStruct, InStruct>.Supported)
            {
                if (!ReflectionCacheExpression<OutStruct, InStruct>.Compiled)
                    ReflectionCacheExpression<OutStruct, InStruct>.Compile(this);

                var ret1 = ReflectionCacheExpression<OutStruct, InStruct>.UnmarshalStructFunc(obj);
                return ret1;
            }
            else
            {
                var refl = ReflectionCache.GetOrBuildReflectionCache(this, typeof(OutStruct), typeof(InStruct));
                var ret1 = new OutStruct();
                var objRef = __makeref(obj);
                var retRef = __makeref(ret1);
                foreach (var f in refl.fieldMapping)
                {
#if DEBUG
                    try
                    {
#endif
                        var local = f.local.GetValueDirect(objRef);
                        if (f.toLocal != null)
                            f.native.SetValueDirect(retRef, f.toNative(local));
                        else
                            f.native.SetValueDirect(retRef, local);
#if DEBUG
                    }
                    catch (Exception ex)
                    {
                        throw new FieldAccessException($"Failed to unmarshal field '{f.native.Name}' in '{f.native.DeclaringType.Name}'. \n" +
                            $"Attributes:\n{GetCustomAttributeDebugString(f.native)}\n" +
                            $"Failed with internal exception:", ex);
                    }
#endif
                }
                return ret1;
            }
        }

        /// <summary>
        /// Converts a byte array into an unmanaged structure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T ByteArrayToStructure<T>(byte[] bytes) where T : unmanaged
        {
            return MemoryMarshal.Read<T>(bytes);
        }

        /// <summary>
        /// Converts a structure to a byte array without allocating any memory.
        /// </summary>
        /// <param name="obj">Struct to convert</param>
        /// <param name="dst">Byte array to copy to</param>
        /// <param name="startIndex">Index in the destination array to copy to</param>
        /// <returns>The number of bytes written to the array</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int StructureToByteArray<T>(T obj, byte[] dst, int startIndex) where T : unmanaged
        {
            MemoryMarshal.Write(dst.AsSpan()[startIndex..], ref obj);
            return Unsafe.SizeOf<T>();
        }

        /// <summary>
        /// Attempts to allocate memory in the remote process using VirtualAlloc/@GetMem.
        /// </summary>
        /// <remarks>
        /// Returns 0 (nullptr) if no memory is to be allocated. Raises an
        /// <seealso cref="OutOfMemoryException"/> if no memory could be allocated.
        /// </remarks>
        /// <param name="bytes">The number of bytes to allocate</param>
        /// <param name="fastAlloc">Use the faster memory allocator. This bypasses the Delphi memory allocator
        /// and simply calls VirtualAlloc which is much faster. It's generally not recommended as Omsi can't usually
        /// free memory allocated this way. The slow allocator has a latency of 1 frame when not running as an Omsi plugin.</param>
        /// <returns>The pointer to the allocated memory</returns>
        /// <exception cref="OutOfMemoryException"></exception>
        public async Task<int> AllocRemoteMemory(int bytes, bool fastAlloc = false)
        {
            // Return a nullptr if no memory is needed.
            if (bytes == 0)
                return 0;

            int addr;
            if (fastAlloc)
            {
                if (isLocalPlugin)
                    addr = (int)Marshal.AllocHGlobal(bytes);
                else
                    addr = Imports.VirtualAllocEx((int)omsiProcessHandle, 0, bytes,
                        Imports.AllocationType.MEM_COMMIT | Imports.AllocationType.MEM_RESERVE,
                        Imports.MemoryProtectionType.PAGE_READWRITE);
            }
            else
            {
                if (!remoteMethods.IsInitialised)
                    throw new Exception("OmsiRemoteMethods are not initialised, remote memory allocator cannot be used!");

                addr = unchecked((int)await remoteMethods.OmsiGetMem(bytes));
            }

            if (addr == 0)
                throw new OutOfMemoryException("Couldn't allocate any more memory in the remote process!");

            return addr;
        }

        /// <summary>
        /// Attempts to free memory in the remote process using VirtualFree/@FreeMem.
        /// </summary>
        /// <remarks>
        /// Returns 0 (nullptr) if no memory is to be allocated. Raises an
        /// <seealso cref="OutOfMemoryException"/> if no memory could be allocated.
        /// </remarks>
        /// <param name="address">A pointer to the starting address of the region of memory to be freed</param>
        /// <param name="fastAlloc">Use the faster memory allocator. This bypasses the Delphi memory allocator
        /// and simply calls VirtualAlloc which is much faster. It's generally not recommended as Omsi can't usually
        /// free memory allocated this way. The slow allocator has a latency of 1 frame when not running as an Omsi plugin.</param>
        public void FreeRemoteMemory(int address, bool fastAlloc = false)
        {
            // Return a nullptr if no memory is needed.
            if (address == 0)
                return;

            if (fastAlloc)
            {
                if (isLocalPlugin)
                    Marshal.FreeHGlobal((nint)address);
                else
                    Imports.VirtualFreeEx((int)omsiProcessHandle, address, 0, Imports.FreeType.MEM_RELEASE);
            }
            else
            {
                if (!remoteMethods.IsInitialised)
                    throw new Exception("OmsiRemoteMethods are not initialised, remote memory allocator cannot be used!");

                remoteMethods.OmsiFreeMemAsync(address);
            }
        }

        /// <inheritdoc cref="FreeRemoteMemory(int, bool)"/>
        public void FreeRemoteMemory(uint address, bool fastAlloc = false)
        {
            FreeRemoteMemory(unchecked((int)address), fastAlloc);
        }

        public void Dispose()
        {
            omsiProcess.Dispose();
            remoteMethods.Dispose();
            if (omsiProcessHandle != IntPtr.Zero)
                Imports.CloseHandle(omsiProcessHandle);
        }

        #endregion
    }

    internal static class Imports
    {
        #region DllImports

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern IntPtr CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, out int lpNumberOfBytesWritten);
        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, ref byte buffer, int size, out int lpNumberOfBytesWritten);

        /// <summary>
        /// Allocates memory in a remote process's memory space.
        /// </summary>
        /// <param name="hProcess">The pointer to the process to allocate memory in</param>
        /// <param name="lpAddress">The desired starting address to allocate memory at (leave at 0 for default)</param>
        /// <param name="dwSize">How many bytes of memory to allocate</param>
        /// <param name="flAllocationType">The type of allocation</param>
        /// <param name="flProtect">The type of memory protection for the regions to be allocated</param>
        /// <returns>The address of the allocated memory. Returns 0 if the allocation failed.</returns>
        [DllImport("kernel32.dll")]
        public static extern int VirtualAllocEx(int hProcess, int lpAddress, int dwSize, AllocationType flAllocationType, MemoryProtectionType flProtect);
        [DllImport("kernel32.dll")]
        public static extern bool VirtualFreeEx(int hProcess, int lpAddress, int dwSize, FreeType dwFreeType);
        #endregion

        [Flags]
        internal enum AllocationType : int
        {
            MEM_COMMIT = 0x00001000,
            MEM_RESERVE = 0x00002000,
            MEM_RESET = 0x00080000,
            MEM_RESET_UNDO = 0x10000000,
            MEM_LARGE_PAGES = 0x20000000,
            MEM_PHYSICAL = 0x00400000,
            MEM_TOP_DOWN = 0x00100000,
        }

        [Flags]
        internal enum FreeType : int
        {
            MEM_DECOMMIT = 0x00004000,
            MEM_RELEASE = 0x00008000,

            MEM_COALESCE_PLACEHOLDERS = 0x00000001,
            MEM_PRESERVE_PLACEHOLDER = 0x00000002
        }

        [Flags]
        internal enum MemoryProtectionType : int
        {
            PAGE_EXECUTE = 0x10,
            PAGE_EXECUTE_READ = 0x20,
            PAGE_EXECUTE_READWRITE = 0x40,
            PAGE_EXECUTE_WRITECOPY = 0x80,
            PAGE_NOACCESS = 0x01,
            PAGE_READONLY = 0x02,
            PAGE_READWRITE = 0x04,
            PAGE_WRITECOPY = 0x08,
            PAGE_TARGETS_INVALID = 0x40000000,
#pragma warning disable CA1069 // Enums values should not be duplicated
            PAGE_TARGETS_NO_UPDATE = 0x40000000,
#pragma warning restore CA1069 // Enums values should not be duplicated
            PAGE_GUARD = 0x100,
            PAGE_NOCACHE = 0x200,
            PAGE_WRITECOMBINE = 0x400,
        }

        [Flags]
        internal enum OpenProcessFlags
        {
            PROCESS_VM_OPERATION = 0x0008,
            PROCESS_VM_READ = 0x0010,
            PROCESS_VM_WRITE = 0x0020,
        }
    }

    /// <summary>
    /// Represents errors which occur when reading or writing to remote memory.
    /// </summary>
    public class MemoryAccessException : Exception
    {
        public int Address { get; init; }

        public MemoryAccessException() : base() { }
        public MemoryAccessException(string message) : base(message) { }
        public MemoryAccessException(string message, int address) : base(message)
        {
            Address = address;
        }

        public MemoryAccessException(string message, uint address) : this(message)
        {
            Address = (int)address;
        }
    }
}
