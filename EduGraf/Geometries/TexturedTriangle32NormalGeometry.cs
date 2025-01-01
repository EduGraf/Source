namespace EduGraf.Geometries;

internal class TexturedTriangle32NormalGeometry(float[] position, float[] normals, uint[] triangles, float[] textureUvs)
    : Triangle32NormalGeometry(position, normals, triangles), IUvMapping
{
    [Dimension(2)]
    public float[] TextureUv { get; } = textureUvs;
}