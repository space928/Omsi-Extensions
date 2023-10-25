using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace OmsiHookRPCPlugin
{
    public static class OmsiHookRPCMethods
    {
        public const string PIPE_NAME = @"OmsiHookRPCPipe";

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum RemoteMethod : int
        {
            TProgManMakeVehicle,
            TTempRVListCreate,
            TProgManPlaceRandomBus,
            GetMem,
            FreeMem,
            HookD3D,
            CreateTexture
        }
    }
}
