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
        /// <param name="initialiseRemoteMethods">Try to initialise the connection to OmsiHookRPCPlugin, which is needed if you intend to call Omsi code or allocate memory.</param>
        /// </summary>
        public async Task AttachToOMSI(bool initialiseRemoteMethods = true)
        {
            Console.WriteLine("Attaching to OMSI.exe...");

            omsiMemory = new Memory();

            var found = false;
            while (!found)
            {
                (found, process) = omsiMemory.Attach("omsi");
                if (!found) {
                    Console.WriteLine("Waiting for OMSI.exe...");
                    await Task.Delay(250);
                }
            }

            if(initialiseRemoteMethods)
            {
                OmsiRemoteMethods.InitRemoteMethods(omsiMemory);
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
    }
}
