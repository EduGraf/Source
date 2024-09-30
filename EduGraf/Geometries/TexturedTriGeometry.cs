namespace EduGraf.Geometries;

internal class TexturedTriGeometry(float[] position, ushort[] triangles, float[] textureUvs)
    : TriGeometry(position, triangles), IUvMapping
{
    [Dimension(2)]
    public float[] TextureUv { get; } = textureUvs;
}