using System;
using System.Numerics;
using OmsiHook;

namespace ClickablePlaneDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("#=#=#=#=#=# OmsiExtensions Paint Sample #=#=#=#=#=#");

            OmsiHook.OmsiHook omsi = new();
            omsi.AttachToOMSI().Wait();
            Console.Clear();
            Paint paint = new(omsi);

            while (true)
            {
                paint.Update();
                Thread.Sleep(15);
            }
        }
    }

    class Paint
    {
        const string MOUSE_EVENT_NAME = "OH_Click";
        const int SCRIPT_TEXTURE_INDEX = 2;
        const int TEXTURE_SIZE = 512;

        private readonly OmsiHook.OmsiHook omsi;
        private readonly D3DTexture texture;
        private OmsiRoadVehicleInst? playerVehicle;
        private OmsiProgMan? progMan;
        private MemArrayList<OmsiAnimSubMesh>? meshes;
        private MemArrayList<OmsiAnimSubMeshInst>? meshInsts;
        private D3DTexture.RGBA[] texBuffer;
        private D3DTexture.RGBA curCol;

        public Paint(OmsiHook.OmsiHook omsi)
        {
            // Try to get the vehicle from Omsi; if this is executed before the map has loaded then these will
            // be null and will be read later.
            this.playerVehicle = omsi.Globals.PlayerVehicle;
            this.progMan = omsi.Globals.ProgamManager;
            this.meshes = playerVehicle?.ComplObjInst?.ComplObj?.Meshes;
            this.meshInsts = playerVehicle?.ComplObjInst?.AnimSubMeshInsts;

            // Create and initialise a new D3DTexture and allocate a buffer to write into.
            this.texture = omsi.CreateTextureObject();
            this.texBuffer = new D3DTexture.RGBA[TEXTURE_SIZE * TEXTURE_SIZE];
            this.curCol = new D3DTexture.RGBA() { data = 0xffff0000 };
            this.omsi = omsi;
            try
            {
                texture.CreateD3DTexture(TEXTURE_SIZE, TEXTURE_SIZE);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
        }

        public void Update()
        {
            // If the vehicle data hasn't yet been read, try getting it now
            playerVehicle ??= omsi.Globals.PlayerVehicle;
            progMan ??= omsi.Globals.ProgamManager;
            meshes ??= playerVehicle?.ComplObjInst?.ComplObj?.Meshes;
            meshInsts ??= playerVehicle?.ComplObjInst?.AnimSubMeshInsts;

            if (meshes == null || meshInsts == null || playerVehicle == null || texture == null)
                return;

            Console.SetCursorPosition(0, 0);
            // Replace the bus' script texture with our newly created texture. This only needs to be done once per session.
            var old = playerVehicle.ComplObjInst.ScriptTextures[SCRIPT_TEXTURE_INDEX];
            if (old.tex != (IntPtr)texture.TextureAddress)
            {
                playerVehicle.ComplObjInst.ScriptTextures[SCRIPT_TEXTURE_INDEX] = new()
                {
                    TexPn = old.TexPn,
                    color = old.color,
                    tex = unchecked((IntPtr)texture.TextureAddress)
                };
            }

            // Draw the colour picker
            for (uint y = 0; y < TEXTURE_SIZE; y++)
            {
                var col = GetStaticRainbowColor(y);
                for (uint x = 0; x <= 32; x++)
                {
                    texBuffer[y * TEXTURE_SIZE + x] = col;
                }
            }
            // Handle colour picking and painting
            var intersect = ComputeCursor();
            if (!float.IsNaN(intersect.X))
            {
                var curX = (uint)Math.Max(0, Math.Round(TEXTURE_SIZE * ((intersect.X + 0.5))));
                var curY = (uint)Math.Max(0, Math.Round(TEXTURE_SIZE * (1 - (intersect.Z + 0.5))));
                Console.WriteLine($"  Clicked on {curX},{curY}".PadRight(Console.WindowWidth - 1));
                if (curX > 32)  // Paint colour
                    SetCirclePixels(ref texBuffer, curX, curY, 8, this.curCol);
                else  // Pick colour
                    this.curCol = GetStaticRainbowColor(curY);
            }

            texture.UpdateTexture(texBuffer.AsMemory()).Wait();
        }

        /// <summary>
        /// This method works out where the mouse is clicking on a given mesh.
        /// <br/>
        /// This does not use a proper mesh-raycast to work out where on the mesh the user is clicking; rather it 
        /// transforms the mouse ray (the ray going out from the camera pointing towards whatever the mouse is 
        /// pointing at) by the inverse of mesh's transformation matrix and then performs a simple ray-plane 
        /// intersection calculation. In the case where the mesh is a simple unit-length plane created at the 
        /// origin and then transformed (ie: a plane with an origin matrix that's centred, rotated, and scaled 
        /// to the plane) then the coordinates returned by this are effectively the same as the UVs of the mesh.
        /// </summary>
        /// <returns>A vector with the relative mouse coordinates, or NaN if the object was not clicked.</returns>
        private Vector3 ComputeCursor()
        {
            Console.WriteLine($"[MOUSE] pos: {progMan.MausPos} ray_pos: {progMan.MausLine3DPos} ray_dir: {progMan.MausLine3DDir}".PadRight(Console.WindowWidth - 1));
            Console.WriteLine($"[MOUSE] {progMan.MausCrossObjFlat} {progMan.MausCrossObjFlat_ObjHeight} mouse_event: {progMan.Maus_MeshEvent}".PadRight(Console.WindowWidth - 1));
            
            OmsiAnimSubMesh? clickMesh = null;
            OmsiAnimSubMeshInst? clickMeshInst = null;
            // Find the mesh with the mouse event we're interested in
            clickMesh = meshes.FirstOrDefault(mesh => mesh.MausEvent == MOUSE_EVENT_NAME);
            if (clickMesh != null)
                clickMeshInst = meshInsts[meshes.IndexOf(clickMesh)];

            if (clickMesh != null && clickMeshInst != null  // Check we found the mesh with the desired mouse event
                && progMan.Maus_MeshEvent == MOUSE_EVENT_NAME  // Check that the user is currently hovering over the object
                && progMan.Maus_Clicked) // Check that the mouse button is down
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
                return intersect;
            }
            return new Vector3() { X = float.NaN, Y = float.NaN, Z = float.NaN};
        }

        public static void SetCirclePixels(ref D3DTexture.RGBA[] texBuffer, uint centerX, uint centerY, uint radius, D3DTexture.RGBA color)
        {
            float invRadius = 1.0f / radius;

            for (uint y = centerY - radius; y <= centerY + radius; y++)
            {
                for (uint x = centerX - radius; x <= centerX + radius; x++)
                {
                    // Calculate the distanceSquared from the center of the circle
                    uint distanceSquared = (x - centerX) * (x - centerX) + (y - centerY) * (y - centerY);

                    // Check if the current pixel is within the circle
                    if (distanceSquared <= radius * radius)
                    {
                        // Check if the current pixel is within the bounds of the array
                        if (x >= 0 && x < TEXTURE_SIZE && y >= 0 && y < TEXTURE_SIZE)
                        {
                            // Apply falloff
                            float weight = 1.0f - MathF.Sqrt(distanceSquared) * invRadius;
                            D3DTexture.RGBA blendedColor = BlendColors(texBuffer[x + (TEXTURE_SIZE * y)], color, weight);

                            texBuffer[x + (TEXTURE_SIZE * y)] = blendedColor;
                        }
                    }
                }
            }
        }

        private static D3DTexture.RGBA BlendColors(D3DTexture.RGBA originalColor, D3DTexture.RGBA newColor, float weight)
        {
            // Perform linear interpolation between the original color and the new color
            byte r = (byte)(originalColor.r + (newColor.r - originalColor.r) * weight);
            byte g = (byte)(originalColor.g + (newColor.g - originalColor.g) * weight);
            byte b = (byte)(originalColor.b + (newColor.b - originalColor.b) * weight);
            byte a = (byte)(originalColor.a + (newColor.a - originalColor.a) * weight);

            return new D3DTexture.RGBA() { r = r, g = g, b = b, a = a };
        }


        static D3DTexture.RGBA GetStaticRainbowColor(uint curY)
        {
            // Define the static colors in the rainbow
            uint[] staticColors = { 0xffff0000, 0xffffff00, 0xff00ff00, 0xff00ffff, 0xff0000ff, 0xffff00ff, 0xff000000 };
            // Calculate the index of the static color based on the current Y value
            int colorIndex = (int)((float)curY / TEXTURE_SIZE * (staticColors.Length));

            // Get the corresponding static color based on the index
            uint selectedColor = staticColors[Math.Min(Math.Max(colorIndex, 0), staticColors.Length - 1)];

            // Create RGBA color from the selected static color
            D3DTexture.RGBA curCol = new() { data = selectedColor };

            return curCol;
        }
    }
}
