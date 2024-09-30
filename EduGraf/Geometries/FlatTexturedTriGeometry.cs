namespace EduGraf.Geometries;

internal class FlatTexturedTriGeometry(float[] position, ushort[] triangles, float[] textureUv)
    : FlatTriGeometry(position, triangles), IUvMapping
{
    [Dimension(2)]
    public float[] TextureUv { get; } = textureUv;
}