namespace EduGraf.Geometries;

internal class FlatTriangleGeometry : FlatGeometry, ITriangle32Mapping
{
    public uint[] Triangles { get; }

    public FlatTriangleGeometry(float[] position, uint[] triangles)
        : base(position)
    {
        Triangles = triangles;
    }
}