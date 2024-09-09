namespace EduGraf.Geometries;

internal class TriangleNormalGeometry : TriangleGeometry, INormalMapping
{
    [Dimension(3)]
    public float[] Normal { get; }

    public TriangleNormalGeometry(float[] position, float[] normals, uint[] triangles)
        : base(position, triangles)
    {
        Normal = normals;
    }
}