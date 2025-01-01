using System.Collections.Generic;

namespace EduGraf.Geometries;

// See interface.
public class Triangle32Geometry(float[] position, uint[] triangles)
    : ITriangle32Geometry
{
    [Dimension(3)]
    public float[] Position { get; } = position; // see interface property

    public int Count { get; } = position.Length / 3;  // see interface property

    public uint[] Triangles { get; } = triangles; // see interface property

    // See interface method.
    public List<Triangle> GetAllTriangles()
    {
        var list = new List<Triangle>();
        int i = 0;
        while (i < Triangles.Length) list.Add(new(Triangles[i++], Triangles[i++], Triangles[i++]));
        return list;
    }
}
