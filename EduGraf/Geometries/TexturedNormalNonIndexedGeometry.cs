namespace EduGraf.Geometries;

internal class TexturedNormalNonIndexedGeometry(float[] position, float[] normals, float[] textureUvs)
    : NormalNonIndexedGeometry(position, normals), IUvMapping
{
    [Dimension(2)]
    public float[] TextureUv { get; } = textureUvs;
}