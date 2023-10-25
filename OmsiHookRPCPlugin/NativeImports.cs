using System.Runtime.InteropServices;

namespace OmsiHookRPCPlugin
{
    internal static class NativeImports
    {
        [DllImport("OmsiHookInvoker.dll")]
        internal static extern int TProgManMakeVehicle(int progMan, int vehList, int _RoadVehicleTypes, bool onlyvehlist, bool CS,
            float TTtime, bool situationload, bool dialog, bool setdriver, bool thread,
            int kennzeichen_index, bool initcall, int startday, byte trainbuilddir, bool reverse,
            int grouphof, int typ, int tour, int line, int farbschema, bool Scheduled,
            bool AIRoadVehicle, bool kennzeichen_random, bool farbschema_random, int filename);
        [DllImport("OmsiHookInvoker.dll")]
        internal static extern int TTempRVListCreate(int classAddr, int capacity);
        [DllImport("OmsiHookInvoker.dll")]
        internal static extern int TProgManPlaceRandomBus(int progMan, int aityp,
            int group, float TTtime, bool thread, bool instantCopy, int _typ,
            bool scheduled, int startDay, int tour, int line);
        [DllImport("OmsiHookInvoker.dll")]
        internal static extern int GetMem(int length);
        [DllImport("OmsiHookInvoker.dll")]
        internal static extern int FreeMem(int addr);
        [DllImport("OmsiHookInvoker.dll")]
        internal static extern int HookD3D();
        [DllImport("OmsiHookInvoker.dll")]
        internal static extern int CreateTexture(uint Width, uint Height, uint Format, uint ppTexture, uint pSharedHandle);
    }
}
