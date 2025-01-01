namespace EduGraf.Geometries;

// Represents a geometry by a list of consecutive triangle corners indexing into the Position property.
public interface ITriangle16Geometry : IGeometry
{
    public ushort[] Triangles { get; } // unrolled list of vertex numbers
}