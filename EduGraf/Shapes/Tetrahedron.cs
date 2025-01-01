using System;

namespace EduGraf.Shapes;

// Geometric representation of a tetrahedron by triangles indexing distinct vertices.
public static class Tetrahedron
{
    // indices into positions in terms of vertex numbers.
    public static readonly ushort[] Triangles =
    [
        // the base triangle
        0, 1, 2,
        // the sides
        0, 1, 3,
        1, 2, 3,
        2, 0, 3
    ];

    // Return the positions. The height is 1 and the tetrahedron-base is on y=0.
    public static float[] GetPositions(float radius /* of the circle the base positions are on. */)
    {
        var angle = 2f / 3 * MathF.PI;
        var rCos = radius * MathF.Cos(angle);
        var rSin = radius * MathF.Sin(angle);
        return
        [
            radius, 0, 0,
            rCos, 0, rSin,
            rCos, 0,-rSin,
            0, 1, 0
        ];
    }
}