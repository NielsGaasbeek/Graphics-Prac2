Niels Gaasbeek 		5850983
Brian van Beusekom 	5899192
Job van Zelm 		5984394

Camera controls:
Translation:
up: leftshift
down: leftcontrol
right: d
left: a
forward: w
backward: s

Rotation:
up: Uparrow
down: Downarrow
right: Rightarrow
left: Leftarrow

When you touch the controls, the rendered image will dissappear and the camera-position can be seen in the debug-screen. 
After you have selected the desired location and orientation, you can then press space to rerender the scene.

How to add scene objects:
Spheres: string ID, Vector3 position, float radius, vector3 color, string material
Plane: string ID, Vector3 NormalVector, float distance to origin, Vector3 color, string material
Triangle: string ID, Vector3 vertice0, Vector3 vertice1, Vector3 vertice2, Vector 3 color, string material
Light: Vector3 position, Vector3 color, int intensity

The specified string material can be one of the set {"Diffuse", "Specular", "Mirror"}
, it will default to Diffuse if the material is not recognised

After creating the scene object, it has to be added to the appropriate List:
-Primitives go to sceneObjects.Add()
-Lights go to Lights.Add()

Bonus assignments:
Textured Skydome
Anti Aliasing
Triangles

Textured Skydome:
in the class raytracer a function GetEnvironment() calculates how to draw the skydome.
the vector is called when no intersections are found in Render() or Trace()

Anti Aliasing:
constant grid super sampling is used for anti aliasing.
the code is found in Render() in the class RayTracer in the form AA[...]

Triangles:
an addition primitive triangle can be found as Triangle.cs with the other primitives.
