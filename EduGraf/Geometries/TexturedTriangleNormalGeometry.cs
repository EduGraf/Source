namespace EduGraf.Geometries;

internal class TexturedTriangleNormalGeometry(float[] position, float[] normals, uint[] triangles, float[] textureUvs)
    : TriangleNormalGeometry(position, normals, triangles), IUvMapping
{
    [Dimension(2)]
    public float[] TextureUv { get; } = textureUvs;
}