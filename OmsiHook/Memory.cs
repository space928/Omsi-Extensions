using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// Memory managment class for accessing and marshaling OMSI's memory
    /// </summary>
    internal class Memory
    {
        private Process m_iProcess;
        private IntPtr m_iProcessHandle;

        private int m_iBytesRead;

        internal struct Flags
        {
            public const int PROCESS_VM_OPERATION = 0x0008;
            public const int PROCESS_VM_READ = 0x0010;
            public const int PROCESS_VM_WRITE = 0x0020;
        }

        /// <summary>
        /// Attempts to attach to a given process as a debugger.
        /// </summary>
        /// <param name="procName">The name of the process to attach to eg: "OMSI.exe"</param>
        /// <returns>A tuple containing whether or not the process attached successfully and the Process instance</returns>
        public (bool, Process proc) Attach(string procName)
        {
            if (Process.GetProcessesByName(procName).Length > 0)
            {
                m_iProcess = Process.GetProcessesByName(procName)[0];
                try
                {
                    Process.EnterDebugMode();
                }
                catch (Exception e)
                {
                    Console.WriteLine("The plugin couldn't grant itself debug privileges, it may fail to attach to the game.");
                    Console.WriteLine(e);
                }
                m_iProcessHandle =
                    Imports.OpenProcess(Flags.PROCESS_VM_OPERATION | Flags.PROCESS_VM_READ | Flags.PROCESS_VM_WRITE,
                        false, m_iProcess.Id);
                return (true, m_iProcess);
            }

            return (false, null);
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
        public void WriteMemory<T>(int address, T value) where T : struct
        {
            var buffer = StructureToByteArray(value);

            Imports.WriteProcessMemory((int)m_iProcessHandle, address, buffer, buffer.Length, out _);
        }

        /// <summary>
        /// Writes the value of a struct to an array of structs at a given address. <para/>
        /// Performs bounds checking.
        /// </summary>
        /// <typeparam name="T">The type of the struct to write</typeparam>
        /// <param name="address">The address of the array to write to</param>
        /// <param name="value">The value of the new element</param>
        /// <param name="index">The index of the element to write to</param>
        public void WriteMemoryArrayItemSafe<T>(int address, T value, int index) where T : struct
        {
            int arr = ReadMemory<int>(address);
            int len = ReadMemory<int>(address - 4);
            if (index < 0 || index >= len)
                throw new ArgumentOutOfRangeException($"Tried to write to item {index} of an {len} element array!");
            WriteMemory(arr + index * Marshal.SizeOf<T>(), value);
        }

        /// <summary>
        /// Writes the value of a struct to an array of structs at a given address. <para/>
        /// Does NOT perform bounds checking!
        /// </summary>
        /// <typeparam name="T">The type of the struct to write</typeparam>
        /// <param name="address">The address of the array to write to</param>
        /// <param name="value">The value of the new element</param>
        /// <param name="index">The index of the element to write to</param>
        public void WriteMemoryArrayItem<T>(int address, T value, int index) where T : struct
        {
            int arr = ReadMemory<int>(address);
            WriteMemory(arr + index * Marshal.SizeOf<T>(), value);
        }

        /// <summary>
        /// Allocates shared memory for an arrays of structs. <para/>
        /// Does not zero the allocated memory!
        /// </summary>
        /// <typeparam name="T">The type of struct to make an array of</typeparam>
        /// <param name="capacity">The number of items to allocate space for</param>
        /// <param name="references">Allows the number of references to the array to be specified. 
        /// Used when overwriting existing arrays to prevent the GC from clearing a array referenced 
        /// by multiple objects when one is destroyed.</param>
        /// <returns>The pointer to the first item of the newly allocated array</returns>
        public int AllocateArray<T>(int capacity, int references = 1) where T : struct
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

            int itemSize = Marshal.SizeOf<T>();
            var ptr = Marshal.AllocCoTaskMem(capacity * itemSize + 8).ToInt32();
            var arrStart = new IntPtr(ptr + 12);
            // Write the array metadata
            WriteMemory(ptr, references);
            WriteMemory(ptr + 0x4, capacity);

            return arrStart.ToInt32() + 8;
        }

        /// <summary>
        /// Allocates shared memory for a string and copies it accross.
        /// </summary>
        /// <param name="value">The string to copy</param>
        /// <param name="wide">Chooses which encoding to convert the string to</param>
        /// <param name="references">Allows the number of references to the string to be specified. 
        /// Used when overwriting existing strings to prevent the GC from clearing a string referenced 
        /// by multiple objects when one is destroyed.</param>
        /// <returns>The pointer to the newly allocated string</returns>
        public int AllocateString(string value, bool wide = false, int references = 1)
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

            byte[] buffer;
            if (wide)
                buffer = Encoding.Unicode.GetBytes(value);
            else
                buffer = Encoding.ASCII.GetBytes(value);

            var ptr = Marshal.AllocCoTaskMem(buffer.Length + 13).ToInt32();
            var strStart = new IntPtr(ptr+12);
            Marshal.Copy(buffer, 0, strStart, buffer.Length);
            // Write the string metadata
            WriteMemory(ptr, (short)1252);
            WriteMemory(ptr + 0x2, (short)(wide?2:1));
            WriteMemory(ptr + 0x4, references);
            WriteMemory(ptr + 0x8, value.Length);
            // Write null terminator
            WriteMemory(strStart.ToInt32() + buffer.Length, (byte)0);

            return strStart.ToInt32();
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
        public void WriteMemory(int address, string value, bool wide = false, bool copyReferences = true)
        {
            int refs = 1;
            if(copyReferences)
                refs = ReadMemory<int>(ReadMemory<int>(address) - 0x8);

            WriteMemory(address, AllocateString(value, wide, refs));
            //Imports.WriteProcessMemory((int)m_iProcessHandle, address, buffer, buffer.Length, out _);
        }

        /// <summary>
        /// Writes the value of a string to an array of strings at a given address.<para/>
        /// WARNING: This method will NOT allocate new memory, ensure that there is enough space before 
        /// writing to the string.
        /// </summary>
        /// <param name="address">The address of the data to set</param>
        /// <param name="value">The new value of the string to set; this must be of the 
        /// correct encoding to avoid memory corruption</param>
        /// <param name="index">The index of the element to write to</param>
        /// <param name="wide">Chooses which encoding to convert the string to</param>
        public void WriteMemoryArrayItem(int address, string value, int index, bool wide = false)
        {
            int arr = ReadMemory<int>(address);
            WriteMemory(arr + index * 4, value, wide);
        }
        #endregion

        #region Memory Reading Methods
        /// <summary>
        /// Reads a struct/value from unmanaged memory and returns it.
        /// </summary>
        /// <typeparam name="T">The type of the value/struct to read</typeparam>
        /// <param name="address">The address to read from</param>
        /// <returns>The value of the struct/value at the given address.</returns>
        public T ReadMemory<T>(IntPtr address) where T : struct
        {
            return ReadMemory<T>(address.ToInt32());
        }

        /// <summary>
        /// Reads a struct/value from unmanaged memory and returns it.
        /// </summary>
        /// <typeparam name="T">The type of the value/struct to read</typeparam>
        /// <param name="address">The address to read from</param>
        /// <returns>The value of the struct/value at the given address.</returns>
        public T ReadMemory<T>(int address) where T : struct
        {
            var ByteSize = Marshal.SizeOf(typeof(T));

            var buffer = new byte[ByteSize];

            Imports.ReadProcessMemory((int)m_iProcessHandle, address, buffer, buffer.Length, ref m_iBytesRead);

            return ByteArrayToStructure<T>(buffer);
        }

        /// <summary>
        /// Returns the value of a null terminated string at a given address.
        /// </summary>
        /// <param name="address">The address to read from</param>
        /// <returns>The value of the string at the given address.</returns>
        public string ReadMemoryString(int address, bool wide = false)
        {
            var sb = new StringBuilder();
            int i = ReadMemory<int>(address);
            while (true)
            {
                var bytes = ReadMemory(i, wide ? 2 : 1);
                if (bytes.All(x => x == 0))
                    break;

                if (wide)
                    sb.Append(Encoding.Unicode.GetString(bytes));
                else
                    sb.Append(Encoding.ASCII.GetString(bytes));
                i++;
                if (wide)
                    i++;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Reads raw bytes from unmanaged memory at a given address.
        /// </summary>
        /// <param name="offset">The address to start reading from</param>
        /// <param name="size">The number of bytes to read</param>
        /// <returns></returns>
        public byte[] ReadMemory(int offset, int size)
        {
            var buffer = new byte[size];

            Imports.ReadProcessMemory((int)m_iProcessHandle, offset, buffer, size, ref m_iBytesRead);

            return buffer;
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
        public T ReadMemoryArrayItemSafe<T>(int address, int index) where T : struct
        {
            int arr = ReadMemory<int>(address);
            int len = ReadMemory<int>(arr - 4);
            if (index < 0 || index >= len)
                throw new IndexOutOfRangeException($"Tried to access element {index} in an array of {len} elements!");
            return ReadMemoryArrayItem<T>(address, index);
        }

        /// <summary>
        /// Reads the value of a struct from an array of structs at a given address. <para/>
        /// Does not perform any bounds checking!
        /// </summary>
        /// <typeparam name="T">The type of the struct to read</typeparam>
        /// <param name="address">The address of the array to read from</param>
        /// <param name="index">The index of the element to read from</param>
        public T ReadMemoryArrayItem<T>(int address, int index) where T : struct
        {
            int arr = ReadMemory<int>(address);
            return ReadMemory<T>(arr + index * Marshal.SizeOf<T>());
        }

        /// <summary>
        /// Reads the value of a <seealso cref="string"/> from an array of strings at a given address. <para/>
        /// Performs bounds checking for people who don't like corrupting data.
        /// </summary>
        /// <typeparam name="T">The type of the string to read</typeparam>
        /// <param name="address">The address of the array to read from</param>
        /// <param name="index">The index of the element to read from</param>
        public string ReadMemoryArrayItemStringSafe(int address, int index, bool wide = false)
        {
            int arr = ReadMemory<int>(address);
            int len = ReadMemory<int>(arr - 4);
            if (index < 0 || index >= len)
                throw new IndexOutOfRangeException($"Tried to access element {index} in an array of {len} elements!");
            return ReadMemoryArrayItemString(address, index);
        }

        /// <summary>
        /// Reads the value of a <seealso cref="string"/> from an array of strings at a given address. <para/>
        /// Does not perform any bounds checking!
        /// </summary>
        /// <typeparam name="T">The type of the string to read</typeparam>
        /// <param name="address">The address of the array to read from</param>
        /// <param name="index">The index of the element to read from</param>
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
        public T[] ReadMemoryObjArray<T>(int address) where T : OmsiObject, new()
        {
            int arr = ReadMemory<int>(address);
            int len = ReadMemory<int>(arr - 4);
            T[] ret = new T[len];
            for (int i = 0; i < len; i++)
            {
                var n = new T();
                n.InitObject(this, ReadMemory<int>(arr + i * 4));
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
        /// <returns>The parsed array of structs.</returns>
        public T[] ReadMemoryStructArray<T>(int address) where T : struct
        {
            int arr = ReadMemory<int>(address);
            int len = ReadMemory<int>(arr - 4);
            T[] ret = new T[len];
            for (int i = 0; i < len; i++)
                ret[i] = ReadMemory<T>(arr + i * Marshal.SizeOf<T>());

            return ret;
        }

        /// <summary>
        /// Reads an array of strings from unmanaged memory at a given address.
        /// </summary>
        /// <param name="address">The address of the array to read from</param>
        /// <returns>The parsed array of strings.</returns>
        public string[] ReadMemoryStringArray(int address, bool wide = false)
        {
            int arr = ReadMemory<int>(address);
            int len = ReadMemory<int>(arr - 4);
            string[] ret = new string[len];
            for (int i = 0; i < len; i++)
                ret[i] = ReadMemoryString(arr + i * 4, wide);

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
           where InStruct : struct
        {
            OutStruct[] ret = new OutStruct[obj.Length];
            for (int i = 0; i < obj.Length; i++)
                ret[i] = MarshalStruct<OutStruct, InStruct>(obj[i]);

            return ret;
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
            where InStruct : struct
        {
            object ret = new OutStruct();
            foreach (var field in obj.GetType().GetFields())
            {
                object val = field.GetValue(obj);
                foreach (var attr in field.GetCustomAttributes(false))
                {
                    // Based on which kind of attribute the field has, perform special marshalling operations
                    switch (attr)
                    {
                        case OmsiStructAttribute a:
                            if (a.RequiresExtraMarshalling)
                                val = typeof(Memory).GetMethod(nameof(MarshalStruct))
                                .MakeGenericMethod(a.ObjType, a.InternalType)
                                .Invoke(this, new object[] { val });
                            break;

                        case OmsiStrPtrAttribute a:
                            val = ReadMemoryString((int)val, a.Wide);
                            break;

                        case OmsiPtrAttribute:
                            val = new IntPtr((int)val);
                            break;

                        case OmsiStructPtrAttribute a:
                            val = typeof(Memory).GetMethod(nameof(ReadMemory))
                                .MakeGenericMethod(a.InternalType)
                                .Invoke(this, new object[] { val });
                            // Perform extra marshalling if needed
                            if (a.RequiresExtraMarshalling)
                                val = typeof(Memory).GetMethod(nameof(MarshalStruct))
                                .MakeGenericMethod(a.ObjType, a.InternalType)
                                .Invoke(this, new object[] { val });
                            break;

                        case OmsiObjPtrAttribute a:
                            int addr = (int)val;
                            val = Activator.CreateInstance(a.ObjType, true);
                            ((OmsiObject)val).InitObject(this, addr);
                            break;

                        case OmsiStructArrayPtrAttribute a:
                            val = typeof(Memory).GetMethod(nameof(ReadMemoryStructArray))
                                .MakeGenericMethod(a.InternalType)
                                .Invoke(this, new object[] { val });
                            // Perform extra marshalling if needed
                            if (a.RequiresExtraMarshalling)
                                val = typeof(Memory).GetMethod(nameof(MarshalStructs))
                                .MakeGenericMethod(a.ObjType, a.InternalType)
                                .Invoke(this, new object[] { val });
                            break;

                        case OmsiObjArrayPtrAttribute a:
                            val = typeof(Memory).GetMethod(nameof(ReadMemoryObjArray))
                                .MakeGenericMethod(a.ObjType)
                                .Invoke(this, new object[] { val });
                            break;

                        case OmsiStrArrayPtrAttribute a:
                            val = ReadMemoryStringArray((int)val, a.Wide);
                            break;
                    }
                }

                // Match fields by name, setting the destination fields to the corresponding source fields
                typeof(OutStruct).GetField(field.Name).SetValue(ret, val);
            }

            return (OutStruct)ret;
        }

        private static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
        }

        private static byte[] StructureToByteArray(object obj)
        {
            var length = Marshal.SizeOf(obj);

            var array = new byte[length];

            var pointer = Marshal.AllocHGlobal(length);

            Marshal.StructureToPtr(obj, pointer, true);
            Marshal.Copy(pointer, array, 0, length);
            Marshal.FreeHGlobal(pointer);

            return array;
        }

        #endregion
    }

    internal class Imports
    {
        #region DllImports

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, out int lpNumberOfBytesWritten);

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
        #endregion

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
    }
}
