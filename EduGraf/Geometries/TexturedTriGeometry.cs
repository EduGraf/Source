namespace EduGraf.Geometries;

internal class TexturedTriGeometry : TriGeometry, IUvMapping
{
    [Dimension(2)]
    public float[] TextureUv { get; }

    public TexturedTriGeometry(float[] position, ushort[] triangles, float[] textureUvs)
        : base(position, triangles)
    {
        TextureUv = textureUvs;
    }
}