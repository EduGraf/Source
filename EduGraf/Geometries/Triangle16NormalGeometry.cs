namespace EduGraf.Geometries;

internal class Triangle16NormalGeometry(float[] position, float[] normals, ushort[] triangles)
    : Triangle16Geometry(position, triangles), INormalMapping
{
    [Dimension(3)]
    public float[] Normal { get; } = normals;
}