namespace EduGraf.Geometries;

internal class NormalGeometry(float[] position, float[] normals)
    : GeometryBase(position), INormalMapping
{
    [Dimension(3)]
    public float[] Normal { get; } = normals;
}