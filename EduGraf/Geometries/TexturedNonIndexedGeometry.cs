namespace EduGraf.Geometries;

internal class TexturedNonIndexedGeometry(float[] position, float[] textureUvs)
    : NonIndexedGeometry(position), IUvMapping
{
    [Dimension(2)]
    public float[] TextureUv { get; } = textureUvs;
}