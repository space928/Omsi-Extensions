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
    public class OmsiHook : IDisposable
    {
        private Memory omsiMemory;
        private OmsiGlobals globals;
        private Task stateMonitorTask;
        private bool isD3DReady;
        private bool isLocalPlugin;

        /// <summary>
        /// Gets the object storing all of Omsi's global variables.
        /// </summary>
        public OmsiGlobals Globals => globals ??= new(omsiMemory, 0, this);

        /// <summary>
        /// Gets the currently hooked Omsi process.
        /// </summary>
        public Process OmsiProcess => omsiMemory?.OmsiProcess;

        /// <summary>
        /// Gets the instance of OmsiRemoteMethods.
        /// </summary>
        public OmsiRemoteMethods RemoteMethods => omsiMemory?.RemoteMethods;

#if DEBUG
        /// <summary>
        /// Gets the remote memory manager. In general you shouldn't need to use this, you should only ever need to interact with OmsiGlobals.
        /// </summary>
        [Obsolete("This property is only available in Debug builds, in general you should never need raw access to remote memory.")]
        public Memory OmsiMemory => omsiMemory;
#endif

        public bool IsD3DReady => isD3DReady;

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
        /// 
        /// Note that this is one of the last events raised when exiting the game; it's raised after PluginFinalize is called.
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
        public event EventHandler<OmsiMap> OnMapChange;
        /// <summary>
        /// An event raised when Omsi has loaded or unloaded a new map. The <c>EventArgs</c> is a boolean representing whether the map is loaded.
        /// 
        /// Note that while this event is raised when the map has finished loading; other systems may still be 
        /// loading (timetables, weather, humans, ai vehicles, and the map camera are loaded later). If you want to be sure 
        /// everything has loaded, the best bet would be to wait for an <c>AccessVariable()</c> call.
        /// </summary>
        /// <remarks>
        /// <inheritdoc cref="OnOmsiExited"/>
        /// </remarks>
        public event EventHandler<bool> OnMapLoaded;
        /// <summary>
        /// An event raised when the active vehicle is changed. The <c>EventArgs</c> is a <c>OmsiRoadVehicleInst</c> of the new bus.
        /// </summary>
        /// <remarks>
        /// <inheritdoc cref="OnOmsiExited"/>
        /// </remarks>
        public event EventHandler<OmsiRoadVehicleInst> OnActiveVehicleChanged;

        private int lastD3DState = 0;
        private int lastMapState = 0;
        private bool lastMapLoaded = false;
        private int lastVehiclePtr = 0;
        #endregion

        /// <summary>
        /// Attaches the hooking application to OMSI.exe.
        /// Always call this at some point before trying to read and write data.
        /// </summary>
        /// <param name="initialiseRemoteMethods">Try to initialise the connection to OmsiHookRPCPlugin, which is needed if you intend to call Omsi code or allocate memory.</param>
        public async Task AttachToOMSI(bool initialiseRemoteMethods = true)
        {
            omsiMemory = new Memory();

            int cursorLine = int.MinValue;
            try
            {
                cursorLine = Console.CursorTop;
            } catch { }
            DateTime startTime = DateTime.Now;
            var found = false;
            while (!found)
            {
                found = omsiMemory.Attach("omsi");
                if (!found) {
                    if (cursorLine != int.MinValue)
                    {
                        Console.WriteLine($"Waiting for OMSI.exe (waited for {(DateTime.Now - startTime).TotalSeconds:0} seconds)...");
                        Console.SetCursorPosition(0, cursorLine);
                    }
                    await Task.Delay(250);
                }
            }

            isLocalPlugin = Process.GetCurrentProcess().ProcessName == OmsiProcess.ProcessName;

            if (initialiseRemoteMethods)
            {
                var remoteMethods = new OmsiRemoteMethods();
                await remoteMethods.InitRemoteMethods(omsiMemory, isLocalPlugin: isLocalPlugin);
                omsiMemory.RemoteMethods = remoteMethods;
                isD3DReady = remoteMethods.OmsiHookD3D();
            }

            stateMonitorTask = new(MonitorStateTask);
            stateMonitorTask.Start();

            OnOmsiGotD3DContext += OmsiHook_OnOmsiGotD3DContext;
            OnOmsiLostD3DContext += OmsiHook_OnOmsiLostD3DContext;
            OnMapChange += OmsiHook_OnMapChange;
        }

        /// <summary>
        /// Factory method for <see cref="D3DTexture"/> objects. The resulting D3DTexture will need to be 
        /// initiallised before use by calling either <see cref="D3DTexture.CreateFromExisting(uint)"/> or 
        /// <see cref="D3DTexture.CreateD3DTexture(uint, uint, OmsiRemoteMethods.D3DFORMAT)"/>.
        /// </summary>
        /// <returns>a new <see cref="D3DTexture"/> object.</returns>
        public D3DTexture CreateTextureObject()
        {
            return new D3DTexture(omsiMemory, 0);
        }

        public void Dispose()
        {
            omsiMemory.Dispose();
        }

        private void OmsiHook_OnMapChange(object sender, OmsiMap e)
        {
            /*Task.Run(() => {
                while(!isD3DReady)
                    isD3DReady = OmsiRemoteMethods.OmsiHookD3D();
            });*/
        }

        private void OmsiHook_OnOmsiLostD3DContext(object sender, EventArgs e)
        {
            isD3DReady = false;
        }

        private void OmsiHook_OnOmsiGotD3DContext(object sender, EventArgs e)
        {
            Task.Run(() => {
                while (RemoteMethods.IsInitialised && !isD3DReady)
                    isD3DReady = RemoteMethods.OmsiHookD3D();
            });
        }

        [Obsolete("This will be obselete once TMyOMSIList is wrapped! The list of vehicles will be moved to OmsiGlobals.")]
        public OmsiRoadVehicleInst GetRoadVehicleInst(int index)
        {
            // TODO: A More permanant fix should be done at some point.
            var vehPtr = GetListItem(0x00861508, index);
            return vehPtr < 1000 ? null : new OmsiRoadVehicleInst(omsiMemory, vehPtr);
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
            while(!OmsiProcess.HasExited)
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
                    if(RemoteMethods.IsInitialised && !isD3DReady) 
                        isD3DReady = RemoteMethods.OmsiHookD3D();

                    int currentMapName = omsiMemory.ReadMemory<int>(currentMapAddr + 0x154);
                    bool currentMapLoaded = omsiMemory.ReadMemory<bool>(currentMapAddr + 0x120);
                    if(lastMapState != currentMapName)
                    {
                        if(currentMapName != 0)
                            OnMapChange?.Invoke(this, Globals.Map);
                        lastMapState = currentMapName;
                    }
                    if(lastMapLoaded != currentMapLoaded)
                    {
                        OnMapLoaded?.Invoke(this, currentMapLoaded);
                        lastMapLoaded = currentMapLoaded;
                    }
                }
                var vehPtr = GetListItem(0x00861508, omsiMemory.ReadMemory<int>(0x00861740));
                if (vehPtr != lastVehiclePtr)
                {
                    lastVehiclePtr = vehPtr;
                    OnActiveVehicleChanged?.Invoke(this, Globals.PlayerVehicle);
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
