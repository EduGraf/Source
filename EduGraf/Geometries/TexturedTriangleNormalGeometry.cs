namespace EduGraf.Geometries;

internal class TexturedTriangleNormalGeometry : TriangleNormalGeometry, IUvMapping
{
    [Dimension(2)]
    public float[] TextureUv { get; }

    public TexturedTriangleNormalGeometry(float[] position, float[] normals, uint[] triangles, float[] textureUvs)
        : base(position, normals, triangles)
    {
        TextureUv = textureUvs;
    }
}