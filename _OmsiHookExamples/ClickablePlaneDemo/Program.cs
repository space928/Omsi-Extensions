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
                    D3DTexture.RGBA[] texBuffer = new D3DTexture.RGBA[1];
                    texBuffer[0] = new D3DTexture.RGBA() { data=0xffffffff};
                    texture.UpdateTexture(texBuffer.AsMemory(), new OmsiRemoteMethods.Rectangle() { left = (uint)Math.Round(512*((intersect.X+1)/2)), top = (uint)Math.Round(512 * ((intersect.Z + 1) / 2)), right = (uint)Math.Round(512 * ((intersect.X + 1) / 2)), bottom = (uint)Math.Round(512 * ((intersect.Z + 1) / 2)) }).Wait();
                }

                Thread.Sleep(20);
            }
        }
    }
}
