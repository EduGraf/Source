namespace EduGraf.Geometries;

internal class TexturedTriangle16NormalGeometry(float[] position, float[] normals, ushort[] triangles, float[] textureUvs)
    : Triangle16NormalGeometry(position, normals, triangles), IUvMapping
{
    [Dimension(2)]
    public float[] TextureUv { get; } = textureUvs;
}