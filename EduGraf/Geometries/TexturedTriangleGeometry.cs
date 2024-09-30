namespace EduGraf.Geometries;

internal class TexturedTriangleGeometry(float[] position, uint[] triangles, float[] textureUvs)
    : TriangleGeometry(position, triangles), IUvMapping
{
    [Dimension(2)]
    public float[] TextureUv { get; } = textureUvs;
}