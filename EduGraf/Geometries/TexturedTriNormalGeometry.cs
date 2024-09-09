namespace EduGraf.Geometries;

internal class TexturedTriNormalGeometry : TriNormalGeometry, IUvMapping
{
    [Dimension(2)]
    public float[] TextureUv { get; }

    public TexturedTriNormalGeometry(float[] position, float[] normals, ushort[] triangles, float[] textureUvs)
        : base(position, normals, triangles)
    {
        TextureUv = textureUvs;
    }
}