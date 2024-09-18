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
            None,
            CloseRPCConnection,
            TProgManMakeVehicle,
            TTempRVListCreate,
            CopyTempListIntoMainList,
            TProgManPlaceRandomBus,
            GetMem,
            FreeMem,
            HookD3D,
            CreateTexture,
            UpdateSubresource,
            ReleaseTexture,
            GetTextureDesc,
            GetTextureLevelCount,
            IsTexture,
            RVTriggerXML,
            SoundTrigger,
            SetCriticalSectionLock,
            ReleaseCriticalSectionLock
        }

        internal static readonly ReadOnlyDictionary<RemoteMethod, int> RemoteMethodsArgsSizes = new(new Dictionary<RemoteMethod, int>()
        {
            { RemoteMethod.None,                        0 },
            { RemoteMethod.CloseRPCConnection,          4 },
            { RemoteMethod.TProgManMakeVehicle,         61 },
            { RemoteMethod.TTempRVListCreate,           8 },
            { RemoteMethod.CopyTempListIntoMainList,    8 },
            { RemoteMethod.TProgManPlaceRandomBus,      35 },
            { RemoteMethod.GetMem,                      4 },
            { RemoteMethod.FreeMem,                     4 },
            { RemoteMethod.HookD3D,                     0 },
            { RemoteMethod.CreateTexture,               20 },
            { RemoteMethod.UpdateSubresource,           40 },
            { RemoteMethod.ReleaseTexture,              4 },
            { RemoteMethod.GetTextureDesc,              36 },
            { RemoteMethod.GetTextureLevelCount,        4 },
            { RemoteMethod.IsTexture,                   4 },
            { RemoteMethod.RVTriggerXML,                12 },
            { RemoteMethod.SoundTrigger,                12 },
            { RemoteMethod.SetCriticalSectionLock,      4 },
            { RemoteMethod.ReleaseCriticalSectionLock,  4 },
        });
    }
}
