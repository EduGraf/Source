namespace EduGraf.Geometries;

internal class FlatTriGeometry : FlatGeometry, ITriangle16Mapping
{
    public ushort[] Triangles { get; }

    public FlatTriGeometry(float[] position, ushort[] triangles)
        : base(position)
    {
        Triangles = triangles;
    }
}