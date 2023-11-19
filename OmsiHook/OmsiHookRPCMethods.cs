using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace OmsiHookRPCPlugin
{
    internal static class OmsiHookRPCMethods
    {
        public const string PIPE_NAME_RX = @"OmsiHookRPCPipe_RX";
        public const string PIPE_NAME_TX = @"OmsiHookRPCPipe_TX";

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal enum RemoteMethod : int
        {
            CloseRPCConnection,
            TProgManMakeVehicle,
            TTempRVListCreate,
            TProgManPlaceRandomBus,
            GetMem,
            FreeMem,
            HookD3D,
            CreateTexture,
            UpdateSubresource,
            ReleaseTexture,
            GetTextureDesc,
            IsTexture
        }

        internal static readonly ReadOnlyDictionary<RemoteMethod, int> RemoteMethodsArgsSizes = new(new Dictionary<RemoteMethod, int>()
        {
            { RemoteMethod.CloseRPCConnection,          4 },
            { RemoteMethod.TProgManMakeVehicle,         61 },
            { RemoteMethod.TTempRVListCreate,           8 },
            { RemoteMethod.TProgManPlaceRandomBus,      35 },
            { RemoteMethod.GetMem,                      4 },
            { RemoteMethod.FreeMem,                     4 },
            { RemoteMethod.HookD3D,                     0 },
            { RemoteMethod.CreateTexture,               16 },
            { RemoteMethod.UpdateSubresource,           36 },
            { RemoteMethod.ReleaseTexture,              4 },
            { RemoteMethod.GetTextureDesc,              32 },
            { RemoteMethod.IsTexture,                   4 },
        });
    }
}
