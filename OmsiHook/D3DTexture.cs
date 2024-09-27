﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static OmsiHook.OmsiRemoteMethods;

namespace OmsiHook
{
    public class D3DTexture : OmsiObject, IDisposable
    {
        internal D3DTexture(Memory omsiMemory, int baseAddress) : base(omsiMemory, baseAddress) { }
        /// <summary>
        /// <see cref="D3DTexture"/> objects must be constructed using the <see cref="OmsiHook.CreateTextureObject"/> factory method. 
        /// This method is for internal use only.
        /// </summary>
        public D3DTexture() : base() { }

        private uint width, height;
        private D3DFORMAT format;
        private uint levels;
        private int stagingBufferSize;
        // private byte[] stagingBuffer;
        private uint remoteStagingBufferPtr;
        private const bool useFastAlloc = true;

        /// <summary>
        /// Gets the address of the native <c>IDirect3DTexture9</c> object.
        /// </summary>
        public uint TextureAddress => unchecked((uint)Address);
        /// <summary>
        /// Gets the width of the texture.
        /// </summary>
        public uint Width => width;
        /// <summary>
        /// Gets the height of the texture.
        /// </summary>
        public uint Height => height;
        /// <summary>
        /// Gets the D3DFormat of the texture.
        /// </summary>
        public D3DFORMAT Format => format;
        /// <summary>
        /// Gets the number of mipmap levels in the texture.
        /// </summary>
        public uint Levels => levels;
        /// <summary>
        /// Returns true if this object is initialised.
        /// </summary>
        public bool IsValid => TextureAddress != 0 && isCreated;
        /// <summary>
        /// Returns true if this texture is valid and can be written too.
        /// </summary>
        public bool CanWrite => IsValid && remoteStagingBufferPtr != 0;

        private volatile bool isCreated = false;

        internal override void InitObject(Memory memory, int address)
        {
            base.InitObject(memory, address);
            // This method may be called repeadedly when marshalling an array of textures. To save performance,
            // we don't await the texture creation (which reads the width/height/... of the texture) and don't
            // enable writing by default.
            if(address != 0)
                _ = CreateFromExisting((uint)address, false);
        }

        /// <summary>
        /// Initialises this <see cref="D3DTexture"/> from an existing <c>IDirect3DTexture9</c>.
        /// </summary>
        /// <param name="address">the address of the existing texture</param>
        /// <param name="initialiseForWriting">whether to allow writing to this texture</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task CreateFromExisting(uint address, bool initialiseForWriting = true)
        {
            if (Address == 0)
                throw new NullReferenceException("Texture was already null!");

            var desc = await Memory.RemoteMethods.OmsiGetTextureDescAsync(address, 0);
            if (HRESULTFailed(desc.hresult))
                throw new Exception(desc.hresult.ToString());

            width = desc.width; 
            height = desc.height;
            format = desc.format;
            levels = await Memory.RemoteMethods.OmsiGetTextureLevelCountAsync(address);
            Address = unchecked((int)address);
            stagingBufferSize = (int)width * (int)height * BitsPerPixel(format) / 8;
            isCreated = true;
            if (initialiseForWriting)
                await InitialiseForWriting();
        }

        /// <summary>
        /// Initialises the texture for writing operations.
        /// </summary>
        public async Task InitialiseForWriting()
        {
            if (!IsValid)
                throw new NotInitialisedException("The texture has not been created yet! Make sure to call CreateD3DTexture().");
            if(remoteStagingBufferPtr != 0)
            {
                Memory.FreeRemoteMemory(remoteStagingBufferPtr, fastAlloc: useFastAlloc);
                remoteStagingBufferPtr = 0;
            }
            remoteStagingBufferPtr = unchecked((uint)await Memory.AllocRemoteMemory(stagingBufferSize, fastAlloc: useFastAlloc));
        }

