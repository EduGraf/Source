namespace EduGraf.Geometries;

internal class FlatTexturedTriangleGeometry : FlatTriangleGeometry, IUvMapping
{
    [Dimension(2)]
    public float[] TextureUv { get; }

    public FlatTexturedTriangleGeometry(float[] position, uint[] triangles, float[] textureUv)
        : base(position, triangles)
    {
        TextureUv = textureUv;
    }
}