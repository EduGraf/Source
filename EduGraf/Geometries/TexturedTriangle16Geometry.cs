namespace EduGraf.Geometries;

internal class TexturedTriangle16Geometry(float[] position, ushort[] triangles, float[] textureUvs)
    : Triangle16Geometry(position, triangles), IUvMapping
{
    [Dimension(2)]
    public float[] TextureUv { get; } = textureUvs;
}