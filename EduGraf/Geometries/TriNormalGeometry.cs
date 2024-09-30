namespace EduGraf.Geometries;

internal class TriNormalGeometry(float[] position, float[] normals, ushort[] triangles)
    : TriGeometry(position, triangles), INormalMapping
{
    [Dimension(3)]
    public float[] Normal { get; } = normals;
}