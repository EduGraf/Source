namespace EduGraf.Geometries;

internal class Triangle32NormalGeometry(float[] position, float[] normals, uint[] triangles)
    : Triangle32Geometry(position, triangles), INormalMapping
{
    [Dimension(3)]
    public float[] Normal { get; } = normals;
}