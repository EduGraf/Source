using System.Collections.Generic;

namespace EduGraf.Geometries;

// See interface.
public class Triangle16Geometry(float[] position, ushort[] triangles)
    : ITriangle16Geometry
{
    [Dimension(3)]
    public float[] Position => position; // see interface property

    public int Count { get; } = position.Length / 3; // see interface property

    public ushort[] Triangles => triangles; // see interface property

    // See interface method.
    public List<Triangle> GetAllTriangles()
    {
        var list = new List<Triangle>();
        int i = 0;
        while (i < Triangles.Length) list.Add(new(Triangles[i++], Triangles[i++], Triangles[i++]));
        return list;
    }
}

