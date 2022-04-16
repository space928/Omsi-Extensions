using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    internal class Memory
    {
        private Process m_iProcess;
        private IntPtr m_iProcessHandle;

        private int m_iBytesRead;

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
        /// Sets the value of a string at a given address.<para/>
        /// WARNING: This method will NOT allocate new memory, ensure that there is enough space before 
        /// writing to the string.
        /// </summary>
        /// <param name="Address">The address of the data to set</param>
        /// <param name="value">The new value of the string to set; this must be of the 
        /// correct encoding to avoid memory corruption</param>
        /// <param name="wide">Chooses which encoding to convert the string to</param>
        public void WriteMemory(int address, string value, bool wide = false)
        {
            byte[] buffer;
            if (wide)
                buffer = Encoding.Unicode.GetBytes(value);
            else
                buffer = Encoding.ASCII.GetBytes(value);

            Imports.WriteProcessMemory((int)m_iProcessHandle, address, buffer, buffer.Length, out _);
        }

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
            int i = address;
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
                        case OmsiStrPtrAttribute a:
                            val = ReadMemoryString((int)val, a.Wide);
                            break;

                        case OmsiPtrAttribute:
                            val = new IntPtr((int)val);
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
                            if(a.RequiresExtraMarshalling)
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

        #region Other

        internal struct Flags
        {
            public const int PROCESS_VM_OPERATION = 0x0008;
            public const int PROCESS_VM_READ = 0x0010;
            public const int PROCESS_VM_WRITE = 0x0020;
        }

        #endregion

        #region Conversion

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
        #endregion
    }
}
