namespace EduGraf.Geometries;

internal class TexturedTriNormalGeometry(float[] position, float[] normals, ushort[] triangles, float[] textureUvs)
    : TriNormalGeometry(position, normals, triangles), IUvMapping
{
    [Dimension(2)]
    public float[] TextureUv { get; } = textureUvs;
}