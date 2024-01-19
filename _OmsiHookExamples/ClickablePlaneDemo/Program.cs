// Ignore Spelling: Clickable

using System;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading;
using OmsiHook;

namespace ClickablePlaneDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("#=#=#=#=#=# OmsiExtensions Testing #=#=#=#=#=#");

            OmsiHook.OmsiHook omsi = new();
            omsi.AttachToOMSI().Wait();
            Console.Clear();

            var playerVehicle = omsi.Globals.PlayerVehicle;
            var progMan = omsi.Globals.ProgamManager;
            var meshes = playerVehicle?.ComplObjInst?.ComplObj?.Meshes;
            var meshInsts = playerVehicle?.ComplObjInst?.AnimSubMeshInsts;
            var texture = omsi.CreateTextureObject();
            D3DTexture.RGBA[] texBuffer = new D3DTexture.RGBA[512*512];
            var curCol = new D3DTexture.RGBA() { data = 0xffff0000 };

            try
            {
                texture.CreateD3DTexture(512, 512);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
            string mouseEventName = "OH_Click";
            int scriptTextureIndex = 2;


            while (true)
            {
                playerVehicle ??= omsi.Globals.PlayerVehicle;
                progMan ??= omsi.Globals.ProgamManager;
                meshes ??= playerVehicle?.ComplObjInst?.ComplObj?.Meshes;
                meshInsts ??= playerVehicle?.ComplObjInst?.AnimSubMeshInsts;
                if (meshes == null || meshInsts == null || playerVehicle == null)
                {
                    Thread.Sleep(20);
                    continue;
                }
                Console.SetCursorPosition(0, 0);
                var old = playerVehicle.ComplObjInst.ScriptTextures[scriptTextureIndex];
                if (old.tex != (IntPtr)texture.TextureAddress)
                {
                    playerVehicle.ComplObjInst.ScriptTextures[scriptTextureIndex] = new()
                    {
                        TexPn = old.TexPn,
                        color = old.color,
                        tex = unchecked((IntPtr)texture.TextureAddress)
                    };
                }

                for (uint y = 0; y < 512;  y++)
                {
                    var col = GetStaticRainbowColor(y);
                    for (uint x = 0; x <= 32; x++)
                    {
                        texBuffer[y * 512 + x] = col;
                    }
                }
                texture.UpdateTexture(texBuffer.AsMemory()).Wait();


                Console.WriteLine($"[MOUSE] pos: {progMan.MausPos} ray_pos: {progMan.MausLine3DPos} ray_dir: {progMan.MausLine3DDir}".PadRight(Console.WindowWidth - 1));
                Console.WriteLine($"[MOUSE] {progMan.MausCrossObjFlat} {progMan.MausCrossObjFlat_ObjHeight} {progMan.Maus_MeshEvent}".PadRight(Console.WindowWidth - 1));
                OmsiAnimSubMesh clickMesh = null;
                OmsiAnimSubMeshInst clickMeshInst = null;
                clickMesh = meshes.FirstOrDefault(mesh => mesh.MausEvent == mouseEventName);
                if (clickMesh != null)
                    clickMeshInst = meshInsts[meshes.IndexOf(clickMesh)];
                if (clickMesh != null && clickMeshInst != null && progMan.Maus_MeshEvent == mouseEventName && (progMan.Maus_Clicked))
                {
                    // Work out object space coords of the mouse click
                    // Note that (if wrapped) we could use D3DXIntersect to find the UV coords given the sub-mesh's d3d mesh

                    // Transform the mouse ray from world space to object space
                    Matrix4x4.Invert((Matrix4x4)clickMeshInst.Matrix, out Matrix4x4 invMat);
                    Matrix4x4.Invert(clickMesh.OriginMatrix, out Matrix4x4 invOriginMat);
                    invMat = Matrix4x4.Multiply(invMat, invOriginMat);

                    var rayStart = Vector3.Transform(progMan.MausLine3DPos, invMat);
                    var rayDir = Vector3.Transform(progMan.MausLine3DDir, invMat);

                    // Now trace the ray, in our simplification we assume the surface of the mesh we want to hit is coplanar to the x/z plane
                    // Taken from: https://stackoverflow.com/a/18543221
                    Vector3 planeNrm = new(0, 1, 0);
                    float planeD = 0;
                    float dot = Vector3.Dot(planeNrm, rayDir);

                    var intersect = new Vector3();
                    if (Math.Abs(dot) > 1e-9)
                    {
                        var w = rayStart - planeNrm * planeD;
                        var fac = -Vector3.Dot(planeNrm, w) / dot;
                        var u = rayDir * fac;
                        intersect = rayStart + u;
                    }

                    Console.WriteLine($"  Clicked on {clickMesh.Filename_Int} at local coords: {(D3DVector)intersect}");
                    var curX = (uint)Math.Max(0, Math.Round(512 * ((intersect.X + 0.5))));
                    var curY = (uint)Math.Max(0, Math.Round(512 * (1 - (intersect.Z + 0.5))));
                    Console.WriteLine($"  Clicked on {curX},{curY}".PadRight(Console.WindowWidth-1));
                    if (curX > 32)
                        SetCirclePixels(ref texBuffer, curX, curY, 8, curCol);
                    else
                        curCol = GetStaticRainbowColor(curY);
                    texture.UpdateTexture(texBuffer.AsMemory()).Wait();
                }

                Thread.Sleep(20);
            }
        }

        public static void SetCirclePixels(ref D3DTexture.RGBA[] texBuffer, uint centerX, uint centerY, uint radius, D3DTexture.RGBA color)
        {
            for (uint y = centerY - radius; y <= centerY + radius; y++)
            {
                for (uint x = centerX - radius; x <= centerX + radius; x++)
                {
                    // Check if the current pixel is within the circle
                    if ((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY) <= radius * radius)
                    {
                        // Check if the current pixel is within the bounds of the array
                        if (x >= 33 && x < 512 && y >= 0 && y < 512)
                        {
                            texBuffer[x + (512 * y)] = color;
                        }
                    }
                }
            }
        }


        static D3DTexture.RGBA GetStaticRainbowColor(uint curY)
        {

            // Define the static colors in the rainbow
            uint[] staticColors = { 0xffff0000, 0xffffff00, 0xff00ff00, 0xff00ffff, 0xff0000ff, 0xffff00ff, 0xff000000 };
            // Calculate the index of the static color based on the current Y value
            int colorIndex = (int)((float)curY / 512 * (staticColors.Length));

            // Get the corresponding static color based on the index
            uint selectedColor = staticColors[Math.Min(Math.Max(colorIndex, 0), staticColors.Length - 1)];

            // Create RGBA color from the selected static color
            D3DTexture.RGBA curCol = new D3DTexture.RGBA() { data = selectedColor };

            return curCol;
        }
    }
}
