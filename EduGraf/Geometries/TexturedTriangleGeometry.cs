namespace EduGraf.Geometries;

internal class TexturedTriangleGeometry : TriangleGeometry, IUvMapping
{
    [Dimension(2)]
    public float[] TextureUv { get; }

    public TexturedTriangleGeometry(float[] position, uint[] triangles, float[] textureUvs)
        : base(position, triangles)
    {
        TextureUv = textureUvs;
    }
}