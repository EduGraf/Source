namespace EduGraf.Geometries;

internal class TexturedGeometry : GeometryBase, IUvMapping
{
    [Dimension(2)]
    public float[] TextureUv { get; }

    public TexturedGeometry(float[] position, float[] textureUvs)
        : base(position)
    {
        TextureUv = textureUvs;
    }
}