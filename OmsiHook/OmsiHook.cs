using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// Base OmsiHook class - use this to hook into OMSI's memory and access its data, all the recognised OMSI globals are defined in the Globals property. 
    /// For example usage see <seealso href="https://space928.github.io/Omsi-Extensions/articles/intro.html">our docs</seealso>.
    /// </summary>
    public class OmsiHook
    {
        private Memory omsiMemory;
        private Process process;
        private OmsiGlobals globals;

        /// <summary>
        /// Gets the object storing all of Omsi's global variables.
        /// </summary>
        public OmsiGlobals Globals => globals ??= new(omsiMemory, 0, this);

        /// <summary>
        /// Gets the currently hooked Omsi process.
        /// </summary>
        public Process OmsiProcess => process;

        /// <summary>
        /// Attaches the hooking application to OMSI.exe.
        /// Always call this at some point before trying to read and write data.
        /// </summary>
        public async Task AttachToOMSI()
        {
            Console.WriteLine("Attaching to OMSI.exe...");

            omsiMemory = new Memory();

            var found = false;
            while (!found)
            {
                await Task.Delay(250);
                (found, process) = omsiMemory.Attach("omsi");
                Console.WriteLine("Waiting for OMSI.exe...");
            }

            Console.WriteLine("Connected succesfully!");
        }

        [Obsolete("This will be obselete once TMyOMSIList is wrapped! The list of vehicles will be moved to OmsiGlobals.")]
        public OmsiRoadVehicleInst GetRoadVehicleInst(int index)
        {
            return new OmsiRoadVehicleInst(omsiMemory, GetListItem(0x00861508, index));
        }

        /// <summary>
        /// Gets an item from a TMyOMSIList by index.
        /// </summary>
        /// <param name="addr">Pointer to the list</param>
        /// <param name="index">Index of the item to get</param>
        /// <returns>The item at that location</returns>
        /// <remarks>TMyOMSIList has a bunch of junk in it as well as a TList which is what we index. 
        /// See 0x0074be18 in the dissassembly for an example of how to get an item from a TMyOMSIList</remarks>
        [Obsolete("This API will be replaced once TMyOMSIList is wrapped!")]
        private int GetListItem(int addr, int index)
        {
            return omsiMemory.ReadMemory<int>(omsiMemory.ReadMemory<int>(omsiMemory.ReadMemory<int>(omsiMemory.ReadMemory<int>(addr) + 0x28) + 0x4) + index * 4);
        }

        /// <summary>
        /// Finds and returns the value of a named float variable.
        /// </summary>
        /// <param name="varName">Name of variable to look for</param>
        /// <param name="baseAddr">Value of *(baseAddr + moduleBase)</param>
        /// <returns></returns>
        [Obsolete("This API is going to be moved into the OmsiRoadVehicleInst class in future!")] 
        public float FindVar(string varName, int baseAddr)
        {
            //Offsets used in these lookups are derived from the dissassembly of omsi at 0x642205
            int varNameAddr = omsiMemory.ReadMemory<int>(omsiMemory.ReadMemory<int>(baseAddr + 0x710) + 0x1ec);
            int varValueAddr = omsiMemory.ReadMemory<int>(omsiMemory.ReadMemory<int>(omsiMemory.ReadMemory<int>(baseAddr + 0x214) + 0x28));

            int vars = omsiMemory.ReadMemory<int>(varNameAddr - 4);
            for (int i = 0; i < vars; i++)
                if (ReadString(omsiMemory.ReadMemory<int>(varNameAddr + i * 4)) == varName)
                    return omsiMemory.ReadMemory<float>(omsiMemory.ReadMemory<int>(varValueAddr + i * 4));

            return 0;
        }

        /// <summary>
        /// Finds and returns the value of a named string variable.
        /// </summary>
        /// <param name="varName">Name of variable to look for</param>
        /// <param name="baseAddr">Value of *(baseAddr + moduleBase)</param>
        /// <returns></returns>
        [Obsolete("This API is going to be moved into the OmsiRoadVehicleInst class in future!")]
        public string FindStringVar(string varName, int baseAddr)
        {
            //Offsets used in these lookups are derived from the dissassembly of omsi at 0x642205
            int varNameAddr = omsiMemory.ReadMemory<int>(omsiMemory.ReadMemory<int>(baseAddr + 0x710) + 0x1f0);
            int varValueAddr = omsiMemory.ReadMemory<int>(omsiMemory.ReadMemory<int>(baseAddr + 0x214) + 0x2c);

            int vars = omsiMemory.ReadMemory<int>(varNameAddr - 4);
            for (int i = 0; i < vars; i++)
                if (ReadString(omsiMemory.ReadMemory<int>(varNameAddr + i * 4)) == varName)
                    return ReadString(omsiMemory.ReadMemory<int>(varValueAddr + i * 4), true);

            return null;
        }

        /// <summary>
        /// Returns the value of a null terminated string at a given address.
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        [Obsolete("Use `Memory.ReadMemoryString()` instead")] 
        private string ReadString(int addr, bool wide = false)
        {
            var sb = new StringBuilder();
            int i = addr;
            byte[] bytesBuff = new byte[2];
            while (true)
            {
                var bytes = omsiMemory.ReadMemory(i, wide ? 2 : 1, bytesBuff);
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
    }
}
