namespace EduGraf.Geometries;

internal class TriangleGeometry(float[] position, uint[] triangles)
    : GeometryBase(position), ITriangle32Mapping
{
    public uint[] Triangles { get; } = triangles;
}
