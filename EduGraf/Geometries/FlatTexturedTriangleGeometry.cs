namespace EduGraf.Geometries;

internal class FlatTexturedTriangleGeometry(float[] position, uint[] triangles, float[] textureUv)
    : FlatTriangleGeometry(position, triangles), IUvMapping
{
    [Dimension(2)]
    public float[] TextureUv { get; } = textureUv;
}