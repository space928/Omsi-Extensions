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
        private Task stateMonitorTask;

        /// <summary>
        /// Gets the object storing all of Omsi's global variables.
        /// </summary>
        public OmsiGlobals Globals => globals ??= new(omsiMemory, 0, this);

        /// <summary>
        /// Gets the currently hooked Omsi process.
        /// </summary>
        public Process OmsiProcess => process;

#if DEBUG
        /// <summary>
        /// Gets the remote memory manager. In general you shouldn't need to use this, you should only ever need to interact with OmsiGlobals.
        /// </summary>
        public Memory OmsiMemory => omsiMemory;
#endif

        #region Events
        /// <summary>
        /// An event raised when omsi.exe has exited.
        /// </summary>
        /// <remarks>
        /// Events are not guaranteed to be raised as soon as the action in question occurs; the game's state is only check once every 20ms.
        /// Events are raised from a worker thread, event handlers should try not to block this thread or future events won't be raised.
        /// </remarks>
        public event EventHandler OnOmsiExited;
        /// <summary>
        /// An event raised when Omsi gets a DirectX context.
        /// </summary>
        /// <remarks>
        /// <inheritdoc cref="OnOmsiExited"/>
        /// </remarks>
        public event EventHandler OnOmsiGotD3DContext;
        /// <summary>
        /// An event raised when Omsi gets a DirectX context.
        /// </summary>
        /// <remarks>
        /// <inheritdoc cref="OnOmsiExited"/>
        /// </remarks>
        public event EventHandler OnOmsiLostD3DContext;
        /// <summary>
        /// An event raised when Omsi starts loading a new map.
        /// </summary>
        /// <remarks>
        /// <inheritdoc cref="OnOmsiExited"/>
        /// </remarks>
        public event EventHandler OnMapChange;
        /// <summary>
        /// An event raised when Omsi has loaded or unloaded a new map. The <c>EventArgs</c> is a boolean representing whether the map is loaded.
        /// </summary>
        /// <remarks>
        /// <inheritdoc cref="OnOmsiExited"/>
        /// </remarks>
        public event EventHandler<bool> OnMapLoaded;

        private int lastD3DState = 0;
        private int lastMapState = 0;
        private bool lastMapLoaded = false;
        #endregion

        /// <summary>
        /// Attaches the hooking application to OMSI.exe.
        /// Always call this at some point before trying to read and write data.
        /// <param name="initialiseRemoteMethods">Try to initialise the connection to OmsiHookRPCPlugin, which is needed if you intend to call Omsi code or allocate memory.</param>
        /// </summary>
        public async Task AttachToOMSI(bool initialiseRemoteMethods = true)
        {
            Console.WriteLine("Attaching to OMSI.exe...");

            omsiMemory = new Memory();

            int cursorLine = Console.CursorTop;
            DateTime startTime = DateTime.Now;
            var found = false;
            while (!found)
            {
                (found, process) = omsiMemory.Attach("omsi");
                if (!found) {
                    Console.WriteLine($"Waiting for OMSI.exe (waited for {(DateTime.Now-startTime).TotalSeconds:0} seconds)...");
                    Console.SetCursorPosition(0, cursorLine);
                    await Task.Delay(250);
                }
            }

            if(initialiseRemoteMethods)
            {
                await OmsiRemoteMethods.InitRemoteMethods(omsiMemory);
            }

            stateMonitorTask = new(MonitorStateTask);
            stateMonitorTask.Start();

            Console.WriteLine("Connected succesfully!");
        }

        [Obsolete("This will be obselete once TMyOMSIList is wrapped! The list of vehicles will be moved to OmsiGlobals.")]
        public OmsiRoadVehicleInst GetRoadVehicleInst(int index)
        {
            var vehPtr = GetListItem(0x00861508, index);
            return vehPtr == 0 ? null : new OmsiRoadVehicleInst(omsiMemory, vehPtr);
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
            try
            {
                return omsiMemory.ReadMemory<int>(omsiMemory.ReadMemory<int>(omsiMemory.ReadMemory<int>(omsiMemory.ReadMemory<int>(addr) + 0x28) + 0x4) + index * 4);
            } catch
            {
                return 0;
            }
        }

        /// <summary>
        /// This method continously spins to check the state of the game and raise events when the state changes.
        /// </summary>
        private void MonitorStateTask()
        {
            while(!process.HasExited)
            {
                int currentD3DState = omsiMemory.ReadMemory<int>(0x008627d0);
                if(currentD3DState != lastD3DState)
                {
                    if (currentD3DState != 0)
                        OnOmsiGotD3DContext?.Invoke(this, new());
                    else
                        OnOmsiLostD3DContext?.Invoke(this, new());
                    lastD3DState = currentD3DState;
                }

                int currentMapAddr = omsiMemory.ReadMemory<int>(0x861588);
                if (currentMapAddr != 0)
                {
                    int currentMapName = omsiMemory.ReadMemory<int>(currentMapAddr + 0x154);
                    bool currentMapLoaded = omsiMemory.ReadMemory<bool>(currentMapAddr + 0x120);
                    if(lastMapState != currentMapName)
                    {
                        if(currentMapName != 0)
                            OnMapChange?.Invoke(this, new());
                        lastMapState = currentMapName;
                    }
                    if(lastMapLoaded != currentMapLoaded)
                    {
                        OnMapLoaded?.Invoke(this, currentMapLoaded);
                        lastMapLoaded = currentMapLoaded;
                    }
                }

                Thread.Sleep(20);
            }

            OnOmsiExited?.Invoke(this, new());
        }
    }

    /// <summary>
    /// Indicates that OmsiHook was not connected to the remote application when the method was called.
    /// </summary>
    public class NotInitialisedException : Exception 
    {
        public NotInitialisedException() : base() { }
        public NotInitialisedException(string message) : base(message) { }
    }
}
