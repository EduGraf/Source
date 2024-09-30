namespace EduGraf.Geometries;

internal class TriGeometry(float[] position, ushort[] triangles)
    : GeometryBase(position), ITriangle16Mapping
{
    public ushort[] Triangles { get; } = triangles;
}