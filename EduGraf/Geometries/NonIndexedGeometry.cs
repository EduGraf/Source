using System.Collections.Generic;

namespace EduGraf.Geometries;

// Represents a geometry by a completely unrolled list of consecutive triangle corner-positions in the Position property.
public class NonIndexedGeometry(float[] position) : IGeometry
{
    [Dimension(3)]
    public float[] Position { get; } = position; // see interface property

    public int Count { get; } = position.Length / 3; // see interface property

    // See interface method.
    public List<Triangle> GetAllTriangles()
    {
        var triangles = new List<Triangle>();

        uint index = 0;
        for (int number = 0; number < Count / 3; number++) triangles.Add(new(index++, index++, index++));

        return triangles;
    }
}
