namespace EduGraf.Geometries;

internal class TexturedTriangle32Geometry(float[] position, uint[] triangles, float[] textureUvs)
    : Triangle32Geometry(position, triangles), IUvMapping
{
    [Dimension(2)]
    public float[] TextureUv { get; } = textureUvs;
}