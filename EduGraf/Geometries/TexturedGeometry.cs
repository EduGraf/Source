namespace EduGraf.Geometries;

internal class TexturedGeometry(float[] position, float[] textureUvs)
    : GeometryBase(position), IUvMapping
{
    [Dimension(2)]
    public float[] TextureUv { get; } = textureUvs;
}