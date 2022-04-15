using System;

namespace OmsiHook
{
    public class OmsiWeather : OmsiObject
    {
        internal OmsiWeather(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        internal OmsiWeather() : base() { }

        //Todo: WeatherSchemes - OmsiWeatherProp[] @ 0x4

        public OmsiWeatherProp ActWeather
        {
            get => Memory.MarshalStruct<OmsiWeatherProp, OmsiWeatherPropInternal>(Memory.ReadMemory<OmsiWeatherPropInternal>(Address + 0x8));
            set => Memory.WriteMemory(Address + 0x8, value);
        }
        public float PriLgtFactor
        {
            get => Memory.ReadMemory<float>(Address + 0x58);
            set => Memory.WriteMemory(Address + 0x58, value);
        }
        public float SecLgtFactor
        {
            get => Memory.ReadMemory<float>(Address + 0x5c);
            set => Memory.WriteMemory(Address + 0x5c, value);
        }
        public float AmbLgtFactor
        {
            get => Memory.ReadMemory<float>(Address + 0x60);
            set => Memory.WriteMemory(Address + 0x60, value);
        }
        public D3DColorValue LightPri
        {
            get => Memory.ReadMemory<D3DColorValue>(Address + 0x64);
            set => Memory.WriteMemory(Address + 0x64, value);
        }
        public D3DColorValue LightSec
        {
            get => Memory.ReadMemory<D3DColorValue>(Address + 0x74);
            set => Memory.WriteMemory(Address + 0x74, value);
        }
        public D3DColorValue LightAmb
        {
            get => Memory.ReadMemory<D3DColorValue>(Address + 0x84);
            set => Memory.WriteMemory(Address + 0x84, value);
        }
        public D3DColorValue LightAll
        {
            get => Memory.ReadMemory<D3DColorValue>(Address + 0x94);
            set => Memory.WriteMemory(Address + 0x94, value);
        }
        public float ActFogDensity
        {
            get => Memory.ReadMemory<float>(Address + 0xa4);
            set => Memory.WriteMemory(Address + 0xa4, value);
        }
        public float ActLightness
        {
            get => Memory.ReadMemory<float>(Address + 0xa8);
            set => Memory.WriteMemory(Address + 0xa8, value);
        }
        /// <summary>
        /// Brightness
        /// </summary>
        public float Helligkeit
        {
            get => Memory.ReadMemory<float>(Address + 0xac);
            set => Memory.WriteMemory(Address + 0xac, value);
        }
        /// <summary>
        /// Twilight switch brightness?
        /// </summary>
        public float Daemmerungsschalter_Helligkeit
        {
            get => Memory.ReadMemory<float>(Address + 0xb0);
            set => Memory.WriteMemory(Address + 0xb0, value);
        }
        public int CloudTypeNum
        {
            get => Memory.ReadMemory<int>(Address + 0xb4);
            set => Memory.WriteMemory(Address + 0xb4, value);
        }
        /// <summary>
        /// TD3DMeshFileObject Pointer
        /// </summary>
        public IntPtr Clouds
        {
            get => Memory.ReadMemory<IntPtr>(Address + 0xb8);
            set => Memory.WriteMemory(Address + 0xb8, value);
        }

        /* TODO:
        public CloudType[] CloudTypes
        {
            get => omsiMemory.ReadMemory<CloudType[]>(baseAddress + 0xbc);
            set => omsiMemory.WriteMemory(baseAddress + 0xbc, value);
        }
        */

        public D3DVector CloudPos
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0xc0);
            set => Memory.WriteMemory(Address + 0xc0, value);
        }
        public float CloudTransparenz
        {
            get => Memory.ReadMemory<float>(Address + 0xcc);
            set => Memory.WriteMemory(Address + 0xcc, value);
        }
        public byte CloudCat
        {
            get => Memory.ReadMemory<byte>(Address + 0xd0);
            set => Memory.WriteMemory(Address + 0xd0, value);
        }

        /*TODO:
         * public OmsiPartikelemitter PercipSystem
        {
            get => omsiMemory.ReadMemory<OmsiPartikelemitter>(baseAddress + 0xd4);
            //set => omsiMemory.WriteMemory(baseAddress + 0xd0, value);
        }*/
        public bool PercipSet
        {
            get => Memory.ReadMemory<bool>(Address + 0xd8);
            set => Memory.WriteMemory(Address + 0xd8, value);
        }
        /// <summary>
        /// TD3DMeshFileObject Pointer
        /// </summary>
        public IntPtr Percip
        {
            get => Memory.ReadMemory<IntPtr>(Address + 0xdc);
            set => Memory.WriteMemory(Address + 0xdc, value);
        }
        public float Percip_WaySum
        {
            get => Memory.ReadMemory<float>(Address + 0xe0);
            set => Memory.WriteMemory(Address + 0xe0, value);
        }
        public D3DMatrix Percip_LastMat
        {
            get => Memory.ReadMemory<D3DMatrix>(Address + 0xe4);
            set => Memory.WriteMemory(Address + 0xe4, value);
        }
        public D3DVector LastCamPos
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x124);
            set => Memory.WriteMemory(Address + 0x124, value);
        }
        public D3DVector CamPosDiff
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x130);
            set => Memory.WriteMemory(Address + 0x130, value);
        }
        public D3DVector PercipVec
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x13c);
            set => Memory.WriteMemory(Address + 0x13c, value);
        }
        public bool WetGround
        {
            get => Memory.ReadMemory<bool>(Address + 0x148);
            set => Memory.WriteMemory(Address + 0x148, value);
        }
        /* TODO:
        public OmsiSound PercipSound
        {
            get => omsiMemory.ReadMemory<OmsiSound>(baseAddress + 0x14c);
            //set => omsiMemory.WriteMemory(baseAddress + 0x148, value);
        }*/
        public float ActualWindSpeed
        {
            get => Memory.ReadMemory<float>(Address + 0x150);
            set => Memory.WriteMemory(Address + 0x150, value);
        }
        public float ActualWindDir
        {
            get => Memory.ReadMemory<float>(Address + 0x154);
            set => Memory.WriteMemory(Address + 0x154, value);
        }
        public D3DVector ActualWindVec
        {
            get => Memory.ReadMemory<D3DVector>(Address + 0x158);
            set => Memory.WriteMemory(Address + 0x158, value);
        }
        public float RelHum
        {
            get => Memory.ReadMemory<float>(Address + 0x164);
            set => Memory.WriteMemory(Address + 0x164, value);
        }
        public float AbsHum
        {
            get => Memory.ReadMemory<float>(Address + 0x168);
            set => Memory.WriteMemory(Address + 0x168, value);
        }
    }
}
