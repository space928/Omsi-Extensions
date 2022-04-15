using System;

namespace OmsiHook
{
    public class OmsiWeather : OmsiObject
    {
        internal OmsiWeather(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }

        //Todo: WeatherSchemes - OmsiWeatherProp[] @ 0x4

        public OmsiWeatherProp ActWeather
        {
            get => omsiMemory.ReadMemory<OmsiWeatherProp>(baseAddress + 0x8);
            set => omsiMemory.WriteMemory(baseAddress + 0x8, value);
        }
        public float PriLgtFactor
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x58);
            set => omsiMemory.WriteMemory(baseAddress + 0x58, value);
        }
        public float SecLgtFactor
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x5c);
            set => omsiMemory.WriteMemory(baseAddress + 0x5c, value);
        }
        public float AmbLgtFactor
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x60);
            set => omsiMemory.WriteMemory(baseAddress + 0x60, value);
        }
        public D3DColorValue LightPri
        {
            get => omsiMemory.ReadMemory<D3DColorValue>(baseAddress + 0x64);
            set => omsiMemory.WriteMemory(baseAddress + 0x64, value);
        }
        public D3DColorValue LightSec
        {
            get => omsiMemory.ReadMemory<D3DColorValue>(baseAddress + 0x74);
            set => omsiMemory.WriteMemory(baseAddress + 0x74, value);
        }
        public D3DColorValue LightAmb
        {
            get => omsiMemory.ReadMemory<D3DColorValue>(baseAddress + 0x84);
            set => omsiMemory.WriteMemory(baseAddress + 0x84, value);
        }
        public D3DColorValue LightAll
        {
            get => omsiMemory.ReadMemory<D3DColorValue>(baseAddress + 0x94);
            set => omsiMemory.WriteMemory(baseAddress + 0x94, value);
        }
        public float ActFogDensity
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0xa4);
            set => omsiMemory.WriteMemory(baseAddress + 0xa4, value);
        }
        public float ActLightness
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0xa8);
            set => omsiMemory.WriteMemory(baseAddress + 0xa8, value);
        }
        /// <summary>
        /// Brightness
        /// </summary>
        public float Helligkeit
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0xac);
            set => omsiMemory.WriteMemory(baseAddress + 0xac, value);
        }
        /// <summary>
        /// Twilight switch brightness?
        /// </summary>
        public float Daemmerungsschalter_Helligkeit
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0xb0);
            set => omsiMemory.WriteMemory(baseAddress + 0xb0, value);
        }
        public int CloudTypeNum
        {
            get => omsiMemory.ReadMemory<int>(baseAddress + 0xb4);
            set => omsiMemory.WriteMemory(baseAddress + 0xb4, value);
        }
        /// <summary>
        /// TD3DMeshFileObject Pointer
        /// </summary>
        public IntPtr Clouds
        {
            get => omsiMemory.ReadMemory<IntPtr>(baseAddress + 0xb8);
            set => omsiMemory.WriteMemory(baseAddress + 0xb8, value);
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
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0xc0);
            set => omsiMemory.WriteMemory(baseAddress + 0xc0, value);
        }
        public float CloudTransparenz
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0xcc);
            set => omsiMemory.WriteMemory(baseAddress + 0xcc, value);
        }
        public byte CloudCat
        {
            get => omsiMemory.ReadMemory<byte>(baseAddress + 0xd0);
            set => omsiMemory.WriteMemory(baseAddress + 0xd0, value);
        }

        /*TODO:
         * public OmsiPartikelemitter PercipSystem
        {
            get => omsiMemory.ReadMemory<OmsiPartikelemitter>(baseAddress + 0xd4);
            //set => omsiMemory.WriteMemory(baseAddress + 0xd0, value);
        }*/
        public bool PercipSet
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0xd8);
            set => omsiMemory.WriteMemory(baseAddress + 0xd8, value);
        }
        /// <summary>
        /// TD3DMeshFileObject Pointer
        /// </summary>
        public IntPtr Percip
        {
            get => omsiMemory.ReadMemory<IntPtr>(baseAddress + 0xdc);
            set => omsiMemory.WriteMemory(baseAddress + 0xdc, value);
        }
        public float Percip_WaySum
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0xe0);
            set => omsiMemory.WriteMemory(baseAddress + 0xe0, value);
        }
        public D3DMatrix Percip_LastMat
        {
            get => omsiMemory.ReadMemory<D3DMatrix>(baseAddress + 0xe4);
            set => omsiMemory.WriteMemory(baseAddress + 0xe4, value);
        }
        public D3DVector LastCamPos
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x124);
            set => omsiMemory.WriteMemory(baseAddress + 0x124, value);
        }
        public D3DVector CamPosDiff
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x130);
            set => omsiMemory.WriteMemory(baseAddress + 0x130, value);
        }
        public D3DVector PercipVec
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x13c);
            set => omsiMemory.WriteMemory(baseAddress + 0x13c, value);
        }
        public bool WetGround
        {
            get => omsiMemory.ReadMemory<bool>(baseAddress + 0x148);
            set => omsiMemory.WriteMemory(baseAddress + 0x148, value);
        }
        /* TODO:
        public OmsiSound PercipSound
        {
            get => omsiMemory.ReadMemory<OmsiSound>(baseAddress + 0x14c);
            //set => omsiMemory.WriteMemory(baseAddress + 0x148, value);
        }*/
        public float ActualWindSpeed
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x150);
            set => omsiMemory.WriteMemory(baseAddress + 0x150, value);
        }
        public float ActualWindDir
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x154);
            set => omsiMemory.WriteMemory(baseAddress + 0x154, value);
        }
        public D3DVector ActualWindVec
        {
            get => omsiMemory.ReadMemory<D3DVector>(baseAddress + 0x158);
            set => omsiMemory.WriteMemory(baseAddress + 0x158, value);
        }
        public float RelHum
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x164);
            set => omsiMemory.WriteMemory(baseAddress + 0x164, value);
        }
        public float AbsHum
        {
            get => omsiMemory.ReadMemory<float>(baseAddress + 0x168);
            set => omsiMemory.WriteMemory(baseAddress + 0x168, value);
        }
    }
}
