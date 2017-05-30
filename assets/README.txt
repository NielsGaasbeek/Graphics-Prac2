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

When you touch the controls, the rendered imaga will not change. to render the scene, press space.

How to add scene objects:
Spheres: string ID, Vector3 position, float radius, vector3 color, string material("Diffuse", "Specular", "Mirror")
Plane: string ID, Vector3 NormalVector, float distance to origin, Vector3 color, string material
Triangle: string ID, Vector3 vertice0, Vector3 vertice1, Vector3 vertice2, Vector 3 color, string material

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
