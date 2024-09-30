namespace EduGraf.Geometries;

internal class TexturedNormalGeometry(float[] position, float[] normals, float[] textureUvs)
    : NormalGeometry(position, normals), IUvMapping
{
    [Dimension(2)]
    public float[] TextureUv { get; } = textureUvs;
}