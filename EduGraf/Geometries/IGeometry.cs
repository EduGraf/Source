using System.Collections.Generic;

namespace EduGraf.Geometries;

// This is the base interface for all geometries.
public interface IGeometry
{
    public float[] Position { get; } // unrolled vertex positions (x, y, z)

    public int Count { get; } // of the vertices

    // Returns all triangles with indices to vertices.
    public List<Triangle> GetAllTriangles();
}