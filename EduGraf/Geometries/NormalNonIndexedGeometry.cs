namespace EduGraf.Geometries;

internal class NormalNonIndexedGeometry(float[] position, float[] normals)
    : NonIndexedGeometry(position), INormalMapping
{
    [Dimension(3)]
    public float[] Normal { get; } = normals;
}