        /// <summary>
        /// Creates a new <c>IDirect3DTexture9</c> with the given parameters and allocates a staging buffer for it.
        /// </summary>
        /// <param name="width">the width of the texture</param>
        /// <param name="height">the height of the texture</param>
        /// <param name="format">the <see cref="D3DFORMAT"/> of the texture</param>
        /// <param name="levels">the number of mipmap levels to create</param>
        /// <param name="initialiseForWriting">whether to allow writing to this texture</param>
        /// <exception cref="Exception"></exception>
        public async Task CreateD3DTexture(uint width, uint height, D3DFORMAT format = D3DFORMAT.D3DFMT_A8R8G8B8, uint levels = 1, bool initialiseForWriting = true)
        {
            if(Address != 0)
                await ReleaseTexture();

            var (hresult, pTexture) = await Memory.RemoteMethods.OmsiCreateTextureAsync(width, height, format, levels);
            if (HRESULTFailed(hresult))
                throw new Exception("Couldn't create D3D texture! Result: " + hresult);

            this.width = width;
            this.height = height;
            this.format = format;
            this.levels = levels;
            Address = unchecked((int)pTexture);
            stagingBufferSize = (int)width * (int)height * BitsPerPixel(format) / 8;
            isCreated = true;
            if (initialiseForWriting)
                await InitialiseForWriting();
        }

        /// <summary>
        /// Releases the native <c>IDirect3DTexture9</c> object and frees the staging buffer.
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task ReleaseTexture()
        {
            if (Address == 0)
                return;
            //    throw new NullReferenceException("Texture was already null!");

            isCreated = false;
            HRESULT hr = await Memory.RemoteMethods.OmsiReleaseTextureAsync(TextureAddress);
            if(HRESULTFailed(hr))
                throw new Exception(hr.ToString());
            Address = 0;
            Memory.FreeRemoteMemory(remoteStagingBufferPtr, fastAlloc: useFastAlloc);
            //stagingBuffer = null;
            remoteStagingBufferPtr = 0;
        }

        /// <summary>
        /// Updates the contents of the <c>IDirect3DTexture9</c> object from a buffer.
        /// </summary>
        /// <remarks>
        /// This method can cause crashes if the texture becomes invalidated by Omsi. This can in certain circumstances 
        /// happen if the script texture is updated in OmsiScript.
        /// </remarks>
        /// <typeparam name="T">The pixel element type</typeparam>
        /// <param name="textureData">a buffer of memory containing the new texture data layed out sequentially in row-major order</param>
        /// <param name="updateArea">optionally, a rectangle specifying the area of the texture to update; the <paramref name="textureData"/> 
        /// must match the size of this rectangle; pass in <see langword="null"/> to update the whole texture</param>
        /// <param name="level">optionally, the mipmap level to update</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task UpdateTexture<T>(Memory<T> textureData, Rectangle? updateArea = null, uint level = 0) where T : unmanaged
        {
            if (!Memory.RemoteMethods.IsInitialised || remoteStagingBufferPtr == 0)
                throw new NotInitialisedException("D3DTexture object must be initialised for write before it can be updated! ");
            if((textureData.Length * Unsafe.SizeOf<T>()) > stagingBufferSize)
                throw new ArgumentOutOfRangeException(nameof(textureData));

            Memory.WriteMemory(remoteStagingBufferPtr, textureData.Span);

            uint dataWidth = (updateArea?.right - updateArea?.left) ?? width;
            uint dataHeight = (updateArea?.bottom - updateArea?.top) ?? height;

            HRESULT hr = await Memory.RemoteMethods.OmsiUpdateTextureAsync(TextureAddress, remoteStagingBufferPtr, dataWidth, dataHeight, updateArea, level);
            if (HRESULTFailed(hr))
                throw new Exception("Couldn't update D3D texture! Result: " + hr);
        }

        /// <summary>
        /// Disposes of this texture by calling <see cref="ReleaseTexture"/> and waiting synchronously.
        /// </summary>
        public void Dispose()
        {
            ReleaseTexture().Wait();
        }

