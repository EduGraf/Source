namespace EduGraf.Geometries;

internal class NormalGeometry : GeometryBase, INormalMapping
{
    [Dimension(3)]
    public float[] Normal { get; }

    public NormalGeometry(float[] position, float[] normals)
        : base(position)
    {
        Normal = normals;
    }
}