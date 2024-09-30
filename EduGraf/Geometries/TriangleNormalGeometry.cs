namespace EduGraf.Geometries;

internal class TriangleNormalGeometry(float[] position, float[] normals, uint[] triangles)
    : TriangleGeometry(position, triangles), INormalMapping
{
    [Dimension(3)]
    public float[] Normal { get; } = normals;
}