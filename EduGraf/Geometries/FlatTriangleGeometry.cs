namespace EduGraf.Geometries;

internal class FlatTriangleGeometry(float[] position, uint[] triangles)
    : FlatGeometry(position), ITriangle32Mapping
{
    public uint[] Triangles { get; } = triangles;
}