namespace EduGraf.Geometries;

internal class TriGeometry : GeometryBase, ITriangle16Mapping
{
    public ushort[] Triangles { get; }

    public TriGeometry(float[] position, ushort[] triangles)
        : base(position)
    {
        Triangles = triangles;
    }
}