        /// <summary>
        /// A struct representing RGBA pixel data. Compatible with <see cref="D3DFORMAT.D3DFMT_A8R8G8B8"/>.
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 4)]
        public struct RGBA
        {
            [FieldOffset(0)] public byte r;
            [FieldOffset(1)] public byte g;
            [FieldOffset(2)] public byte b;
            [FieldOffset(3)] public byte a;

            [FieldOffset(0)] public uint data;
        }

        /// <summary>
        /// Returns how many bits per pixel a given <see cref="D3DFORMAT"/> uses. Note that compressed formats may use less than 1 byte per pixel.
        /// </summary>
        /// <param name="fmt">the format to query</param>
        /// <returns>the number of bits per pixel for that format.</returns>
        public static int BitsPerPixel(D3DFORMAT fmt)
        {
            switch (fmt)
            {
                // 4 bpp
                case D3DFORMAT.D3DFMT_DXT1:
                case D3DFORMAT.D3DFMT_DXT2:
                case D3DFORMAT.D3DFMT_DXT3:
                case D3DFORMAT.D3DFMT_DXT4:
                case D3DFORMAT.D3DFMT_DXT5:
                    return 4;
                // 8 bpp
                case D3DFORMAT.D3DFMT_R3G3B2:
                case D3DFORMAT.D3DFMT_A8:
                case D3DFORMAT.D3DFMT_P8:
                case D3DFORMAT.D3DFMT_L8:
                    return 8;
                // 16 bpp
                case D3DFORMAT.D3DFMT_R5G6B5:
                case D3DFORMAT.D3DFMT_X1R5G5B5:
                case D3DFORMAT.D3DFMT_A1R5G5B5:
                case D3DFORMAT.D3DFMT_A4R4G4B4:
                case D3DFORMAT.D3DFMT_A8R3G3B2:
                case D3DFORMAT.D3DFMT_X4R4G4B4:
                case D3DFORMAT.D3DFMT_A8P8:
                case D3DFORMAT.D3DFMT_A8L8:
                case D3DFORMAT.D3DFMT_A4L4:
                case D3DFORMAT.D3DFMT_V8U8:
                case D3DFORMAT.D3DFMT_L6V5U5:
                case D3DFORMAT.D3DFMT_R8G8_B8G8:
                case D3DFORMAT.D3DFMT_YUY2:
                case D3DFORMAT.D3DFMT_G8R8_G8B8:
                case D3DFORMAT.D3DFMT_D16_LOCKABLE:
                case D3DFORMAT.D3DFMT_D15S1:
                case D3DFORMAT.D3DFMT_D16:
                case D3DFORMAT.D3DFMT_L16:
                case D3DFORMAT.D3DFMT_INDEX16:
                case D3DFORMAT.D3DFMT_R16F:
                case D3DFORMAT.D3DFMT_CxV8U8:
                    return 16;
                // 24 bpp
                case D3DFORMAT.D3DFMT_R8G8B8:
                    return 24;
                // 32 bpp
                case D3DFORMAT.D3DFMT_A8R8G8B8:
                case D3DFORMAT.D3DFMT_X8R8G8B8:
                case D3DFORMAT.D3DFMT_A2B10G10R10:
                case D3DFORMAT.D3DFMT_A8B8G8R8:
                case D3DFORMAT.D3DFMT_X8B8G8R8:
                case D3DFORMAT.D3DFMT_G16R16:
                case D3DFORMAT.D3DFMT_A2R10G10B10:
                case D3DFORMAT.D3DFMT_X8L8V8U8:
                case D3DFORMAT.D3DFMT_Q8W8V8U8:
                case D3DFORMAT.D3DFMT_V16U16:
                case D3DFORMAT.D3DFMT_A2W10V10U10:
                case D3DFORMAT.D3DFMT_UYVY:
                case D3DFORMAT.D3DFMT_D32:
                case D3DFORMAT.D3DFMT_D24S8:
                case D3DFORMAT.D3DFMT_D24X8:
                case D3DFORMAT.D3DFMT_D24X4S4:
                case D3DFORMAT.D3DFMT_D32F_LOCKABLE:
                case D3DFORMAT.D3DFMT_D24FS8:
                case D3DFORMAT.D3DFMT_INDEX32:
                case D3DFORMAT.D3DFMT_MULTI2_ARGB8: // Maybe?
                case D3DFORMAT.D3DFMT_G16R16F:
                case D3DFORMAT.D3DFMT_R32F:
                    return 32;
                // 64 bpp
                case D3DFORMAT.D3DFMT_A16B16G16R16:
                case D3DFORMAT.D3DFMT_Q16W16V16U16:
                case D3DFORMAT.D3DFMT_A16B16G16R16F:
                case D3DFORMAT.D3DFMT_G32R32F:
                    return 64;
                // 128 bpp
                case D3DFORMAT.D3DFMT_A32B32G32R32F:
                    return 128;
                // 0 bpp
                case D3DFORMAT.D3DFMT_UNKNOWN:
                case D3DFORMAT.D3DFMT_VERTEXDATA:
                    return 0;
                default:
                    return 0;
            }
        }
    }
}
