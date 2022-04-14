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
        /// <param name="ProcName">The name of the process to attach to eg: "OMSI.exe"</param>
        /// <returns>A tuple containing whether or not the process attached successfully and the Process instance</returns>
        public (bool, Process proc) Attach(string ProcName)
        {
            if (Process.GetProcessesByName(ProcName).Length > 0)
            {
                m_iProcess = Process.GetProcessesByName(ProcName)[0];
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

        public void WriteMemory<T>(int Address, T Value)
        {
            var buffer = StructureToByteArray(Value);

            Imports.WriteProcessMemory((int)m_iProcessHandle, Address, buffer, buffer.Length, out _);
        }

        public void WriteMemory<T>(int Adress, char[] Value)
        {
            var buffer = Encoding.UTF8.GetBytes(Value);

            Imports.WriteProcessMemory((int)m_iProcessHandle, Adress, buffer, buffer.Length, out _);
        }

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
        /// <param name="address"></param>
        /// <returns></returns>
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

        public byte[] ReadMemory(int offset, int size)
        {
            var buffer = new byte[size];

            Imports.ReadProcessMemory((int)m_iProcessHandle, offset, buffer, size, ref m_iBytesRead);

            return buffer;
        }

        public float[] ReadMatrix<T>(int address, int MatrixSize) where T : struct
        {
            var ByteSize = Marshal.SizeOf(typeof(T));
            var buffer = new byte[ByteSize * MatrixSize];
            Imports.ReadProcessMemory((int)m_iProcessHandle, address, buffer, buffer.Length, ref m_iBytesRead);

            return ConvertToFloatArray(buffer);
        }

        public int GetModuleAddress(string Name)
        {
            try
            {
                foreach (ProcessModule ProcMod in m_iProcess.Modules)
                    if (Name == ProcMod.ModuleName)
                        return (int)ProcMod.BaseAddress;
            }
            catch
            {
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR: Cannot find - " + Name + " | Check file extension.");
            Console.ResetColor();

            return -1;
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

        public static float[] ConvertToFloatArray(byte[] bytes)
        {
            if (bytes.Length % 4 != 0)
                throw new ArgumentException();

            var floats = new float[bytes.Length / 4];

            for (var i = 0; i < floats.Length; i++)
                floats[i] = BitConverter.ToSingle(bytes, i * 4);

            return floats;
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
        #endregion
    }
}
