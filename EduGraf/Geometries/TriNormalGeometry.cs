namespace EduGraf.Geometries;

internal class TriNormalGeometry : TriGeometry, INormalMapping
{
    [Dimension(3)]
    public float[] Normal { get; }

    public TriNormalGeometry(float[] position, float[] normals, ushort[] triangles)
        : base(position, triangles)
    {
        Normal = normals;
    }
}