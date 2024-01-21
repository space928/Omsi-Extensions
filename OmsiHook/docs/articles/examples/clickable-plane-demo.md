# Clickable Plane Demo

This article provides a basic understanding to a basic C# .NET example leveraging the OMSIHook library. This demo is a more advanced 
demo using a specially made mesh (available [here](/doc-resources/ClickablePlaneDemo/_touch_surface.o3d)) and some of the model data properties to paint onto a script texture.

_This article is in direct relation to the Sample Project available [here](https://github.com/space928/Omsi-Extensions/tree/main/_OmsiHookExamples/ClickablePlaneDemo)._

![Paint Demo](/images/Paint-demo.png)

## Coordinate calculation
To achieve the click detection, the global mouse events are monitored, a ray is cast from the cursor onto the plane, it's intersection is computed and converted into a local coordinate space.

## The Mesh
The mesh has to be created in a specific way to allow it to be computed correctly due to how the data is accessed, we use the object transformation matrix rather than the raw vertex positions, as such for the best effect a 1m by 1m plane should be created then use the mesh transformation matrix to position scale and rotate it. This would limit this technique to flat planes however in most use cases this is an acceptable compromise.

## Model.CFG
The model configuration file is a fairly simple one - simply defining a script texture, mesh and a mouse event

```ini
2: OH Paint Demo
[scripttexture]
1024
1024


-=-=-=-=-=-=-=-=-=-=-

[mesh]
_touch_surface.o3d

[matl]
D_Matrix.bmp
0

[useScriptTexture]
2

[mouseevent]
OH_Click
```