namespace EduGraf.Geometries;

internal class TriangleGeometry : GeometryBase, ITriangle32Mapping
{
    public uint[] Triangles { get; }

    public TriangleGeometry(float[] position, uint[] triangles)
        : base(position)
    {
        Triangles = triangles;
    }
}
