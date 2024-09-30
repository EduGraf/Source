namespace EduGraf.Geometries;

internal class FlatTriGeometry(float[] position, ushort[] triangles)
    : FlatGeometry(position), ITriangle16Mapping
{
    public ushort[] Triangles { get; } = triangles;
}