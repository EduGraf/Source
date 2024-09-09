namespace EduGraf.Geometries;

// This interface declares a geometry to be defined in terms of triangles indexing vertices.
public interface ITriangle16Mapping : IGeometry
{
    // unrolled list of vertex numbers
    ushort[] Triangles { get; }
}